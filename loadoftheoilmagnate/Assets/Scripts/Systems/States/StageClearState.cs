using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using System;

namespace OilMagnate.StageScene
{
    public class StageClearState : MonoBehaviour
    {

        [SerializeField]
        [Header("クリア時のシナリオを再生するかどうか")]
        bool _useEndlADV = false;

        /// <summary>
        /// Initial scenario player.
        /// </summary>
        [SerializeField]
        ADVplayer _endADV = null;

        [SerializeField]
        float _startDelayTimeAtEndADV = 3f;

        StageManager _stage;


        private void Start()
        {
            _stage = StageManager.Instance;
            _stage.StateMachine.RegisterStateEvent
                (StageStateMachine.When.Enter, OnStageClearEnter);

            _endADV.gameObject.SetActive(false);
            _endADV.OnScenarioEnd += _endADV_OnScenarioEnd;
        }

        private void _endADV_OnScenarioEnd()
        {
            _stage.Result();
        }

        private async void OnStageClearEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.StageClear)) return;

            _stage.Player.MyData.UpdateProgress(SceneFader.Instance.CurrentScene);

            if (_useEndlADV)
            {
                //await UniTask.Delay(TimeSpan.FromSeconds(_startDelayTimeAtEndADV));
                _endADV.gameObject.SetActive(true);
            }
            else
            {
                await UniTask.DelayFrame(1);
                _stage.Result();
            }
        }
    }
}