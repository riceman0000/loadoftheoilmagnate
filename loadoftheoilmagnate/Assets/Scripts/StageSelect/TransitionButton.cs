using System;
using UnityEngine;
using UnityEngine.UI;

namespace OilMagnate.StageSelect
{
    /// <summary>
    /// 次へボタン
    /// </summary>
    public class TransitionButton : ManagedMono
    {
        [SerializeField]
        [Header("対応させるボタン")]
        Button _button;
        /// <summary>
        /// 使用するImageコンポーネント
        /// </summary>
        [SerializeField]
        [Header("使用するImageコンポーネント")]
        Image _image;

        /// <summary>
        /// デフォルトの「次へ」画像
        /// </summary>
        [SerializeField]
        [Header(" デフォルトの「次へ」画像")]
        Sprite _defaultSprite;

        /// <summary>
        /// ステージボタンが選択されている時の画像
        /// </summary>
        [SerializeField]
        [Header("ステージボタンが選択されている時の画像")]
        Sprite _hilightedSprite;


        public ButtonState CurrentState { get; private set; } = ButtonState.None;
        public Button Button => _button;

        private void Start()
        {
            UpdateImage();
        }

        public void ChangeState(ButtonState nextState)
        {
            CurrentState = nextState;
            UpdateImage();
        }

        private void UpdateImage()
        {
            switch (CurrentState)
            {
                case ButtonState.None:
                    _image.sprite = _defaultSprite;
                    break;
                case ButtonState.Selected:
                    _image.sprite = _hilightedSprite;
                    break;
                default:
                    break;
            }
        }
    }
}