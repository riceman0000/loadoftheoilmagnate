using UnityEngine;
using System.Collections;
using System;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    public class GameOverState : MonoBehaviour
    {
        [SerializeField]
        float _fadeTime = 2f;
        [SerializeField]
        float _delayTime = 1f;

        private void Start()
        {
            StageManager.Instance.StateMachine
                .RegisterStateEvent(StageStateMachine.When.Enter, OnGameOver);
        }

        private async void OnGameOver(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.GameOver)) return;

            await UniTask.Delay(TimeSpan.FromSeconds(_delayTime));

            SceneFader.Instance.FadeOut(SceneTitle.StageSelect, _fadeTime);
        }
    }
}