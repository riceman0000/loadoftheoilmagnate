using UnityEngine;
using System;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// <see cref="OilGeyser"/>の成分
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName ="OilGeyserParam",menuName ="Player/OilGeyserParam")]
    public class OilGeyserMaterial : ScriptableObject
    {

        [SerializeField]
        [Header("オイルの寿命")]
        float _oilLifeTime = 2f;

        /// <summarya>
        /// 石油の最大長さ
        /// </summary>
        [SerializeField]
        [Header("石油の最大長さ")]
        float _limitOilGeyserLength = 10f;

        /// <summary>
        /// 最大石油数
        /// </summary>
        [SerializeField]
        [Header("最大石油数")]
        int _maximumLineCount;

        [SerializeField]
        [Header("オイルの噴出、減衰に掛かる時間")]
        float _animationTime = .2f;

        [SerializeField]
        [Header("油田に流される強さ")]
        float _oilGeyserforce;

        [SerializeField]
        [Header("一発放つ毎に掛かる石油の消費量")]
        long _consumptionQuantity;

        [SerializeField]
        [Header("石油の強さの上限")]
        float _limitOilGeyserPower;


        public float OilLifeTime => _oilLifeTime;
        public float AnimationTime => _animationTime;
        public float Force => _oilGeyserforce;
        public float LimitOilGeyserLength => _limitOilGeyserLength;
        public float MaximumLineCount => _maximumLineCount;
        public long ConsumptionQuantity => _consumptionQuantity;

        public float LimitOilGeyserPower=> _limitOilGeyserPower; 

        public OilGeyserMaterial
            (float oilLifeTime, float animationTime, float oilGeyserforce,
            long consumptionQuantity)
        {
            _oilLifeTime = oilLifeTime;
            _animationTime = animationTime;
            _oilGeyserforce = oilGeyserforce;
            _consumptionQuantity = consumptionQuantity;
        }

        public OilGeyserMaterial(OilGeyserMaterial mat)
            : this(mat._oilGeyserforce, mat._animationTime, mat._oilGeyserforce,
                  mat.ConsumptionQuantity)
        { }
    }
}