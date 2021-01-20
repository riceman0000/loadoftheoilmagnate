using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate
{
    [System.Serializable,CreateAssetMenu(fileName ="VoiceList",menuName ="VoiceList/Create")]
    public class VoiceList : ScriptableObject
    {
        public List<VoiceContent> voiceContents = new List<VoiceContent>();
        Dictionary<VoiceTag, AudioClip> voiceDic = new Dictionary<VoiceTag, AudioClip>();
        
        public Dictionary<VoiceTag, AudioClip> VoiceDic
        {
            get
            {
                if (voiceDic == null||voiceDic.Count == 0)
                {
                    foreach (var item in voiceContents)
                    {
                        voiceDic.Add(item.VoiceName, item.VoiceClip);
                    }
                }
                return voiceDic;
            }
        }
    }
    [System.Serializable]
    public class VoiceContent
    {
        public VoiceTag VoiceName;
        public AudioClip VoiceClip;
    }
    public enum VoiceTag
    {
        /// <summary>主人公、石油アクションを行う時の声</summary>
        OilAction,
        /// <summary>主人公、攻撃時の声</summary>
        Attack,
        /// <summary>主人公、ダメージを受けた時の声</summary> 
        ReceiveDamage,
        /// <summary>主人公、死亡時の声</summary>
        Death,
        /// <summary>主人公、ゴールした時の声</summary>
        Clear
    }
}