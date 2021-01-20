using System;
using UnityEngine;
using OilMagnate.StageScene;

namespace OilMagnate.Player
{

    [Serializable]
    [CreateAssetMenu(fileName = "PlayerStatus", menuName = "Player/Status")]
    public class Status : ScriptableObject
    {
        [SerializeField]
        [Header("マネーパワーのパラメータ")]
        MPParameter _mPParam;

        /// <summary>speed</summary>
        [SerializeField]
        [Header("移動スピード")]
        float _speed = 1f;

        /// <summary>
        /// スワイプ検知の閾値
        /// </summary>
        [SerializeField]
        [Header("スワイプ検知の閾値")]
        float _swipeThreshold = 1f;

        [SerializeField]
        [Header("プレイヤーの攻撃力")]
        float _attack = 1f;

        [SerializeField]
        [Header("ノックバック状態の時間")]
        float _knockbackTime = 1f;

        [SerializeField]
        [Header("速度か攻撃力どっちを使うか")]
        AttackMode _attackMode = AttackMode.Attack;


        //[SerializeField]
        //[Header("収入を得るインターバル(秒数)")]
        //float incomeInterval;

        //[SerializeField]
        //[Header("一回に得る不労所得(総資産からのパーセンテージ)")]
        //float incomeRate;

        /// <summary>
        /// MPゲージのパラメータ。
        /// </summary>
        public MPParameter MPParam { get => _mPParam; private set => _mPParam = value; }
        public float MoveSpeed { get => _speed; private set => _speed = value; }
        public float SwipeThreshold { get => _swipeThreshold; private set => _swipeThreshold = value; }
        public float Attack { get => _attack; private set => _attack = value; }
        public float KnockbackTime { get => _knockbackTime; private set => _knockbackTime = value; }
        internal AttackMode AttackMode { get => _attackMode;private set => _attackMode = value; }

        ///// <summary>
        ///// 収入を得るインターバル(秒数)
        ///// </summary>
        //public float IncomeInterval { get => incomeInterval; private set => incomeInterval = value; }

        ///// <summary>
        ///// 一回に得る不労所得(総資産からのパーセンテージ)
        ///// </summary>
        //public float IncomeRate { get => incomeRate; private set => incomeRate = value; }

    }

    internal enum AttackMode
    {
        Attack,
        Velocity,
    }
}