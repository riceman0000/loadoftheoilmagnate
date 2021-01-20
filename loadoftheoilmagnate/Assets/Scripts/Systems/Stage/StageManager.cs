using System.Net.Mime;
using UnityEngine;
using System.Collections;
using OilMagnate.Player;
using System;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;

namespace OilMagnate.StageScene
{
    public class StageManager : ManagedMono
    {
        #region Singleton   
        static StageManager instance;

        public static StageManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType(typeof(StageManager)) as StageManager;
                    if (!instance)
                    {
                        throw new NullReferenceException(nameof(StageManager));
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// Parameters of Stage.
        /// </summary>
        [SerializeField]
        StageParameter _stageParam = null;

        /// <summary>
        /// Time manager in the stage.
        /// </summary>
        [field: SerializeField]
        public StageTimeManager TimeManager { get; private set; }

        /// <summary>
        /// State machine in the stage.
        /// </summary>
        [field: SerializeField]
        public StageStateMachine StateMachine { get; private set; }

        /// <summary>
        /// Gimmick observer.
        /// </summary>
        [field: SerializeField]
        public GimmickObserver GimmickObserver { get; private set; }

        public MPGaugeModel MPGauge { get; private set; }

        public Player.Player  Player { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            Player = GameObject.FindGameObjectWithTag(ObjTag.Player.ToString()).GetComponent<Player.Player>();
        }

        void Start()
        {
            StateMachine.ChangeState(_stageParam._FirstState);

            SetEvents();
            Player.MyData.UpdateProgress(SceneTitle.Title);
            Player.MyData.Save();
        }

        public void SetMPGauge(MPGaugeModel mpGauge) => MPGauge = mpGauge;




        private void SetEvents()
        {
            // ゲームクリア時、ゲームオーバー時、

            StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnStageClearEnter);
            StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnStateGameOverEnter);

            TimeManager.RegisterTimerEvent(StageTimeManager.TimerStatus.End, OnTimerEnd);
        }


        /// <summary>
        /// タイマーが終了した時イベント処理を行う
        /// </summary>
        private void OnTimerEnd()
        {

        }

        private void OnStateGameOverEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.GameOver)) return;

            TimeManager.StopTimer();
        }

        /// <summary>
        /// ステージクリア時のイベント処理
        /// </summary>
        /// <param name="state"></param>
        private void OnStageClearEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.StageClear)) return;

            TimeManager.StopTimer();
        }


        /// <summary>
        /// Game over.
        /// </summary>
        public void GameOver()
        {
            Debug.Log($"GameOver.");
            StateMachine.ChangeState(StageStateMachine.StageState.GameOver);
        }

        public void Clear()
        {
            //Debug.Log($"Stage clear.");
            StateMachine.ChangeState(StageStateMachine.StageState.StageClear);
        }

        public void Result()
        {
            //Debug.Log($"Result");
            StateMachine.ChangeState(StageStateMachine.StageState.Result);
        }

        public void Pause()
        {
            //Debug.Log($"Will be pause.");
            StateMachine.ChangeState(StageStateMachine.StageState.Pause);
        }

        public void PlayStage()
        {
            //Debug.Log($"In the stage");
            StateMachine.ChangeState(StageStateMachine.StageState.InStage);
        }

        public void PlayInitialADV()
        {
            //Debug.Log($"Play initial ADV.");
            StateMachine.ChangeState(StageStateMachine.StageState.InitialADV);
        }

        [Serializable]
        public class StageParameter
        {
            [Header("最初に呼出すステート。")]
            public StageStateMachine.StageState _FirstState;

        }
    }
}