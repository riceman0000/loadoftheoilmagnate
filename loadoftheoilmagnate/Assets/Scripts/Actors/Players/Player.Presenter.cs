using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using OilMagnate.StageScene;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.Player
{
    /// <summary>
    /// Presenter.
    /// </summary>
    public partial class Player
    {
        [SerializeField]
        GameObject _damageUIView;

        /// <summary>
        /// ほかにもTrigger使うかもしれないからtagでプレイヤーに触ったものを区別。
        /// 床、空中判定用。
        /// Floorタグを床になりうるオブジェクトにつけたい。(要相談)
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            ObjTag tag;

            if (Enum.TryParse(collision.gameObject.tag, out tag))
            {
                switch (tag)
                {
                    case ObjTag.FieldMap:
                        CurrentState.Value |= PhysicalState.Grounded;
                        Animator.SetBool(AnimatorParameters.IsGrounded.ToString(), true);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            ObjTag tag;

            if (Enum.TryParse(collision.gameObject.tag, out tag))
            {
                switch (tag)
                {
                    case ObjTag.FieldMap:
                        CurrentState.Value |= PhysicalState.Grounded;
                        Animator.SetBool(AnimatorParameters.IsGrounded.ToString(), true);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            ObjTag tag;
            if (Enum.TryParse(collision.gameObject.tag, out tag))
            {
                switch (tag)
                {
                    case ObjTag.FieldMap:
                        CurrentState.Value ^= PhysicalState.Grounded;
                        Animator.SetBool(AnimatorParameters.IsGrounded.ToString(), false);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Play damage ui animation.
        /// </summary>
        /// <param name="amount"></param>
        public void PlayDamageUI(long amount)
        {
            var go = Instantiate(_damageUIView, transform.position, Quaternion.identity);
            var damageUIView = go.GetComponent<DamageUIView>();
            damageUIView.PlayDamageUI(amount);
            var disposable = new SingleAssignmentDisposable();

            //disposable.Disposable = this.UpdateAsObservable()
            //    .Where()
        }

        //float _buffer;
        //bool _firstGate =true;
        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    IGimmick gimmick;
        //    if (!CurrentState.Value.HasFlag(PhysicalState.Grounded)) return;
        //    if (!collision.transform.TryGetComponent(out gimmick)) return;

        //    _buffer += Time.deltaTime;
        //    if (_buffer > SandwichedThreshold && _firstGate)
        //    {
        //        _firstGate = false;
        //        Debug.Log($"圧死だああああああ");
        //        Animator.SetBool(AnimatorParameters.ToDeath.ToString(), true);
        //        StageManager.Instance.GameOver();
        //    }
        //}

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    _buffer = 0f;
        //}
    }
}
