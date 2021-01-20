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
    public class ResultManager : MonoBehaviour
    {
        [SerializeField]
        TypefaceAnimator _textAnimator;

        [SerializeField]
        Text _textBox;

        [SerializeField]
        MPGaugeModel _gauge;

        [SerializeField]
        string _firstScript;

        StageManager _stage;

        MPParameter _param;

        private void Awake()
        {
            _stage = StageManager.Instance;
        }

        public void Initialize()
        {
            _param = new MPParameter(_stage.Player.MyData.Status.MPParam.Max, 0L);
            _param.Current = 0;
            _gauge.Initialize(_param);
            RemainMPAnim();
        }

        public async void RemainMPAnim()
        {
            _textBox.text = $"{_firstScript}";
            _textAnimator.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(_textAnimator.duration));
            _param.Current = _stage.Player.MyData.Status.MPParam.Current;
            _gauge.UpdateGauge(MPGaugeModel.AnimPattern.Health);
        }
    }
}