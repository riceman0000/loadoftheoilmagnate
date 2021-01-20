using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace OilMagnate.StageScene
{
    [Serializable]
    //[CreateAssetMenu(fileName = "MPParam", menuName = "Player/MP")]
    public class MPParameter /*: ScriptableObject*/
    {
        [SerializeField]
        [Header("MPの最大数")]
        long _max;


        //[SerializeField]
        [Header("レイヤーの境界線を何処に設定するか設定する値")]
        float _layerBorderRatio = 1f;

        //[SerializeField]
        //[Header("上限成長限界")]
        //[Range(1f, 2f)]
        //float _growthLimit;

        [HideInInspector]
        long _current;
        private MPParameter param;

        /// <summary>
        /// MPの最大数
        /// </summary>
        public long Max { get => _max; set => _max = value; }
        /// <summary>
        /// 現在のMP数
        /// </summary>
        public long Current { get => _current; set => _current = value; }

        /// <summary>
        /// レイヤーの境界線を何処に設定するか設定する値
        /// </summary>
        public float LayerBorderRatio { get => _layerBorderRatio; set => _layerBorderRatio = value; }

        ///// <summary>
        ///// 上限成長限界
        ///// </summary>
        //public float GrowthLimit { get => _growthLimit; set => _growthLimit = value; }

        /// <summary>
        /// 最大値からみた現在の値の割合
        /// </summary>
        public float Ratio => (float)_current / _max;

        public float Layer1Ratio => _current / _layerBorderRatio;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="max">mpの最大値</param>
        public MPParameter(long max)
        {
            Max = max;
            Current = max;
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="max">mpの最大値</param>
        /// <param name="current"></param>
        public MPParameter(long max,long current)
        {
            Max = max;
            Current = current;
        }

        public MPParameter(MPParameter param)
        {
            Max = param.Max;
            Current = param.Current;
        }
    }
}