using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate
{
    [System.Serializable, CreateAssetMenu(fileName = "SEList", menuName = "SEList/Create")]

    public class SEList : ScriptableObject
    {
        public List<SEContent> sEContents = new List<SEContent>();
        Dictionary<SETag, AudioClip> seDics = new Dictionary<SETag, AudioClip>();

        public Dictionary<SETag, AudioClip> SeDics
        {
            get
            {
                if (seDics == null|| seDics.Count == 0)
                {
                    foreach (var item in sEContents)
                    {
                        seDics.Add(item.SEName, item.SEClip);
                    }
                }
                return seDics;
            }
            
        }
    }
    [System.Serializable]
    public class SEContent
    {
        public SETag SEName;
        public AudioClip SEClip;
    }
   
    public enum SETag
    {
        /// <summary>選択音</summary>
        Select,
        /// <summary>ステージ１（船上ステージ）の足音</summary>
        FootstepsAtShip,        
        /// <summary>ステージ２（砂漠ステージ）の足音</summary>
        FootstepsAtDesert,
        /// <summary>敵の発砲音</summary>
        Shoot,
        /// <summary>リザルト画面で鳴る音</summary>
        Result,
        /// <summary>選択音</summary>
        Satutaba_Beat,
        /// <summary>ドラム缶が転がる音</summary>
        Drum,
        /// <summary>木箱が爆発する音</summary>
        Woodenbox,
        /// <summary>敵のミサイルが自機かオブジェクト、地面と接触して爆発する音</summary>
        Explosion,
        /// <summary>石油を出した時の音</summary>
        Oil,
        /// <summary>自機がダメージを負った時の音</summary>
        ReceiveDamage,
        /// <summary>自機が敵かオブジェクトに接触した時の音</summary>
        Strike,
        /// <summary>戦闘機の登場時の効果音</summary>
        Plane,
        /// <summary>札束が舞い散る音</summary>
        BillBundle
    };
}