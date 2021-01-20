using UnityEngine;

namespace OilMagnate.StageScene
{
    public class ResultState : MonoBehaviour
    {
        [SerializeField]
        ResultView _resultView;

        [SerializeField]
        ResultManager _resultManager;

        [SerializeField]
        bool _use;

        StageManager _stage;


        private void Start()
        {
            _stage = StageManager.Instance;
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnResultEnter);
        }

        /// <summary>
        /// Result呼び出し
        /// </summary>
        /// <param name="state"></param>
        private void OnResultEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.Result)) return;

            if (_use)
            {
                _resultManager.gameObject.SetActive(true);
                _resultManager.Initialize();
            }
            else
            {
                _resultView.gameObject.SetActive(true);
                _resultView.SetResult(_stage.MPGauge.Param);
            }
        }
    }
}
