using System;
using UnityEngine;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// Gimmicks parameter.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "GimmickParameter", menuName = "Gimmick/Parameter")]
    public class GimmickParam : ScriptableObject
    {
        [SerializeField]
        [Header("接触した対象の速度減衰率")]
        [Header("対象と接触した直後耐久値が0以下の時")]
        float _attenuationRate;

        [SerializeField]
        [Header("接触した対象を弾く反発係数")]
        [Header("対象と接触した直後耐久値が1以上の時")]
        float _coefficientOfRestitution;

        [SerializeField]
        [Header("耐久値")]
        float _durability = 1;

        [SerializeField]
        [Header("自分自身が吹き飛ばされる時の力積")]
        float _blowForce;

        [SerializeField]
        [Header("吹き飛ばす方向に調整を掛けるベクトル")]
        Vector2 _blowDirection;

        [SerializeField]
        [Header("フィールドとして扱うかどうか")]
        bool _isField = false;

        [SerializeField]
        [Header("資産額")]
        long _assetAmount;


        const float _deadLine = 300f;

        /// <summary>
        /// 当たった時の減衰率
        /// </summary>
        public float AttenuationRate { get => _attenuationRate; set => _attenuationRate = value; }

        /// <summary>
        /// 反発係数
        /// </summary>
        public float CoefficientOfRestitution { get => _coefficientOfRestitution; set => _coefficientOfRestitution = value; }

        /// <summary>
        /// そのギミックの耐久値
        /// </summary>
        public float Durability { get => _durability; set => _durability = value; }

        /// <summary>
        /// この数値以上初期位置から離れたら破棄される。
        /// </summary>
        public float DeadBorderLine => _deadLine;

        /// <summary>
        /// 吹き飛ばす時のスカラー
        /// </summary>
        public Vector2 BlowForce => _blowForce * _blowDirection;

        /// <summary>
        /// 耐久値がある時接触時の物理演算に制限を掛けるか
        /// </summary>
        public bool IsField => _isField;

        /// <summary>
        /// 資産額
        /// </summary>
        public long AssetAmount => _assetAmount;
    }
}