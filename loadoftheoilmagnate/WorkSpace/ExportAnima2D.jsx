/********************************************************************
        Export Anima2D 
        2018.05.25 (c) SEGA
********************************************************************/

// 定数
var EXPORT_SIZEX = 512;  // キャラの出力サイズ X
var EXPORT_SIZEY = 512;  // キャラの出力サイズ Y
var PREFIX_EXPORT_DIR = "-parts"   // 出力ディレクトリの末尾名
var PREFIX_EXPORT_FILE = "-partspos"  // 出力JSONファイルの末尾名

main();

function main() {

    // Photoshopの基本設定の変更
    var originalRulerUnit = preferences.rulerUnits;
    preferences.rulerUnits = Units.PIXELS;

    // レイヤー名のエラーチェック
    if (checkLayer(activeDocument) == false) {
        // エラーがあれば終了
        return false
    }

    // 現在のドキュメントの情報を取得
    var filename = activeDocument.name.split(".")[0];
    var pathCurrentDir = activeDocument.path;
    var pathExportDir = pathCurrentDir + "/" + filename + PREFIX_EXPORT_DIR;
    var layerData = [];  // JSON出力用レイヤーデータ(名前, 順番, x座標, y座標)

    // エクスポートディレクトリ内を空にする
    cleanDir(pathExportDir);

    // エクスポート作業用にドキュメントを複製
    doc = activeDocument.duplicate();

    // レイヤーセットを結合
    //（レイヤースタイルを使用している場合、ドキュメントをリサイズすると効果の掛かり方が変わってしまうので、リサイズ前に結合する）
    margeLayerSets(doc);

    // 非表示のレイヤーを削除
    deleteHideLayers(doc);

    //ドキュメントのリサイズ
    doc.resizeImage(EXPORT_SIZEX, EXPORT_SIZEY);

    //画像の中心を求める(ドキュメントの幅の半分を中心とする)
    var w = doc.width.value;
    var h = doc.height.value;
    var docCenter = Math.round(w / 2);

    // 不要画像範囲の削除
    activeDocument.crop([0, 0, w, h]);

    // レイヤーを全て非表示に
    hideAllLayers(doc);

    //レイヤー一覧の取得
    for (i = 0; i < doc.layers.length; i++) {

        var layer = doc.layers[i];

        // エクスポート対象レイヤーの表示
        doc.activeLayer = layer;
        layer.visible = true;

        // レイヤー番号を求める（一番下のレイヤーを0番）
        var order = doc.layers.length - i - 1;

        // 画像のポジションを求める
        var x = 0;
        var y = 0;
        try {

            // 選択範囲のセンターを求める
            loadSelection();
            var cx = (doc.selection.bounds[0].value + doc.selection.bounds[2].value) / 2;
            var cy = (doc.selection.bounds[1].value + doc.selection.bounds[3].value) / 2;

            // ドキュメントの中心を原点とした時の座標を求める
            var x = (cx - docCenter);
            var y = (docCenter - cy);

        }
        catch (e) {
        }

        layerData[i] = [doc.layers[i].name, order, x, y];

        //エクスポート(png)
        historyIndex = storeHistory();
        activeDocument.trim(TrimType.TRANSPARENT, true, true, true, true);
        saveAsPng(activeDocument, pathExportDir, layer.name);
        restoreHistory(historyIndex);
        layer.visible = false;

    }

    // JSONデータの出力
    pathJSON = pathCurrentDir + "/" + filename + PREFIX_EXPORT_FILE + ".json"
    writeJSON(pathJSON, layerData);

    // エクスポート用仮ドキュメントのクローズ
    doc.close(SaveOptions.DONOTSAVECHANGES);

    // 変更したPhotoshopの設定を元に戻す
    preferences.rulerUnits = originalRulerUnit;

    return true;

}

/*===================================
    レイヤー名のチェック
===================================*/
function checkLayer(doc) {

    var err = [];
    var layernames = {}

    for (i = 0; i < doc.layers.length; i++) {

        //2バイト文字がないか判定
        layername = doc.layers[i].name;
        flg = layername.match(/[^A-Za-z0-9_]+/);
        if (flg) {
            err.push("使用できない文字があります：" + layername);
        }

        // 同名レイヤーが無いか判定
        if (layername in layernames) {
            err.push("同名レイヤーがあります：" + layername);
        } else {
            layernames[layername] = "";  // 同名チェック用に連想配列に仮のキーを登録
        }

    }

    // エラーがあれば表示
    if (err.length > 0) {
        var msg = ""
        for (i = 0; i < err.length; i++) {
            msg = msg + err[i] + "\n";
        }
        alert(msg);
    }

    //戻り値
    //      エラーなし：true
    //      エラーあり：false
    return err.length == 0 ? true : false;

}


/*===================================
    不透明部分の選択範囲を作成
===================================*/
function loadSelection() {

    var desc = new ActionDescriptor();
    var ref = new ActionReference();
    ref.putProperty(charIDToTypeID('Chnl'), charIDToTypeID('fsel'));
    desc.putReference(charIDToTypeID('null'), ref);
    var ref1 = new ActionReference();
    ref1.putEnumerated(charIDToTypeID('Chnl'), charIDToTypeID('Chnl'), charIDToTypeID('Trsp'));
    desc.putReference(charIDToTypeID('T   '), ref1);
    desc.putBoolean(charIDToTypeID('Invr'), false);
    executeAction(charIDToTypeID('setd'), desc, DialogModes.NO);

}


/*===================================
    レイヤーセットを結合
===================================*/
function margeLayerSets(doc) {

    var doc = activeDocument;

    // レイヤーセットの収集     
    var layersets = deepcopy(doc.layerSets)

    // レイヤーセットの結合
    for (var i = 0; i < layersets.length; i++) {
        var tmpVisible = layersets[i].visible;
        app.activeDocument.activeLayer = layersets[i];
        var idMrgtwo = charIDToTypeID("Mrg2");
        executeAction(idMrgtwo, undefined, DialogModes.NO);
        app.activeDocument.activeLayer.visible = tmpVisible;
    }

}



/*===================================
        非表示のレイヤーを削除
===================================*/
function deleteHideLayers(doc) {

    var tmpLayers = deepcopy(doc.layers);
    for (i = 0; i < tmpLayers.length; i++) {
        if (tmpLayers[i].visible == false) {
            tmpLayers[i].remove();
        }
    }
}


/*===================================
        全レイヤーを非表示に設定
===================================*/
function hideAllLayers(doc) {
    for (i = 0; i < doc.layers.length; i++) {
        doc.layers[i].visible = false;
    }
}



/*===================================
    PNGで保存
===================================*/
function saveAsPng(doc, path, filename) {

    // フォルダが存在しない場合作成
    var folder = new Folder(path);
    if (!folder.exists) {
        folder.create();
    }

    // PNG保存オプション
    var pngOpt = new PNGSaveOptions();
    pngOpt.interlaced = false;

    // 保存
    var file = new File(folder.fsName + "/" + filename);
    doc.saveAs(file, pngOpt, true, Extension.LOWERCASE);

}

/*===================================
    JSONファイルの書き出し
===================================*/
function writeJSON(path, data) {

    var jsonArray = [];
    var templateJSON = '\t"LAYERNAME":{"index":INDEX, "x":VALUEX, "y":VALUEY}';
    for (i = 0; i < data.length; i++) {
        tmpJSON = templateJSON;
        tmpJSON = tmpJSON.replace("LAYERNAME", data[i][0]);
        tmpJSON = tmpJSON.replace("INDEX", data[i][1]);
        tmpJSON = tmpJSON.replace("VALUEX", data[i][2]);
        tmpJSON = tmpJSON.replace("VALUEY", data[i][3]);
        jsonArray.push(tmpJSON);
    }
    json = "{\n" + jsonArray.join(",\n") + "\n}\n";

    // JSONデータの出力
    var file = new File(path);
    file.remove();
    file.open("w", "TEXT");
    file.lineFeed = "\n";
    file.write(json);
    file.close();

}



/*===================================
    配列のディープコピー
===================================*/
function deepcopy(array_data) {
    var arr = [];
    for (i = 0; i < array_data.length; i++) {
        arr.push(array_data[i]);
    }
    return arr;
}


/*===================================
    現在のヒストリーインデックスを取得
===================================*/
function storeHistory() {

    return activeDocument.historyStates.length - 1;

}

function restoreHistory(historyIndex) {

    activeDocument.activeHistoryState = activeDocument.historyStates[historyIndex];
}



/*===================================
    ディレクトリの中身を空にする
===================================*/
function cleanDir(path) {

    fld = new Folder(path);
    if (fld.exists) {
        fileList = fld.getFiles();
        for (i = 0; i < fileList.length; i++) fileList[i].remove();
    }

}