using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using System;

namespace OilMagnate.StageScene
{
    public class Missile : ManagedMono
    {
        
        GameObject Player;
        Rigidbody2D rig;
        [SerializeField] float Flyspeed, angleSpeed;
        Vector3 pos;
        
        StageManager _stageManager;

        void Start()
        {
            rig = this.GetComponent<Rigidbody2D>();

            Player = GameObject.FindGameObjectWithTag("Player");

            StartCoroutine(GetPlayer());
            _stageManager = StageManager.Instance;

            _stageManager.StateMachine.RegisterStateEvent(StageStateMachine.When.Execute, PursuitTarget);


        }

        private void OnDisable()
        {
            _stageManager.StateMachine.DisposeStateEvent(StageStateMachine.When.Execute, PursuitTarget);
        }

        private void PursuitTarget(StageStateMachine.StageState state)
        {

            if (!state.HasFlag(StageStateMachine.StageState.InStage)) return;

            
            //ミサイルとプレイヤーの位置を比べる
            var playerGapMissile = (this.transform.position - pos).normalized;
            //上の比べたものから角度を出す(Radiun角)
            var n_missileAngle = (Mathf.Atan2(playerGapMissile.y, playerGapMissile.x) * Mathf.Rad2Deg) - 90f;
            //Quanternion型に変換
            var angle_Euler = Quaternion.Euler(0f, 0f, n_missileAngle);
            //こいつの角度を変更する。

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angle_Euler, angleSpeed);

            GoAhead();
        }

        IEnumerator GetPlayer()
        {
            
            while (true)
            {
                yield return new WaitForSeconds(3f);
                pos = Player.transform.position;
            }
        }

        void GoAhead() => transform.Translate(-transform.up*Flyspeed,Space.World);
    }
}