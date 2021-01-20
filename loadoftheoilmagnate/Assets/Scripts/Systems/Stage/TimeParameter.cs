using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// Parameter of <see cref="Timer"/>
    /// </summary>
    [Serializable]
    public class TimeParameter
    {
        [SerializeField]
        [Header("シナリオ終了してからタイマーを開始する迄の遅延時間")]
        float _startTimerDelayTime;

        [SerializeField]
        [Header("タイマーの制限時間")]
        int _timeLimit;

        /// <summary>
        /// シナリオ終了してからタイマーを開始する迄の遅延時間
        /// </summary>
        public TimeSpan StartTimerDelayTime => TimeSpan.FromSeconds(_startTimerDelayTime);
        /// <summary>
        /// タイマーの制限時間
        /// </summary>
        public int TimeLimit => _timeLimit;
    }
}