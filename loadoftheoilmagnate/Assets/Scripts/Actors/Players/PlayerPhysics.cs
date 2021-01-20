using UnityEngine;
using OilMagnate.StageScene;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using static OilMagnate.Player.Player;

namespace OilMagnate.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerPhysics : ManagedMono, IReflective, IContactable
    {

        Player _player;

        Rigidbody2D _rb2d;

        /// <summary>
        /// Threshold for deciding whether to enable shock waves.
        /// 衝撃波を有効にするかどうか決めるしきい値
        /// </summary>
        float _shockWaveThreshold;

        /// <summary>
        /// ギミックに弾かれてから石油に触れるないしは接地する迄true.
        /// </summary>
        bool isReflecting = false;



        /// <summary>
        /// The Velocity.
        /// </summary>
        public Vector3 Velocity { get => _rb2d.velocity; set => _rb2d.velocity = value; }

        /// <summary>
        /// ANCHOR: ひとまずtrueにしておく。
        /// </summary>
        public bool IsBreakable => true;

        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<Player>();
            _rb2d = GetComponent<Rigidbody2D>();
            _player.CurrentState.Value = PhysicalState.None;
        }

        private void Start()
        {
            /// Velocityと同じ横方向を向く様にする。
            this.UpdateAsObservable()
                .Select(_ => Velocity.x)
                .Where(velX => /*!isReflecting && */Mathf.Abs(velX) > .5f) // gimmickにぶつかって石油アクションに触れる、もしくは接地する迄方向を変えない。
                .Subscribe(v_x =>
                {
                    var x = v_x >= 0f ? -1f : 1f;
                    transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
                })
                .AddTo(this);

            /// Gimmickとぶつかった時<see cref="OnCollisionGimmick(IGimmick)"/>をキック
            IGimmick gimmick = null;
            this.OnCollisionEnter2DAsObservable()
                .Where(col => col.collider.TryGetComponent(out gimmick))
                .Subscribe(col =>
                {
                    OnCollisionGimmick(gimmick);
                })
                .AddTo(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isReflecting) isReflecting = false;
        }

        /// <summary>
        /// 初期化.
        /// </summary>
        void Initialize()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// <see cref="IGimmick"/>を継承しているクラスと接触した時キック.
        /// 相手の耐久値を下げて、残量に応じて自身を反発させるか相手を吹き飛ばすかする。
        /// </summary>
        /// <param name="gimmick"></param>
        void OnCollisionGimmick(IGimmick gimmick)
        {
            switch (_player.MyData.Status.AttackMode)
            {
                case AttackMode.Attack:
                    gimmick.ReduceDurability(_rb2d, _player.MyData.Status.Attack);
                    break;
                case AttackMode.Velocity:
                    gimmick.ReduceDurability(_rb2d, Velocity);
                    break;
                default:
                    throw new ArgumentException($"{typeof(AttackMode)}がちゃんと設定されてないよ");
            }

            if (gimmick.IsBreakable)
            {
                AttenuateVelocity(gimmick.Param.AttenuationRate);
            }
            else
            {
                Reflection(gimmick.Param.CoefficientOfRestitution);
            }
        }

        public void Knockback()
        {
            _player.CurrentState.Value |= PhysicalState.Knockback;
            if (_player.CurrentState.Value.HasFlag(PhysicalState.Knockback))
            {
                _player.CurrentState.Value ^= PhysicalState.Knockback;
            }

        }

        /// <summary>
        /// 接触相手の反発係数に応じて
        /// </summary>
        public void Reflection(float coefficientOfRestitution)
        {
            isReflecting = true;
            var force = -Velocity.normalized * coefficientOfRestitution;

            _rb2d.AddForce(force, ForceMode2D.Impulse);

            Knockback();
        }

        /// <summary>
        /// 速度を減衰させる。
        /// <see cref="Velocity"/>よりも<paramref name="attenuationRate"/>が大きい場合停止処理に変える。
        /// </summary>
        /// <param name="attenuationRate">減衰値</param>
        void AttenuateVelocity(float attenuationRate)
        {
            if (attenuationRate > Velocity.magnitude)
            {
                attenuationRate = Velocity.magnitude;
            }

            var force = -Velocity.normalized * attenuationRate;
            _rb2d.AddForce(force, ForceMode2D.Impulse);
        }

        /// <summary>
        /// <paramref name="force"/>の力積を与えて自身を吹き飛ばす.
        /// </summary>
        /// <param name="force">力積.</param>
        /// <param name="blowMode "><see cref="GimmickParam.BlowForce"/>を使用するかどうか.</param>
        /// /// <param name="forceMode">力の掛け方. 外力か,力積か.</param>
        public void BlowOff(Vector2 force, BlowMode blowMode = BlowMode.FromParam, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            switch (blowMode)
            {
                case BlowMode.FromParam:
                    // parameterないので適当
                    force = force.normalized * 1f;
                    break;
                case BlowMode.FromVelocity:
                    break;
                default:
                    break;
            }
            //Debug.Log($"force = {force.magnitude}");
            _rb2d.AddForce(force, forceMode);
        }
    }
}
