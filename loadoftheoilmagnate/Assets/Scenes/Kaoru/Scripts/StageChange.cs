using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OilMagnate.Player;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using System;

namespace OilMagnate
{
    public class StageChange : MonoBehaviour
    {

        Image _image;

        Button _button;

        [SerializeField]
        private Sprite _pressedButton;

        [SerializeField]
        private Sprite _disableButton;

        [SerializeField]
        private Sprite _intaractableButton;

        [SerializeField]
        List<StageEnable> _stageButtons;

        public ReactiveProperty<SceneTitle> TargetStage { get; set; }
             = new ReactiveProperty<SceneTitle>();

        public void SceneFade()
        {
            SEManager.Instance.SEPlay(SETag.Select);
            _image.sprite = _pressedButton;
            SceneFader.Instance.FadeOut(TargetStage.Value, 1f);
        }

        private void Start()
        {
            _button = this.gameObject.GetComponent<Button>();
            _image = gameObject.GetComponent<Image>();
            _button.interactable = false;
            _image.sprite = _disableButton;
        }
        public void IfOn()
        {
            _button = GetComponent<Button>();
            _button.interactable = true;
            _image.sprite = _intaractableButton;
        }

        public void ToggleFrameVisible(StageEnable stageEnable)
        {
            foreach (var se in _stageButtons)
            {
                se.FrameSetActive(stageEnable == se);
            }
        }
    }

}

