using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OilMagnate.StageScene
{
    public class TimerReciever : MonoBehaviour
    {

        [SerializeField]
        Text timerText;

        StageManager stage;

        private void Start()
        {
            stage = StageManager.Instance;

            stage.TimeManager.RegisterTimerEvent(StageTimeManager.TimerStatus.Start, OnStartTimer);
            stage.TimeManager.RegisterTimerEvent(StageTimeManager.TimerStatus.During, OnDuringTimer);
            stage.TimeManager.RegisterTimerEvent(StageTimeManager.TimerStatus.End, OnEndTimer);

            timerText.enabled = false;
        }

        private void OnEndTimer()
        {
            timerText.text = $"0:00";
        }

        private void OnDuringTimer()
        {
            var time = stage.TimeManager.RemainsTimeAsTimeSpan;
            timerText.text = $"{time.Minutes:00}:{time.Seconds:00}";
        }

        private void OnStartTimer()
        {
            timerText.enabled = true;

            var remainsTime = stage.TimeManager.RemainsTimeAsTimeSpan;
            timerText.text = $"{remainsTime.Minutes:00}:{remainsTime.Seconds:00}";
        }
    }
}