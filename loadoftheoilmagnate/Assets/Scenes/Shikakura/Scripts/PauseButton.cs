using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OilMagnate.StageScene;
using System;

namespace OilMagnate
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField]
        private Sprite _pauseSelected, _pauseDefault;//ポーズボタンのSprite
        [SerializeField]
        private GameObject PouseMenu;//ポーズボタン作動時に表示するオブジェクト
        private Image _pauseImage;

        StageManager stage;

        SceneFader _sceneFader;

        // Start is called before the first frame update
        void Start()
        {
            _pauseImage = GetComponent<Image>();
            _sceneFader = SceneFader.Instance;
            stage = StageManager.Instance;

            stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnPauseEnter);
            stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnEngStage);
            stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Exit, OnPauseExit);
        }


        private void OnDisable()
        {
            stage?.StateMachine.DisposeStateEvent(StageStateMachine.When.Enter, OnPauseEnter);
            stage?.StateMachine.DisposeStateEvent(StageStateMachine.When.Exit, OnPauseExit);
        }
        private void OnPauseExit(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.Pause)) return;

            Time.timeScale = 1f;
            _pauseImage.sprite = _pauseDefault;
        }

        private void OnPauseEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.Pause)) return;
            Time.timeScale = 0f;
            _pauseImage.sprite = _pauseSelected;
        }
        private void OnEngStage(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.GameOver)
                || !state.HasFlag(StageStateMachine.StageState.StageClear)
                || !state.HasFlag(StageStateMachine.StageState.Result))
            {
                return;
            }

            gameObject.SetActive(false);
        }

        public void OnClick()
        {
            stage.Pause();
            SEManager.Instance.SEPlay(SETag.Select);
            PouseMenu.SetActive(true);
        }
        public void ReturnGame()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            stage.PlayStage();
            PouseMenu.SetActive(false);
        }
        public void ReStart()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            Time.timeScale = 1f;
            _sceneFader.FadeOut(_sceneFader.CurrentScene);
        }
        public void ReturnStage()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            Time.timeScale = 1f;
            _sceneFader.FadeOut(SceneTitle.Title, new Color(0, 0, 0), 1f);
        }
    }
}
