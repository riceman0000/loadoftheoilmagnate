using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Anima2D;

/// <summary>
/// 服を着せ替えするコンポーネント
/// </summary>
namespace OilMagnate.Player
{

    public class PlayerCoordinator : MonoBehaviour
    {
        [SerializeField] List<SpriteMeshAnimation> switchableClothes = new List<SpriteMeshAnimation>();
        [SerializeField] List<SpriteMeshInstance> detachableClothes = new List<SpriteMeshInstance>();

        public void SetClothes(ClothesLevel level)
        {
            foreach (var clothe in switchableClothes)
            {

            }
        }

        public void SetVlothes(bool isNaked)
        {
            foreach (var clothe in switchableClothes)
            {
                clothe.frame = isNaked ? 0 : 1;
            }
            foreach (var clothe in detachableClothes)
            {
                clothe.enabled = !isNaked;
            }
        }


        /// <summary>
        /// 着衣のレベル
        /// </summary>
        public enum ClothesLevel
        {
            /// <summary>全部着ている</summary>
            Full,
            /// <summary>半分着ている</summary>
            Half,
            /// <summary>全裸</summary>
            Naked,
        }
    }
}