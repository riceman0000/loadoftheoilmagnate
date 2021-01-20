using Anima2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using OilMagnate.StageScene;

namespace OilMagnate.Player
{
    public partial class Player : ManagedMono
    {

        public Vector2 Direction { get; private set; }
        public bool IsDebug { get => _isDebug; set => _isDebug = value; }

        [SerializeField]
        bool _isDebug;

        #region CallBacks
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Start()
        {

            var events = new TouchGestureDetector.EventMaterial[]
            {
                new TouchGestureDetector.EventMaterial(
                    TouchGestureDetector.Gesture.TouchMove| TouchGestureDetector.Gesture.TouchStationary,DrawDebugLine),
            };
            _touchGestureDetector.RegisterGestureEvent(events);
            //PassiveIncome();
        }


        private void Initialize()
        {
            SaveDataCheck();
            _rb2d = GetComponent<Rigidbody2D>();
            _touchGestureDetector = TouchGestureDetector.Instance;
            _playerData = Game.Instance.SaveData;
            _inputProvider = FindObjectOfType(typeof(Joystick)) as Joystick;
            _oilGenerator = FindObjectOfType(typeof(OilGenerator)) as OilGenerator;
            MPManager = GetComponent<PlayerMPManager>();

            _inputProvider.gameObject.SetActive(false);

            _stage = StageManager.Instance;
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnInStageEnter);
            _stage.StateMachine.RegisterStateEvent(StageStateMachine.When.Exit, OnInStageExit);
        }

        private void OnInStageExit(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            _inputProvider.ResetInput();
            _inputProvider.gameObject.SetActive(false);
        }

        private void OnInStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;
            _inputProvider.gameObject.SetActive(true);
        }



        private void SaveDataCheck()
        {
            _playerData = PlayerData.Instance;
            if (!IsDebug) return;
            if (_initialStatus == null) throw new NullReferenceException("PlayerData.InitialStatusを設定して！");
            _playerData.SetStatus(Instantiate(_initialStatus));
            _playerData.Status.MPParam.Current = _playerData.Status.MPParam.Max;
        }
        public override void MFixedUpdate()
        {
            var isMovable = _inputProvider?.Direction != Vector2.zero;

            if (isMovable)
            {
                Move();
            }
            Animator.SetBool(AnimatorParameters.IsMoving.ToString(), isMovable);
        }

        #endregion



        /// <see cref="PlayerController"/>のステータスのビットフィールド
        /// </summary>
        [Flags]
        public enum PhysicalState
        {
            None = 0,
            Moving = 1,
            Grounded = 2,
            Knockback = 4,
        }

        /// <summary>
        /// Playerの<see cref="UnityEngine.Animator"/>のパラメーター. 
        /// </summary>
        /// <remarks>
        /// <see cref="object.ToString()"/>して使う。
        /// </remarks>
        enum AnimatorParameters
        {
            IsMoving,
            IsGrounded,
            Attack,
            Damage,
            ToDeath,
        }
    }
}