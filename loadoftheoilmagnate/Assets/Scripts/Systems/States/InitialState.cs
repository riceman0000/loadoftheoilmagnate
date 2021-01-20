using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;


namespace OilMagnate.StageScene
{
    public class InitialState : MonoBehaviour
    {
        [SerializeField]
        [Header("開始のシナリオを再生するかどうか")]
        bool _useInitialADV = false;

        /// <summary>
        /// Initial scenario player.
        /// </summary>
        [SerializeField]
        ADVplayer _initialADV = null;

        [SerializeField]
        float _startDelayTimeAtEndADV = 3f;

        StageManager _stage;

        private void Awake()
        {
            _stage = StageManager.Instance;
            _initialADV.gameObject.SetActive(false);
            if (_useInitialADV)
            {
                _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, PlayInitialScenario);
                _initialADV.OnScenarioEnd += InitialADV_OnScenarioEnd;
            }
            else
            {
                InitialADV_OnScenarioEnd();
            }
        }

        private async void InitialADV_OnScenarioEnd()
        {
            await UniTask.DelayFrame(1);

            _stage.PlayStage();
            _stage.TimeManager.StartTimer();
        }

        void PlayInitialScenario(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InitialADV)) return;
            _initialADV.gameObject.SetActive(true);
        }

    }
}