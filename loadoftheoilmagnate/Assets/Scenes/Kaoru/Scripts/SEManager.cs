using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate
{
    public class SEManager : MonoSingleton<SEManager>
    {
        [SerializeField] SEList seList;
        [SerializeField] AudioSource SE_Source;

        public AudioSource seSource
        {
            get
            {
                if (SE_Source == null)
                {
                    SE_Source = AudioManager.Instance.SE;
                }
                return SE_Source;
            }
        }
        /// <summary>
        /// SE管理
        /// 対応したSEClipを再生する
        /// </summary>
        /// <param name="s">再生させたいSEのタグ</param>
        public void SEPlay(SETag s)
        {
            var clip = seList.SeDics[s];
            seSource.PlayOneShot(clip);
        }
    }
}