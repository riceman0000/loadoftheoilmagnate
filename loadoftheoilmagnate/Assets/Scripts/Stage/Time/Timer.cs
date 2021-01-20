using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using OilMagnate.StageScene;

namespace OilMagnate
{
    public class Timer 
    {
        private TimeParameter timeParam = null;

        public Timer() { }
        public Timer(TimeParameter timeParam)
        {
            this.timeParam = timeParam;
        }

        /// <summary>
        /// Event issued when the countdown starts.
        /// </summary>
        public event Action OnStartTimer = () => { };

        /// <summary>
        /// Event issued during the countdown.
        /// </summary>
        public event Action OnDuringTimer = () => { };

        /// <summary>
        /// Event issued when the countdown ends.
        /// </summary>
        public event Action OnEndTimer = () => { };

        /// <summary>
        /// TimeLimit.
        /// </summary>
        public int TimeLimit { get; private set; } = 0;

        /// <summary>
        /// Current elapsed time.
        /// </summary>
        public float RemainsTime { get; private set; } = 0f;

        /// <summary>
        /// Is the timer running.
        /// </summary>
        public bool IsTimerRunning { get; private set; } = false;


        /// <summary>
        /// 毎フレーム時間経過させる。
        /// タイムリミット
        /// Add timer every frame.
        /// </summary>
        public void UpdateTimer()
        {
            if (!IsTimerRunning) return;

            RemainsTime -= Time.deltaTime;
            OnDuringTimer?.Invoke();

            if (RemainsTime <= 0)
            {
                OnEndTimer?.Invoke();
                IsTimerRunning = false;
            }
        }
        /// <summary>
        /// タイマースタート
        /// </summary>
        /// <param name="timeLimit"></param>
        /// <param name="delayTime"></param>
        public void StartTimer(int timeLimit)
        {
            RemainsTime = TimeLimit = timeLimit;
            IsTimerRunning = true;
            OnStartTimer?.Invoke();
        }

        /// <summary>
        /// <see cref="timeParam"/> の情報を使用してタイマースタート
        /// </summary>
        public void StartTimer()
        {
            if(timeParam ==null)throw new NullReferenceException($"timePramはnull");
            StartTimer(timeParam.TimeLimit);
        }

 

        /// <summary>
        /// 指定秒数遅延してからタイマースタート
        /// </summary>
        /// <param name="timeLimit"></param>
        /// <param name="delayTime"></param>
        public async void DelayStartTimerAsync(int timeLimit, TimeSpan delayTime)
        {
            await UniTask.Delay(delayTime);

            RemainsTime = TimeLimit = timeLimit;
            IsTimerRunning = true;
            OnStartTimer?.Invoke();
        }

        /// <summary>
        /// <see cref="timeParam"/> の情報を使用して遅延タイマースタート
        /// </summary>
        public void DelayStartTimerAsync()
        {
            if (timeParam == null) throw new NullReferenceException($"timePramはnull");
            DelayStartTimerAsync(timeParam.TimeLimit, timeParam.StartTimerDelayTime);
        }

        /// <summary>
        /// Pause timer.
        /// </summary>
        public void PauseTimer()
        {
            IsTimerRunning = false;
        }

        /// <summary>
        /// Resume timer.
        /// </summary>
        public void ResumeTimer()
        {
            IsTimerRunning = true;
        }
    }
}
