﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace OilMagnate
{
    /// <summary>
    /// Touch gesture detector for Unity.
    /// </summary>
    /// <author>KAKO Akihito</author>
    /// <email>kako@qnote.co.jp</email>
    /// <license>MIT License</license>
    public class TouchGestureDetector : MonoSingleton<TouchGestureDetector>
    {

        const float FLICK_TIME_LIMIT = 0.3f;

        /// <summary>
        /// Gesture.
        /// </summary>
        [Flags]
        public enum Gesture
        {
            /// <summary>The touch begin.</summary>
            TouchBegin = 1,
            /// <summary>The touch move.</summary>
            TouchMove = 1 << 1,
            /// <summary>The touch stationary.</summary>
            TouchStationary,
            /// <summary>The touch end.</summary>
            TouchEnd = 1 << 2,
            /// <summary>The click.</summary>
            Click = 1 << 3,
            /// <summary>The flick top to bottom.</summary>
            FlickTopToBottom = 1 << 4,
            /// <summary>The flick bottom to top.</summary>
            FlickBottomToTop = 1 << 5,
            /// <summary>The flick left to right.</summary>
            FlickLeftToRight = 1 << 6,
            /// <summary> flick right to left.</summary>
            FlickRightToLeft = 1 << 7,
        }

        /// <summary>タッチジェスチャーのイベントシステム</summary>
        public event Action<Gesture, TouchInfo> OnDetectGesture = null;
        /// <summary>メインカメラ</summary>
        public Camera ShootingCamera;
        /// <summary>フリック検知を有効にするかどうかを切り替えるフラグ</summary>
        public bool DetectFlick = true;
        /// <summary>TouchInfo:タッチの情報を格納するリスト</summary>
        List<TouchInfo> TouchInfos = new List<TouchInfo>();
        /// <summary>割る数。画面フリックのsensitivity(感度)を決める値</summary>
        public float Divisor = 10f;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            // Sceneをロードした時onGestureDetectedのlistenerを全て消去する
            SceneManager.sceneLoaded += (scene, sceneMode) =>
            {
                OnDetectGesture = null; //listenerをリセット
            };

            if (null == ShootingCamera) // カメラオブジェクトがnullの場合メインカメラを代入する
            {
                ShootingCamera = Camera.main;
            }
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        void Update()
        {
            foreach (var touch in Input.touches) // 毎フレームタッチされた本数分判定する
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began: // タッチされた瞬間のフレーム呼ばれる
                        OnTouchBegin(touch.fingerId, touch.position);
                        break;
                    case TouchPhase.Moved: // タッチされている間のフレームずっと呼ばれる
                        OnTouchMove(touch.fingerId, touch.position);
                        break;
                    case TouchPhase.Stationary: // タッチされて動いていない間のフレームずっと呼ばれる
                        OnTouchStationary(touch.fingerId);
                        break;
                    case TouchPhase.Canceled: // タッチが認識出来ない時(eg.本数多すぎ、手のひらで触る等)呼ばれる
                    case TouchPhase.Ended: // 離す時最後に触れていたフレームで呼ばれる
                        OnTouchEnd(touch.fingerId, touch.position);
                        break;
                }
            }
            if (Input.touchCount == 0) // タッチが0の時
            {
                if (Input.GetMouseButtonDown(0)) //左クリック押したら
                {
                    OnTouchBegin(Int32.MaxValue, Input.mousePosition);
                }
                else if (Input.GetMouseButtonUp(0)) // 左クリックを離したら
                {
                    OnTouchEnd(Int32.MaxValue, Input.mousePosition);
                }
                else if (Input.GetMouseButton(0))//左クリックを押している間
                {
                    OnTouchMove(Int32.MaxValue, Input.mousePosition);
                }
            }
        }

        /// <summary>
        /// Ons the touch begin.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        /// <param name="position">Position.</param>
        void OnTouchBegin(int fingerId, Vector2 position)
        {
            var touchInfo = new TouchInfo(fingerId, position); // touch情報が入っている変数
            TouchInfos.Add(touchInfo); //touchInfoをリストに追加
            OnGestureDetected(Gesture.TouchBegin, touchInfo); // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す
        }

        /// <summary>
        /// Ons the touch move.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        /// <param name="position">Position.</param>
        void OnTouchMove(int fingerId, Vector2 position)
        {
            var touchInfo = TouchInfos.FirstOrDefault(x => x.fingerId == fingerId); // touchInfosリストの先頭要素が引数のtouchInfoのfingerIdと同値だった場合その要素を返す,でなければnullを返す。
            if (null == touchInfo) // touchInfoがnullならmethod終了
            {
                return;
            }
            touchInfo.AddPosition(position); // touchInfoをリストに追加する
            OnGestureDetected(Gesture.TouchMove, touchInfo); // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す
        }

        /// <summary>
        /// Ons the touch stationary.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        void OnTouchStationary(int fingerId)
        {
            var touchInfo = TouchInfos.FirstOrDefault(x => x.fingerId == fingerId); // touchInfosリストの先頭要素が引数のtouchInfoのfingerIdと同値だった場合その要素を返す,でなければnullを返す。
            if (null == touchInfo) // touchInfoがnullならmethod終了
            {
                return;
            }
            OnGestureDetected(Gesture.TouchStationary, touchInfo); // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す
        }

        /// <summary>
        /// Ons the touch end.
        /// </summary>
        /// <param name="fingerId">Finger identifier.</param>
        /// <param name="position">Position.</param>
        void OnTouchEnd(int fingerId, Vector2 position)
        {
            var touchInfo = TouchInfos.FirstOrDefault(x => x.fingerId == fingerId); // touchInfosリストの先頭要素が引数のtouchInfoのfingerIdと同値だった場合その要素を返す,でなければnullを返す。
            if (null == touchInfo) // touchInfoがnullならmethod終了
            {
                Debug.Log("touchInfo = nullだよ");
                return;
            }
            touchInfo.AddPosition(position); // そのフレームでタッチしている座標touchInfoのpositionリストに追加する
            OnGestureDetected(Gesture.TouchEnd, touchInfo);  // 呼び出し元でAddListenerで登録されたmethodに引数を渡してInvokeで呼び出す

            var diff = touchInfo.Diff; // タッチ始めのフレームと離す瞬間のフレーム時の差分ベクトルを代入する
            var flickDistanceLimit = (float)Math.Min(Screen.width, Screen.height) / Divisor; //画面の縦横で短い方を任意の値で割った数値を代入する

            // フリック検知フラグがON,且つタッチを始めてからの経過時刻がタイムリミット以内,且つxかyのタッチ差分が任意の閾値を超えた時
            if (DetectFlick && touchInfo.ElapsedTime < FLICK_TIME_LIMIT && (Mathf.Abs(diff.x) > flickDistanceLimit || Mathf.Abs(diff.y) > flickDistanceLimit))
            {
                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y)) // タッチ差分がy軸よりx軸方が大きい時
                {
                    if (diff.x < 0f) // x軸がマイナス方向だったら
                    {
                        OnGestureDetected(Gesture.FlickRightToLeft, touchInfo); // 引数を左フリックにする
                    }
                    else
                    {
                        OnGestureDetected(Gesture.FlickLeftToRight, touchInfo);// でなければ引数を右フリックにする
                    }
                }
                else // y軸の方が大きい時
                {
                    if (diff.y < 0f) // y軸がマイナス方向だったら
                    {
                        OnGestureDetected(Gesture.FlickTopToBottom, touchInfo); // 引数を下フリックにする
                    }
                    else
                    {
                        OnGestureDetected(Gesture.FlickBottomToTop, touchInfo); // でなければ引数を上フリックにする
                    }
                }
            }
            else // ジェスチャーがフリックを検知しなかったら
            {
                OnGestureDetected(Gesture.Click, touchInfo); // 引数をクリックにする
            }

            TouchInfos.RemoveAll(x => x.fingerId == fingerId); // 引数のfingerIdと同値だったらリストから削除する
        }

        public void RegisterGestureEvent(EventMaterial eventMat)
        {
            OnDetectGesture += ((gesture, touchinfo) =>
            {
                if (eventMat.TargetGesture.HasFlag(gesture))
                {
                    eventMat.EventAction?.Invoke(touchinfo);
                }
            });
        }

        public void RegisterGestureEvent(params EventMaterial[] eventMats)
        {
            foreach (var mat in eventMats)
            {
                RegisterGestureEvent(mat);
            }
        }

        public class EventMaterial
        {
            public Gesture TargetGesture { get; private set; }
            public Action<TouchInfo> EventAction { get; private set; }

            public EventMaterial(Gesture gesture, Action<TouchInfo> action)
            {
                this.TargetGesture = gesture;
                this.EventAction = action;
            }
        }

        /// <summary>
        /// Ons the gesture detected.
        /// </summary>
        /// <param name="gesture">Gesture.</param>
        /// <param name="touchInfo">Touch info.</param>
        void OnGestureDetected(Gesture gesture, TouchInfo touchInfo)
        {
            OnDetectGesture?.Invoke(gesture, touchInfo);
        }

        /// <summary>
        /// Touch info.
        /// </summary>
        public class TouchInfo
        {
            /// <summary>
            /// Gets the difference.
            /// 二次元座標の差分
            ///  </summary>
            /// <value>The diff.</value>
            public Vector2 Diff
            {
                get
                {
                    return new Vector2(positions.Last().x - positions.First().x, positions.Last().y - positions.First().y); // 最初にタップした位置から最後にタップしていた位置の差分ベクトルを返す
                }
            }

            public Vector2 LastPosition { get { return positions.Last(); } }

            /// <summary>Read only touch positions.</summary>
            public IReadOnlyList<Vector2> Positions => positions;

            /// <summary>経過時間</summary>
            /// <value>The elapsed time.</value>
            public float ElapsedTime
            {
                get
                {
                    return Time.time - startTime;
                }
            }
            /// <summary>The finger identifier.</summary>
            public readonly int fingerId; // どのタッチかを識別するユニーク値

            /// <summary>The start time.</summary>
            float startTime; // インスタンス生成した瞬間のゲーム経過時刻
            /// <summary>The positions.</summary>
            List<Vector2> positions = new List<Vector2>();

            /// <summary>
            ///Touch情報を入れておくクラス
            ///  </summary>
            /// <param name="fingerId">Finger identifier.</param>
            /// <param name="position">Touch position.</param>
            public TouchInfo(int fingerId, Vector2 position)
            {
                this.fingerId = fingerId; // touch.fingerIdを代入して初期化
                startTime = Time.time; // タッチ開始時間を記録する
                AddPosition(position); //TouchInfoリストに追加する
            }

            /// <summary>
            /// Adds the position to List.
            /// positionsリストにタッチ座標を追加する
            /// </summary>
            /// <param name="position">Position.</param>
            public void AddPosition(Vector2 position)
            {
                positions.Add(position);
            }


            /// <summary>
            /// タッチ情報を元にレイキャストを飛ばしてGameObjectもUIも当たったオブジェクトを返す
            /// camera引数には指定カメラを引数にする事が可能.何も渡さなかった場合MainCameraタグが付いているカメラを代入する
            /// 戻り値には当たったかどうかの真偽値,out引数に当たったオブジェクトを代入して返す.何も検出出来なかったらnullが入る
            /// </summary>
            /// <returns><c>true</c>, if detection was hit, <c>false</c> otherwise false.</returns>
            /// <param name="targetGameObject">Target game object.</param>
            /// <param name="hitResult">Hit result.</param>
            /// <param name="camera">Camera.</param>
            public bool HitDetection(out GameObject hitResult, GameObject targetGameObject = null, Camera camera = null)
            {
                if (null == camera) // 引数のカメラがnullなら
                {
                    camera = Camera.main; // 最初に検出したメインカメラタグがついているカメラオブジェクトを代入する
                }
                var lastTouchPosition = positions.Last(); // タッチを離す瞬間のフレームの座標

                // UIではないGameObject用のraycast
                var ray2d = new Ray2D(Camera.main.ScreenToWorldPoint(lastTouchPosition), Vector2.zero);// 最後にタッチした座標をRay2Dに変換する
                RaycastHit2D hit = Physics2D.Raycast(ray2d.origin, ray2d.direction, Mathf.Infinity);
                if (hit.collider != null) // Raycastにオブジェクトが検出されたら
                {
                    hitResult = hit.collider.gameObject; // 検出したゲームオブジェクトの参照を代入
                    if (hit.collider.gameObject == targetGameObject || targetGameObject == null) // そのオブジェクトが引数のオブジェクトと一緒,もしくはTargetを指定していない場合
                    {
                        return true; // trueを返す
                    }
                    return false; // 然もなくばfalseを返す
                }

                // UI用のraycast,GameObject用のRaycastで検出出来なかった場合此方で再検査する
                var pointerEventData = new PointerEventData(EventSystem.current) // RaycastAll用の変数
                {
                    position = lastTouchPosition // 指を離す瞬間のフレーム時の座標を代入する
                };
                var raycastResults = new List<RaycastResult>(); // UI用レイキャストの結果をリストで保持する変数
                EventSystem.current.RaycastAll(pointerEventData, raycastResults); // ここでレイキャストを飛ばして当たったオブジェクトをリストに格納する
                if (raycastResults.Count == 0) // UIを1個も検出出来なかったらout引数にnull,returnにfalseを返す
                {
                    hitResult = null;
                    return false;
                }
                var resultGameObject = raycastResults.First().gameObject; // 最初に検出したオブジェクトを代入する

                // Targetを指定していない,且つ何かしらのUIを検出出来た場合そのゲームオブジェクトを返す
                if (targetGameObject == null && resultGameObject != null)
                {
                    hitResult = resultGameObject;
                    return true;
                }

                while (null != resultGameObject) // 検出したオブジェクトがあれば
                {
                    if (resultGameObject == targetGameObject) // 検出したオブジェクトが引数と一緒ならばそのゲームオブジェクトとtrueを返す
                    {
                        hitResult = resultGameObject; // 検出したゲームオブジェクトの参照を代入
                        return true;
                    }
                    var parent = resultGameObject.transform.parent; // 検出したオブジェクトの親が存在するならば、代入
                                                                    // もし検出したオブジェクトに親がいるなら、親を代入してwhile文の最初からやり直し。これを行う事により、特定のCanvas等UIの親に存在するゲームオブジェクトも検出可能
                    resultGameObject = null != parent ? parent.gameObject : null;
                }

                hitResult = null; // Raycastに何も引っかからなかった場合out引数をnull,returnをfalseにする
                return false;
            }
        }

        /// <summary>
        /// Gesture detector event.
        /// methodを登録するクラス
        /// </summary>
        public class GestureDetectorEvent : UnityEvent<Gesture, TouchInfo>
        {
            public GestureDetectorEvent()
            {
            }
        }
    }
}