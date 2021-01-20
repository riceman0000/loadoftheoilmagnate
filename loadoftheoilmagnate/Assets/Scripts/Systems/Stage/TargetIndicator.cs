using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// The target indicator.
    /// </summary>
    public class TargetIndicator : ManagedMono
    {
        /// <summary>
        /// ターゲットからの距離
        /// </summary>
        float _distanceFromTarget;

        /// <summary>
        /// 追跡対象。
        /// </summary>
        IGimmick _target = null;

        /// <summary>
        /// Player
        /// </summary>
        Transform _player = null;

        StageManager _stageManager;

        SpriteRenderer _renderer;


        /// <summary>
        /// 差分
        /// </summary>
        public Vector3 Diff => _target.Transform.position - _player.position;

        /// <summary>
        /// 方向
        /// </summary>
        public Vector3 Direction => Diff.normalized;

        /// <summary>
        /// 距離
        /// </summary>
        public float Distance => Diff.magnitude;

        /// <summary>
        /// 初期化したかどうか.
        /// </summary>
        public bool IsInitialized { get; private set; } = false;
        SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="target"></param>
        public void Initialize(Transform player, IGimmick target)
        {
            _player = player;
            _target = target;
            IsInitialized = true;
            _stageManager = StageManager.Instance;
            _renderer = GetComponent<SpriteRenderer>();
            //_renderer.enabled = false;

            if (_stageManager.StateMachine.CurrentState.HasFlag(StageStateMachine.StageState.InStage))
            {
                _renderer.enabled = true;
            }
            _stageManager.StateMachine.RegisterStateEvent(StageStateMachine.When.Enter, OnInStageEnter);
            _stageManager.StateMachine.RegisterStateEvent(StageStateMachine.When.Execute, OnInStageExecute);
            _stageManager.StateMachine.RegisterStateEvent(StageStateMachine.When.Exit, OnInStageExit);

            this.UpdateAsObservable()
                .First(_ => _target.IsBreakable)
                .Subscribe(_ =>
                {
                    Destroy(gameObject);
                })
                .AddTo(this);
        }

        private void OnDisable()
        {
            if (!_stageManager) return;
            _stageManager.StateMachine.DisposeStateEvent(StageStateMachine.When.Enter, OnInStageEnter);
            _stageManager.StateMachine.DisposeStateEvent(StageStateMachine.When.Execute, OnInStageExecute);
            _stageManager.StateMachine.DisposeStateEvent(StageStateMachine.When.Exit, OnInStageExit);

            if (!disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// enable sprite renderer.
        /// </summary>
        /// <param name="state"></param>
        private void OnInStageEnter(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage))
            {
                return;
            }
            _renderer.enabled = true;
        }

        /// <summary>
        /// Pursuit target.
        /// </summary>
        /// <param name="state"></param>
        private void OnInStageExecute(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;

            var pos = (_target.Transform.position - Direction) * _distanceFromTarget;
            var q = Quaternion.LookRotation(Diff);

            transform.position = pos;
            transform.rotation = q;
        }

        /// <summary>
        /// disable sprite renderer.
        /// </summary>
        /// <param name="state"></param>
        private void OnInStageExit(StageStateMachine.StageState state)
        {
            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;

            _renderer.enabled = false;
        }
    }
}
