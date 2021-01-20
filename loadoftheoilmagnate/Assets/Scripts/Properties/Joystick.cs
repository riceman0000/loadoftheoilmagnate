using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace OilMagnate
{
    /// <summary>
    /// Joystick
    /// </summary>
    public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IInputProvider
    {
        /// <summary>x成分</summary>
        public float Horizontal => inputVector.x != 0f ? inputVector.x : Input.GetAxis("Horizontal");
        /// <summary>y成分</summary>
        public float Vertical => inputVector.y != 0f ? inputVector.y : Input.GetAxis("Vertical");
        /// <summary>ハンドルのベクトル</summary>
        public Vector2 Direction => new Vector2(Horizontal, Vertical);


        /// <summary>ジョイスティックハンドルの可動範囲</summary>
        [Header("Options")]
        [Range(0f, 2f)] [SerializeField] protected float handleLimit = 1f;
        /// <summary>ジョイスティックモード</summary>
        [SerializeField] protected JoystickMode joystickMode = JoystickMode.Both;
        /// <summary>ジョイスティックの背景</summary>
        [Header("Components")]
        [SerializeField] protected RectTransform background;
        /// <summary>ジョイスティックのハンドル</summary>
        [SerializeField] protected RectTransform handle;

        /// <summary>入力されたジョイスティックの方向</summary>
        protected Vector2 inputVector = Vector2.zero;
        Vector2 joystickPosition = Vector2.zero;

        [SerializeField] RectTransform myself;



        /// <summary>
        /// ドラッグされた時
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            var dir = eventData.position - joystickPosition;
            var fac = background.sizeDelta.x / 2f;
            inputVector = (dir.magnitude > fac) ? dir.normalized : dir / (fac);
            ClampJoystick();
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        }

        internal void ResetInput()
        {
            inputVector = Vector2.zero;
        }

        /// <summary>
        /// タッチした時
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            joystickPosition = eventData.position;
        }

        /// <summary>
        /// タッチを離した時
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerUp(PointerEventData eventData) { inputVector = handle.anchoredPosition = Vector2.zero; }
        /// <summary>
        /// JoystickModeに合わせて稼働方向に制限を与える
        /// </summary>
        protected void ClampJoystick()
        {
            switch (joystickMode)
            {
                case JoystickMode.Horizontal:
                    inputVector = new Vector2(inputVector.x, 0f);
                    break;
                case JoystickMode.Vertical:
                    inputVector = new Vector2(0f, inputVector.y);
                    break;
                case JoystickMode.Both:
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// ジョイスティックのモード
    /// </summary>
    public enum JoystickMode
    {
        /// <summary>水平のみ</summary>
        Horizontal,
        /// <summary>垂直のみ</summary>
        Vertical,
        /// <summary>両方</summary>
        Both,
    }
}