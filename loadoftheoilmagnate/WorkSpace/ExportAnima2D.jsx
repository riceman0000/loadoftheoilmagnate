/********************************************************************
        Export Anima2D 
        2018.05.25 (c) SEGA
********************************************************************/

// �萔
var EXPORT_SIZEX = 512;  // �L�����̏o�̓T�C�Y X
var EXPORT_SIZEY = 512;  // �L�����̏o�̓T�C�Y Y
var PREFIX_EXPORT_DIR = "-parts"   // �o�̓f�B���N�g���̖�����
var PREFIX_EXPORT_FILE = "-partspos"  // �o��JSON�t�@�C���̖�����

main();

function main() {

    // Photoshop�̊�{�ݒ�̕ύX
    var originalRulerUnit = preferences.rulerUnits;
    preferences.rulerUnits = Units.PIXELS;

    // ���C���[���̃G���[�`�F�b�N
    if (checkLayer(activeDocument) == false) {
        // �G���[������ΏI��
        return false
    }

    // ���݂̃h�L�������g�̏����擾
    var filename = activeDocument.name.split(".")[0];
    var pathCurrentDir = activeDocument.path;
    var pathExportDir = pathCurrentDir + "/" + filename + PREFIX_EXPORT_DIR;
    var layerData = [];  // JSON�o�͗p���C���[�f�[�^(���O, ����, x���W, y���W)

    // �G�N�X�|�[�g�f�B���N�g��������ɂ���
    cleanDir(pathExportDir);

    // �G�N�X�|�[�g��Ɨp�Ƀh�L�������g�𕡐�
    doc = activeDocument.duplicate();

    // ���C���[�Z�b�g������
    //�i���C���[�X�^�C�����g�p���Ă���ꍇ�A�h�L�������g�����T�C�Y����ƌ��ʂ̊|��������ς���Ă��܂��̂ŁA���T�C�Y�O�Ɍ�������j
    margeLayerSets(doc);

    // ��\���̃��C���[���폜
    deleteHideLayers(doc);

    //�h�L�������g�̃��T�C�Y
    doc.resizeImage(EXPORT_SIZEX, EXPORT_SIZEY);

    //�摜�̒��S�����߂�(�h�L�������g�̕��̔����𒆐S�Ƃ���)
    var w = doc.width.value;
    var h = doc.height.value;
    var docCenter = Math.round(w / 2);

    // �s�v�摜�͈͂̍폜
    activeDocument.crop([0, 0, w, h]);

    // ���C���[��S�Ĕ�\����
    hideAllLayers(doc);

    //���C���[�ꗗ�̎擾
    for (i = 0; i < doc.layers.length; i++) {

        var layer = doc.layers[i];

        // �G�N�X�|�[�g�Ώۃ��C���[�̕\��
        doc.activeLayer = layer;
        layer.visible = true;

        // ���C���[�ԍ������߂�i��ԉ��̃��C���[��0�ԁj
        var order = doc.layers.length - i - 1;

        // �摜�̃|�W�V���������߂�
        var x = 0;
        var y = 0;
        try {

            // �I��͈͂̃Z���^�[�����߂�
            loadSelection();
            var cx = (doc.selection.bounds[0].value + doc.selection.bounds[2].value) / 2;
            var cy = (doc.selection.bounds[1].value + doc.selection.bounds[3].value) / 2;

            // �h�L�������g�̒��S�����_�Ƃ������̍��W�����߂�
            var x = (cx - docCenter);
            var y = (docCenter - cy);

        }
        catch (e) {
        }

        layerData[i] = [doc.layers[i].name, order, x, y];

        //�G�N�X�|�[�g(png)
        historyIndex = storeHistory();
        activeDocument.trim(TrimType.TRANSPARENT, true, true, true, true);
        saveAsPng(activeDocument, pathExportDir, layer.name);
        restoreHistory(historyIndex);
        layer.visible = false;

    }

    // JSON�f�[�^�̏o��
    pathJSON = pathCurrentDir + "/" + filename + PREFIX_EXPORT_FILE + ".json"
    writeJSON(pathJSON, layerData);

    // �G�N�X�|�[�g�p���h�L�������g�̃N���[�Y
    doc.close(SaveOptions.DONOTSAVECHANGES);

    // �ύX����Photoshop�̐ݒ�����ɖ߂�
    preferences.rulerUnits = originalRulerUnit;

    return true;

}

/*===================================
    ���C���[���̃`�F�b�N
===================================*/
function checkLayer(doc) {

    var err = [];
    var layernames = {}

    for (i = 0; i < doc.layers.length; i++) {

        //2�o�C�g�������Ȃ�������
        layername = doc.layers[i].name;
        flg = layername.match(/[^A-Za-z0-9_]+/);
        if (flg) {
            err.push("�g�p�ł��Ȃ�����������܂��F" + layername);
        }

        // �������C���[������������
        if (layername in layernames) {
            err.push("�������C���[������܂��F" + layername);
        } else {
            layernames[layername] = "";  // �����`�F�b�N�p�ɘA�z�z��ɉ��̃L�[��o�^
        }

    }

    // �G���[������Ε\��
    if (err.length > 0) {
        var msg = ""
        for (i = 0; i < err.length; i++) {
            msg = msg + err[i] + "\n";
        }
        alert(msg);
    }

    //�߂�l
    //      �G���[�Ȃ��Ftrue
    //      �G���[����Ffalse
    return err.length == 0 ? true : false;

}


/*===================================
    �s���������̑I��͈͂��쐬
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
    ���C���[�Z�b�g������
===================================*/
function margeLayerSets(doc) {

    var doc = activeDocument;

    // ���C���[�Z�b�g�̎��W     
    var layersets = deepcopy(doc.layerSets)

    // ���C���[�Z�b�g�̌���
    for (var i = 0; i < layersets.length; i++) {
        var tmpVisible = layersets[i].visible;
        app.activeDocument.activeLayer = layersets[i];
        var idMrgtwo = charIDToTypeID("Mrg2");
        executeAction(idMrgtwo, undefined, DialogModes.NO);
        app.activeDocument.activeLayer.visible = tmpVisible;
    }

}



/*===================================
        ��\���̃��C���[���폜
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
        �S���C���[���\���ɐݒ�
===================================*/
function hideAllLayers(doc) {
    for (i = 0; i < doc.layers.length; i++) {
        doc.layers[i].visible = false;
    }
}



/*===================================
    PNG�ŕۑ�
===================================*/
function saveAsPng(doc, path, filename) {

    // �t�H���_�����݂��Ȃ��ꍇ�쐬
    var folder = new Folder(path);
    if (!folder.exists) {
        folder.create();
    }

    // PNG�ۑ��I�v�V����
    var pngOpt = new PNGSaveOptions();
    pngOpt.interlaced = false;

    // �ۑ�
    var file = new File(folder.fsName + "/" + filename);
    doc.saveAs(file, pngOpt, true, Extension.LOWERCASE);

}

/*===================================
    JSON�t�@�C���̏����o��
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

    // JSON�f�[�^�̏o��
    var file = new File(path);
    file.remove();
    file.open("w", "TEXT");
    file.lineFeed = "\n";
    file.write(json);
    file.close();

}



/*===================================
    �z��̃f�B�[�v�R�s�[
===================================*/
function deepcopy(array_data) {
    var arr = [];
    for (i = 0; i < array_data.length; i++) {
        arr.push(array_data[i]);
    }
    return arr;
}


/*===================================
    ���݂̃q�X�g���[�C���f�b�N�X���擾
===================================*/
function storeHistory() {

    return activeDocument.historyStates.length - 1;

}

function restoreHistory(historyIndex) {

    activeDocument.activeHistoryState = activeDocument.historyStates[historyIndex];
}



/*===================================
    �f�B���N�g���̒��g����ɂ���
===================================*/
function cleanDir(path) {

    fld = new Folder(path);
    if (fld.exists) {
        fileList = fld.getFiles();
        for (i = 0; i < fileList.length; i++) fileList[i].remove();
    }

}