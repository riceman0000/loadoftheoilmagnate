using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OilMagnate.StageScene
{
    public class OilGenerator : ManagedMono
    {
        /// <summary>放出中の石油間欠泉群</summary>
        public List<OilGeyser> OilGeysersDuringEmitting { get; private set; } = new List<OilGeyser>();

        /// <summary>
        /// The material of <see cref="OilGeyser"/>.
        /// </summary>
        [SerializeField]
        [Header("油田のぱらめーた")]
        OilGeyserMaterial masterMatData;


        OilGeyserMaterial _oilGeyserMat;

        float _swipeThreshold;

        /// <summary>
        /// 石油間欠泉のprefab
        /// </summary>
        [SerializeField]
        GameObject oilGeyserPrefab;

        public OilGeyserMaterial OilGeyerMat => _oilGeyserMat;

        protected override void Awake()
        {
            base.Awake();
            _oilGeyserMat = Instantiate(masterMatData);
        }

        private void Start()
        {
            _swipeThreshold = StageManager.Instance.Player.MyData.Status.SwipeThreshold;
            var tgd = TouchGestureDetector.Instance;
            tgd.OnDetectGesture += (gesture,touchInfo)=> {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                        OnTouchBegin(touchInfo.LastPosition);
                        break;
                    case TouchGestureDetector.Gesture.TouchMove:
                    case TouchGestureDetector.Gesture.TouchStationary:
                        OnTouchStay(touchInfo.LastPosition);
                        break;
                    case TouchGestureDetector.Gesture.TouchEnd:
                    case TouchGestureDetector.Gesture.Click:
                        OnTouchEnd(touchInfo.LastPosition);
                        break;
                    case TouchGestureDetector.Gesture.FlickTopToBottom:
                        break;
                    case TouchGestureDetector.Gesture.FlickBottomToTop:
                        break;
                    case TouchGestureDetector.Gesture.FlickLeftToRight:
                        break;
                    case TouchGestureDetector.Gesture.FlickRightToLeft:
                        break;
                    default:
                        break;
                }
            };
        }


        private void OnDisable()
        {
            if (_oilGeyserMat != null)
            {
                Destroy(_oilGeyserMat);
            }
        }

        private void OnTouchBegin(Vector2 touchPosition)
        {
            powerLimitBuffer = startPos = endPos = Camera.main.ScreenToWorldPoint(touchPosition);
        }

        /// <summary>タッチ開始座標</summary>
        Vector3 startPos = Vector3.zero;
        /// <summary>スワイプの最終座標</summary>
        Vector3 endPos = Vector3.zero;
        /// <summary><see cref="endPos"/>の前フレームの座標の一時保存する変数</summary>
        Vector3 endPosBuffer = Vector3.zero;
        Vector3 powerLimitBuffer = Vector3.zero;
        private void OnTouchStay(Vector2 touchPosition)
        {
            /// <see cref="Vector3.magnitude"/>が<see cref="limitOilGeyserLength"/>を超えた場合その直前の値でクランプを掛ける
            endPos = Camera.main.ScreenToWorldPoint(touchPosition);
            var distance = endPos - startPos;
            var scalar = distance.magnitude;


            if (scalar <= _oilGeyserMat.LimitOilGeyserPower)
            {
                powerLimitBuffer = endPos;
            }

            if (scalar > _oilGeyserMat.LimitOilGeyserLength)
            {
                endPos = endPosBuffer;
            }
            else
            {
                endPosBuffer = endPos;
            }

        }

        public void GenerateOilGeyser()
        {
            var go = Instantiate(oilGeyserPrefab, Vector3.zero, Quaternion.identity);
            OilGeyser og;
            if (go.TryGetComponent(out og))
            {
                startPos.z = 10f;
                endPos.z = 10f;
                powerLimitBuffer.z = 10f;
                og.InitAndPlay(this, _oilGeyserMat, startPos, endPos, powerLimitBuffer);
            }
            else throw new ArgumentException($"{oilGeyserPrefab}が不正だよ");
        }

        /// <summary>
        /// <see cref="OilGeyser"/>の購読処理
        /// </summary>
        /// <param name="og">購読対象</param>
        public void SubscribeOilGeyser(OilGeyser og)
        {
            OilGeysersDuringEmitting.ForEach(oilGeyser => oilGeyser.ToBackward());
            OilGeysersDuringEmitting.Add(og);
            OnSubscribe();
        }

        /// <summary>
        /// <see cref="OilGeyser"/>の購読解除
        /// </summary>
        /// <param name="og">購読削除対象</param>
        public void UnsubscribeOilGeyser(OilGeyser og)
            => OilGeysersDuringEmitting.Remove(og);

        /// <summary>
        /// Subscribe時のコールバック関数
        /// </summary>
        private void OnSubscribe()
        {
            // ラインのリミットを超えた時古いラインから消していく
            if (OilGeysersDuringEmitting.Count > _oilGeyserMat.MaximumLineCount)
            {
                var targetOG = OilGeysersDuringEmitting.First(og => !og.IsCalledAtteunation);
                var mat = targetOG.gameObject.GetComponent<LineRenderer>().material;
                targetOG.StopAllCoroutines();
                StartCoroutine(targetOG.AttenuateOilGeyser());
            }
        }

        public void OnTouchEnd(Vector2 eventData)
        {
            if (!StageManager.Instance.StateMachine.CurrentState.HasFlag(StageStateMachine.StageState.InStage)) return;
            var magnitude = (endPos - startPos).magnitude;

            if (magnitude > _swipeThreshold)
            {
                GenerateOilGeyser();
            }
        }

        /// <summary>
        /// 石油アクションアニメーションのモード
        /// </summary>
        public enum Mode
        {
            /// <summary>噴き出す</summary>
            Emmition,
            /// <summary>減衰する<summary>
            Attenuation,
        }
    }
}