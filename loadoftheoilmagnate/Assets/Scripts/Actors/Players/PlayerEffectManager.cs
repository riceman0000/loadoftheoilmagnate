using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OilMagnate.StageScene;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using static OilMagnate.Player.Player;
using System;

namespace OilMagnate.Player
{
    public class PlayerEffectManager : ManagedMono
    {


        [SerializeField]
        ParticleSystem _trajectoryOfMoney;

        Player _player;

        StageStateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<Player>();
            _stateMachine = StageManager.Instance.StateMachine;
        }

        //private void Start()
        //{
        //    var e = _player.CurrentState
        //        .Where(state => _stateMachine.CurrentState.HasFlag(StageStateMachine.StageState.InStage))
        //        .Subscribe(state =>
        //        {
        //            if (!state.HasFlag(PhysicalState.Grounded))
        //            {
        //                _trajectoryOfMoney.Play();
        //            }
        //            else
        //            {
        //                _trajectoryOfMoney.Stop();
        //            }
        //        }).AddTo(this);
        //}
    }
}