using System.Net.Mime;
using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;


namespace OilMagnate.StageScene
{   
    public class StageTimeManager : ManagedMono
    {
        /// <summary>
        /// Timer.
        /// </summary>
        Timer _timer = null;

        /// <summary>
        /// TimeParameters.
        /// </summary>
        [SerializeField]
        TimeParameter _timeParam;

        /// <summary>
        /// 今ステージをプレイしているかどうか。
        /// </summary>
        public bool IsPlayingStage => _timer.IsTimerRunning;

        /// <summary>
        /// Parameters of Time 
        /// </summary>
        public TimeParameter TimeParam { get => _timeParam; private set => _timeParam = value; }

        public TimeSpan RemainsTimeAsTimeSpan => TimeSpan.FromSeconds(_timer.RemainsTime);

        public bool IsTimerRunning => _timer.IsTimerRunning;



        /// <summary>
        /// Start timer.
        /// </summary>
        public void StartTimer() => _timer.StartTimer();

        /// <summary>
        /// Stop timer.
        /// </summary>
        public void StopTimer() => _timer.PauseTimer();

        /// <summary>
        /// 指定秒数遅延してタイマースタート。
        /// </summary>
        public void DelayStartTimerAsync() => _timer.DelayStartTimerAsync();

        /// <summary>
        /// タイマーの状態に対応したイベントを登録する
        /// </summary>
        /// <param name="when">いつ</param>
        /// <param name="what">何をさせるか</param>
        public void RegisterTimerEvent(TimerStatus when, Action what)
        {
            switch (when)
            {
                case TimerStatus.Start:
                    _timer.OnStartTimer += what;
                    break;
                case TimerStatus.During:
                    _timer.OnDuringTimer += what;
                    break;
                case TimerStatus.End:
                    _timer.OnEndTimer += what;
                    break;
                default:
                    throw new ArgumentException($"{when}は無効な引数じゃ");
            }
        }

        /// <summary>
        /// 登録した特定のイベントを破棄する。
        /// </summary>
        /// <param name="when"></param>
        /// <param name="what"></param>
        public void DisposeTimerEvent(TimerStatus when, Action what)
        {
            switch (when)
            {
                case TimerStatus.Start:
                    _timer.OnStartTimer -= what;
                    break;
                case TimerStatus.During:
                    _timer.OnDuringTimer -= what;
                    break;
                case TimerStatus.End:
                    _timer.OnEndTimer -= what;
                    break;
                default:
                    throw new ArgumentException($"{when}は無効な引数じゃ");
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _timer = new Timer(_timeParam);
        }

        public override void MUpdate()
        {
            _timer.UpdateTimer();
        }

        /// <summary>
        /// タイマーの状態
        /// </summary>
        public enum TimerStatus
        {
            Start,
            During,
            End,
        }
    }
}
