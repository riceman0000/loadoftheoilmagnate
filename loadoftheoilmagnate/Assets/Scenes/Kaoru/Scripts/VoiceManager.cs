using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate
{

    public class VoiceManager : MonoSingleton<VoiceManager>
    {
        [SerializeField] VoiceList voiceList;
        [SerializeField] AudioSource voiceSource;

        public AudioSource VoiceSource
        {
            get
            {
                if (voiceSource == null)
                {
                    voiceSource = AudioManager.Instance.Voice;
                }
                return voiceSource;
            }
        }
        /// <summary>
        /// Voice管理
        /// 再生させたいVoiceTagを引数にとり、対応したVoiceを再生する。
        /// </summary>
        /// <param name="s">再生させたいVoiceのタグ</param>
        public void VoicePlay(VoiceTag s)
        {
            var clip = voiceList.VoiceDic[s];
            voiceSource.PlayOneShot(clip);
        }
    }
}