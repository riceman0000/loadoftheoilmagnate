using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using UnityEngine.UI;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// Damage ui view.
    /// </summary>
    public class DamageUIView : MonoBehaviour
    {
        /// <summary>
        /// Textをアニメーションさせるコンポーネント
        /// </summary>
        [SerializeField]
        TypefaceAnimator _typefaceAnimator;

        /// <summary>
        /// 表示するテキストコンポーネント
        /// </summary>
        [SerializeField]
        Text _text;

        ///// <summary>
        ///// プレイ終了時のコールバック
        ///// </summary>
        //public event Action OnComplete = () => { };

        /// <summary>
        /// damage UIのアニメーションを再生する。再生後gameobjectを破棄する。
        /// </summary>
        /// <param name="amount"></param>
        public async void PlayDamageUI(long amount)
        {
            _text.text = $"-{amount}$";
            _typefaceAnimator.Play();

            await UniTask.WaitWhile(() => _typefaceAnimator.isPlaying);
            if (_text)
            {
                _text.text = "";
                Destroy(gameObject);
            }
            //OnComplete?.Invoke();
        }
    }
}