using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;


namespace OilMagnate
{
    /// <summary>
    /// オーディオ情報を管理。
    /// </summary>
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] AudioSource bgmPrefab;
        [SerializeField] AudioSource sePrefab;
        [SerializeField] AudioSource introPrehub;
        [SerializeField] AudioSource voicePrefub;
        public AudioSource Bgm { get; set; }
        public AudioSource SE { get; set; }
        public AudioSource Voice { get; set; }

        public AudioSource Intro { get; set; }

        /// <summary>
        /// 使用するオーディオミキサー。
        /// </summary>
        public AudioMixer Mixer;

        /// <summary>
        /// 使用するオーディオグループ。
        /// </summary>
        public AudioMixerGroup MixerBgmGroup;
        public AudioMixerGroup MixerSeGroup;
        public AudioMixerGroup MixerVoiceGroup;

        /// <summary>
        /// 管理しているオーディオ情報
        /// </summary>
        public List<AudioPlayer> AudioPlayers { get; private set; }

        /// <summary>
        /// 管理しているBgm情報を取得。
        /// </summary>
        public List<AudioPlayer> AudioBgmPlayers
        {
            get { return AudioPlayers.FindAll(ap => ap.Loop.outputAudioMixerGroup == MixerBgmGroup); }
        }

        /// <summary>
        /// 管理しているSe情報を取得。
        /// </summary>
        public List<AudioPlayer> AudioSePlayers
        {
            get { return AudioPlayers.FindAll(ap => ap.Loop.outputAudioMixerGroup == MixerSeGroup); }
        }

        /// <summary>
        /// 管理しているVoice情報を取得。
        /// </summary>
        public List<AudioPlayer> AudioVoicePlayers
        {
            get { return AudioPlayers.FindAll(ap => ap.Loop.outputAudioMixerGroup == MixerVoiceGroup); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioManager()
        {
            AudioPlayers = new List<AudioPlayer>();
        }


        public override void OnInitialize()
        {
            DontDestroyOnLoad(this.gameObject);
            if (!SE) SE = Instantiate(sePrefab, transform);
            if (!Bgm) Bgm = Instantiate(bgmPrefab, transform);
            if (!Intro) Intro = Instantiate(introPrehub, transform);
            if (!Voice) Voice = Instantiate(voicePrefub, transform);
        }
        /// <summary>
        /// AudioPlayerの登録。
        /// </summary>
        /// <param name="player"></param>
        public void RegistPlayer(AudioPlayer player)
        {
            AudioPlayers.Add(player);
        }

        /// <summary>
        /// プレイヤーの登録解除。
        /// </summary>
        /// <param name="player"></param>
        public void UnregistPlayer(AudioPlayer player)
        {
            AudioPlayers.Remove(player);
        }

    }
}