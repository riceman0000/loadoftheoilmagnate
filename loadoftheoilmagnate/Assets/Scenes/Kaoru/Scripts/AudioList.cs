using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OilMagnate
{

    /// <summary>
    /// BGMを格納する。
    /// </summary>
    [System.Serializable, CreateAssetMenu(fileName = "AudioList", menuName = "AudioList/Create")]
    public class AudioList : ScriptableObject
    {
        public List<AudioContent> audioContents = new List<AudioContent>();

        /// <summary> BGMのデータリスト</summary>   
        Dictionary<SceneTitle, Clips> audioDics = new Dictionary<SceneTitle, Clips>();

        public Dictionary<SceneTitle, Clips> AudioDics
        {
            get
            {
                if (audioDics == null || audioDics.Count == 0)
                {
                    foreach (var item in audioContents)
                    {
                        audioDics.Add(item.sceneTitle, item.clipDatas);
                    }
                }
                return audioDics;
            }
        }

        [System.Serializable]
        public class AudioContent
        {
            public SceneTitle sceneTitle;
            public Clips clipDatas = new Clips();
        }
        [System.Serializable]
        public class Clips
        {
            public AudioClip loopClip;
            public bool introFlag;
            public AudioClip introClip;
        }
    }
}