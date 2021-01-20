using System.Net.Mime;
using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;


namespace OilMagnate.StageScene
{
    /// <summary>
    /// ステージ上のステートマシン。
    /// </summary>
    [DisallowMultipleComponent]
    public class StageStateMachine : ManagedMono
    {
        /// <summary>
        /// ステートが呼ばれたフレームにコールバックされるイベント
        /// </summary>
        public event Action<StageState> OnStatusEnter = ss => { };
        /// <summary>
        /// そのステートの間毎フレームコールバックされるイベント
        /// </summary>
        public event Action<StageState> OnStatusExecute = ss => { };
        /// <summary>
        /// ステートから遷移するフレームにコールバックされるイベント
        /// </summary>
        public event Action<StageState> OnStatusExit = ss => { };

        /// <summary>
        /// 現在のステート
        /// </summary>
        public StageState CurrentState { get; private set; }

        /// <summary>
        /// ステートを遷移させる。
        /// </summary>
        /// <param name="next"></param>
        public void ChangeState(StageState next)
        {
            OnStatusExit?.Invoke(CurrentState);
            CurrentState = next;
            isCalledEnter = false;
        }

        public void RegisterStateEvent(When when, Action<StageState> what)
        {
            switch (when)
            {
                case When.Enter:
                    OnStatusEnter += what;
                    break;
                case When.Execute:
                    OnStatusExecute += what;
                    break;
                case When.Exit:
                    OnStatusExit += what;
                    break;
                default:
                    throw new ArgumentException($"{when}は無効な引数じゃ");
            }
        }

        public void DisposeStateEvent(When when, Action<StageState> what)
        {
            switch (when)
            {
                case When.Enter:
                    OnStatusEnter -= what;
                    break;
                case When.Execute:
                    OnStatusExecute -= what;
                    break;
                case When.Exit:
                    OnStatusExit -= what;
                    break;
                default:
                    throw new ArgumentException($"{when}は無効な引数じゃ");
            }
        }

        /// <summary>
        /// 指定されたStatusと同じステータスかどうか
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsMatchState(StageState status) => CurrentState.HasFlag(status);

        /// <summary>
        /// ステート遷移時Enterを一度だけ呼ぶためのフラグ
        /// </summary>
        bool isCalledEnter = false;

        /// <summary>
        /// 管理されたUpdate関数。
        /// </summary>
        public override void MUpdate()
        {
            if (!isCalledEnter)
            {
                OnStatusEnter?.Invoke(CurrentState);
                isCalledEnter = true;
            }
            OnStatusExecute?.Invoke(CurrentState);

        }

        public enum When
        {
            Enter,
            Execute,
            Exit,
        }

        /// <summary>
        /// ステージ上のステート。
        /// </summary>
        public enum StageState
        {
            InitialADV = 1,
            InStage = 2,
            Pause = 4,
            StageClear = 8,
            GameOver = 16,
            Result = 32,
        }
    }
}