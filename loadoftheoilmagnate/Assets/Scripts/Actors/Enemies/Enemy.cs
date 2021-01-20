using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OilMagnate.Player;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// Enemyの基底クラス。
    /// </summary>
    /// <typeparam name="TEnemy">継承した敵クラス</typeparam>
    /// <typeparam name="TEnum">その敵が使うステートの<see cref="Enum"/></typeparam>
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
    public abstract class Enemy<TEnemy, TEnum> : StatefulObject<TEnemy, TEnum>, IDamagable, IDestructible, IAttacable
        where TEnemy : Enemy<TEnemy, TEnum>
        where TEnum : Enum, IConvertible
    {
        public event Action<Collision2D> OnColEnter2D = v => { };
        public event Action<Collision2D> OnColStay2D = v => { };
        public event Action<Collision2D> OnColExit2D = v => { };
        public event Action<Collider2D> OnTriEnter2D = v => { };
        public event Action<Collider2D> OnTriStay2D = v => { };
        public event Action<Collider2D> OnTriExit2D = v => { };

        /// <summary>
        /// ステータス
        /// </summary>   
        public EnemyStatus Status { get; protected set; }

        /// <summary>
        /// Playerの<see cref="Transform"/>
        /// </summary>
        public Transform Player
        {
            get
            {
                if (!player) throw new NullReferenceException("playerはnullだお");
                return player;
            }
        }

        public BoxCollider2D m_detectionCensor
        {
            get
            {
                return detectionCensor;
            }
            set
            {
                m_detectionCensor = value;
            }
        }

        /// <summary>
        /// ステータスのマスターデータ。
        /// </summary>
        /// <remarks>
        /// 実際に使うデータは<see cref="status"/>に<see cref="Object.Instantiate(Object)"/>して使う
        /// </remarks>
        [SerializeField] [Header("パラメーターのマスタデータ")] protected EnemyStatus masterData;
        /// <summary>索敵用コライダー</summary>
        [SerializeField] [Header("索敵用コライダー")] protected BoxCollider2D detectionCensor;
        /// <summary>攻撃判定用コライダー</summary>
        [SerializeField] [Header("攻撃判定用コライダー")] protected Collider2D AttackCollider;

        /// <summary>Animator</summary>
        protected Animator animator;

        protected static Transform player;

        /// <summary>
        /// Initialize instance.
        /// </summary>
        protected sealed override void Awake()
        {
            base.Awake();
            Status = Instantiate(masterData);
            animator = GetComponent<Animator>();
            if (!player) player = GameObject.FindGameObjectWithTag($"Player").GetComponent<Transform>();
            OnAwake();
        }

        #region Colliders
        protected void OnCollisionEnter2D(Collision2D collision) => OnColEnter2D?.Invoke(collision);
        protected void OnCollisionStay2D(Collision2D collision) => OnColStay2D?.Invoke(collision);
        protected void OnCollisionExit2D(Collision2D collision) => OnColExit2D?.Invoke(collision);
        protected void OnTriggerEnter2D(Collider2D collision) => OnTriEnter2D?.Invoke(collision);
        protected void OnTriggerStay2D(Collider2D collision) => OnTriStay2D?.Invoke(collision);
        protected void OnTriggerExit2D(Collider2D collision) => OnTriExit2D?.Invoke(collision);
        #endregion

        protected abstract void OnAwake();

        /// <summary>
        /// Damage受けた時の処理
        /// </summary>
        public abstract void TakeDamage(int amount);
        /// <summary>
        /// Damage受けた時の処理
        /// </summary>
        public abstract void TakeDamage(float amount);

        /// <summary>
        /// 自分自身の破壊処理
        /// </summary>
        public abstract void Destruction();

        /// <summary>
        /// 攻撃処理
        /// </summary>
        public abstract void Attack();

        /// <summary>
        /// <see cref="Animator"/>のステートマシンのパラメーター。
        /// </summary>
        /// <remarks>
        /// <see cref="string"/>の代わりに<see cref="Object.ToString()"/>して使う。
        /// </remarks>
        public enum AnimationParameter
        {
            /// <summary>攻撃する</summary>
            Attack,
            /// <summary>ダメージを受ける</summary>
            TakeDamage,
            /// <summary>死ぬ</summary>
            ToDeath,
            /// <summary>移動しているかどうか</summary>
            IsMoving,
            /// <summary>浮遊</summary>
            IsFlowing,
        }
    }
}