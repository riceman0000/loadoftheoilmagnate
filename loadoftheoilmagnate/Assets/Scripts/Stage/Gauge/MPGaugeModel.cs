using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using OilMagnate.Player;

namespace OilMagnate.StageScene
{
    public class MPGaugeModel : ManagedMono
    {
        /// <summary>
        /// View
        /// </summary>
        [SerializeField]
        MPGaugeView _view;

        /// <summary>
        /// ゲージアニメーションに掛ける時間
        /// </summary>
        [SerializeField]
        [Header("ゲージアニメーションに掛ける時間")]
        float _animSpeed = .1f;

        /// <summary>
        /// ゲージのパラメータークラス
        /// </summary>
        MPParameter _param;

        /// <summary>
        /// Gaugeの同期中かどうか
        /// </summary>
        bool isSyncing = false;

        /// <summary>
        /// 上書きするかどうか
        /// </summary>
        bool isOverwrite = false;

        /// <summary>
        /// ビュー用の比率を入れておくバッファー
        /// </summary>
        float _viewRatioBuffer = 0f;

        /// <summary>
        /// ロジックの比率を入れておくバッファー
        /// </summary>
        float _modelRatioBuffer = 0f;

        /// <summary>
        /// ゲージの情報テキスト
        /// </summary>
        public string GaugeInfo
        {
            get
            {
                var current = Param.Current >= 0f ? Param.Current : 0f;
                return $"{current:#######0}$ / {Param.Max:#######0}$";
            }
        }

        /// <summary>
        /// Animation中かどうか
        /// </summary>
        public bool IsAnimating => isSyncing;

        public MPParameter Param { get => _param; set => _param = value; }

        protected override void Awake()
        {
            base.Awake();
            StageManager.Instance.SetMPGauge(this);
        }

        /// <summary>
        /// 初期化処理。このクラスを使う前に必ず呼ぶ。
        /// </summary>
        /// <param name="param"></param>
        public void Initialize(MPParameter param)
        {
            Param = new MPParameter(param);
            _modelRatioBuffer = _viewRatioBuffer = Param.Ratio;

            _view.UpdateGauge(GaugeInfo, Param.Ratio, Param.Ratio - Param.LayerBorderRatio
                , Param.Ratio, Param.Ratio - Param.LayerBorderRatio);

            Param.Current = Param.Max;
        }

        public async void UpdateGauge(AnimPattern animPattern)
        {
            if (isSyncing)
            {
                isOverwrite = true;
            }
            await UniTask.WaitWhile(() => isSyncing);

            isOverwrite = false;
            isSyncing = true;

            var viewRatio = _viewRatioBuffer;
            var modelRatio = _modelRatioBuffer;

            var gaugeAnimDisposable = new SingleAssignmentDisposable();
            gaugeAnimDisposable.Disposable = this.UpdateAsObservable()
               .Select(_ => viewRatio - Param.Ratio)
               .Subscribe(diff =>
               {
                   viewRatio += Time.deltaTime * -diff;
                   modelRatio = Param.Ratio;
                   float layer1, layer2, damage1, damage2;
                   layer1 = layer2 = damage1 = damage2 = 0f;

                   switch (animPattern)
                   {
                       case AnimPattern.Consume:
                           layer1 = modelRatio;
                           damage1 = viewRatio;
                           _view.UpdateGauge(GaugeInfo, layer1Ratio: layer1, damage1Ratio: damage1);
                           break;
                       case AnimPattern.Health:
                           layer1 = modelRatio;
                           damage1 = viewRatio;
                           _view.UpdateGauge(GaugeInfo, layer1Ratio: layer1, damage1Ratio: damage1);
                           break;
                       case AnimPattern.Growth:
                           layer1 = viewRatio;
                           damage1 = modelRatio;
                           _view.UpdateGauge(GaugeInfo, layer1Ratio: layer1, damage1Ratio: damage1);
                           break;
                       default:
                           break;
                   }

                   // アニメーション途中で値の更新が入った場合は現在進行中のアニメーションを中断して新しくアニメーション再生。
                   if (isOverwrite)
                   {
                       _viewRatioBuffer = viewRatio;
                       _modelRatioBuffer = Param.Ratio;
                       isSyncing = false;
                       gaugeAnimDisposable.Dispose();
                       return;
                   }

                   if (Mathf.Abs(diff) < 0.01f)
                   {
                       _viewRatioBuffer = viewRatio;
                       _modelRatioBuffer = modelRatio;
                       _view.UpdateGauge(GaugeInfo, Param.Ratio, Param.Ratio - Param.LayerBorderRatio,
                           Param.Ratio, Param.Ratio - Param.LayerBorderRatio);
                       isSyncing = false;
                       gaugeAnimDisposable.Dispose();
                       return;
                   }
               });
        }


        /// <summary>
        /// MPゲージ
        /// </summary>
        public enum AnimPattern
        {
            Consume,
            Health,
            Growth,
        }
    }
}