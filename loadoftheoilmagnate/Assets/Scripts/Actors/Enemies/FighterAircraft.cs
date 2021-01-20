using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using OilMagnate.Player;


namespace OilMagnate.StageScene
{
    /// <summary>
    /// 戦闘機
    /// </summary>
    public class FighterAircraft : MonoBehaviour
    {
        [SerializeField]
        Rigidbody2D _rb2d;

        float _reflectionPower = 1f;


        Vector3 _direction;
        Vector3 _buffer;
        public void ThrustFighter(Vector3 dir, float force, float reflectionPower)
        {
            _buffer = transform.position;
            _reflectionPower = reflectionPower;
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    _rb2d.velocity = dir * Time.deltaTime * force;
                    _direction = transform.position - _buffer;
                    _buffer = transform.position;
                })
                .AddTo(this);

            PlayerPhysics player = null;
            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.TryGetComponent(out player))
                .Take(1)
                .Subscribe(_ =>
                {
                    Debug.Log($"{_.gameObject.name}");
                    player.BlowOff(_direction * _reflectionPower, BlowMode.FromVelocity);
                })
                .AddTo(this);
        }
    }
}