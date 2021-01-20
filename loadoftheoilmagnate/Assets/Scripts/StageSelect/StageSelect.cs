using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OilMagnate.Player;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using System;
using System.Linq;
using UnityEngine.Events;

namespace OilMagnate.StageSelect
{
    public class StageSelect : MonoBehaviour
    {
        /// <summary>
        /// ステージセレクトのボタン群
        /// </summary>
        [SerializeField]
        [Header("ステージセレクトのボタン群")]
        List<StageButton> _stageButtons;

        /// <summary>
        /// 次へボタン
        /// </summary>
        [SerializeField]
        [Header("次へボタン")]
        TransitionButton _transitionButton;

        /// <summary>
        /// 現在選択されているボタン。
        /// </summary>
        StageButton _currentSelectedButton = null;

        /// <summary>
        /// いずれかのステージボタンを押している状態か否か。
        /// </summary>
        public bool IsPressingAnyStageButton => _stageButtons.Any(button => button.CurrentState.HasFlag(ButtonState.Selected));

        private void Awake()
        {
            foreach (var sb in _stageButtons)
            {
                sb.Button.onClick.AddListener(() =>
                {
                    if (_currentSelectedButton != null)
                    {
                        _currentSelectedButton.ChangeState(ButtonState.None);
                    }

                    _currentSelectedButton = sb;
                    _currentSelectedButton.ChangeState(ButtonState.Selected);
                    _transitionButton.ChangeState(ButtonState.Selected);
                });
            }

            _transitionButton.Button.onClick.AddListener(() =>
            {
                if (_currentSelectedButton == null)
                {
                    return;
                }
                if (_currentSelectedButton.CurrentState.HasFlag(ButtonState.Selected))
                {
                    _currentSelectedButton.TransitionScene();
                }
            });
        }
    }

    //ボタンが押された時のいべんｔ

    /// <summary>
    /// Button ステート。
    /// </summary>
    public enum ButtonState
    {
        None = 1,
        Selected = 2,
    }
}