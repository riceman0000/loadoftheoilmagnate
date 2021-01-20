using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    public class MPGaugeView : ManagedMono
    {
        /// <summary>
        /// ゲージレイヤー1
        /// </summary>
        [SerializeField]
        Image _gaugeLayer1;

        /// <summary>
        /// ゲージレイヤー1のダメージゲージ
        /// </summary>
        [SerializeField]
        Image _dagameGaugeLayer1;

        /// <summary>
        /// ゲージレイヤー2
        /// </summary>
        [SerializeField]
        Image _gaugeLayer2;

        /// <summary>
        /// ゲージレイヤー2のダメージゲージ
        /// </summary>
        [SerializeField]
        Image _dagameGaugeLayer2;

        /// <summary>
        /// ゲージの情報を表示するテキスト
        /// </summary>
        [SerializeField]
        Text _gaugeText;


        public void UpdateGauge(string gaugeInfo = "", float layer1Ratio = -100f, float layer2Ratio = -100f, float damage1Ratio = -100f, float damage2Ratio = -100f)
        {
            if (layer1Ratio != -100f) _gaugeLayer1.fillAmount = layer1Ratio;
            if (layer2Ratio != -100f) _gaugeLayer2.fillAmount = layer2Ratio;
            if (damage1Ratio != -100f) _dagameGaugeLayer1.fillAmount = damage1Ratio;
            if (damage2Ratio != -100f) _dagameGaugeLayer2.fillAmount = damage2Ratio;

            if (!string.IsNullOrEmpty(gaugeInfo))
            {
                _gaugeText.text = gaugeInfo;
            }
        }
    }
}