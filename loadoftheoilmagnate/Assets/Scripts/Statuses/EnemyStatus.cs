using System;
using System.Collections;
using UnityEngine;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// Custom asset of Enemy status.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "EnemyStatus", menuName = "Statuses/Enemy")]
    public class EnemyStatus : ScriptableObject
    {        
        /// <summary>hit point</summary>
        public int Hp { get => hp; set => hp = value; }
        /// <summary>power</summary>
        public float PowerRate { get => powerRate; set => powerRate = value; }
        /// <summary>speed</summary>
        public float Speed { get => speed; set => speed = value; }
        /// <summary>AroundRange</summary>
        public GameObject AttackSE { get => attackSE; set => attackSE = value; }

        [SerializeField] [Header("Hit Point")] int hp;
        [SerializeField] [Header("Speed")] float speed;
        [SerializeField] [Header("PlayerのMPの何割の攻撃を与えるか")] [Range(0f, 1f)] float powerRate;
        [SerializeField] [Header("Sound effect on Attack.")] GameObject attackSE;
    }
}