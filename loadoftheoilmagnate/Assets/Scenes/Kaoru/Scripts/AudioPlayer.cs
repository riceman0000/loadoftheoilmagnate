using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

namespace OilMagnate
{
    /* 未実装メモ
     * イントロ追加するなら
     * AudioListにintro格納Dicsionary追加
     * introがあるシーンなら最初にイントロ再生で分岐判定
     * その後BGMループ処理
     * フラグで管理？
     * SEも
     */


    /// <summary>
    /// オーディオを再生させる。
    /// </summary>
    public class AudioPlayer : ManagedMono
    {
        /// <summary>
        /// オーディオソース。
        /// </summary>
        private AudioSource loop;
        private AudioSource intro;
        public AudioSource Loop
        {
            get
            {
                if (loop == null)
                {
                    loop = AudioManager.Instance.Bgm;
                }
                return loop;
            }
        }
        public AudioSource Intro
        {
            get
            {
                if (intro == null)
                {
                    intro = AudioManager.Instance.Intro;
                }
                return intro;
            }
        }

        [SerializeField] AudioList audioList;

        AudioManager _audioManager;
        /// <summary>
        /// シーンごとにAudioSourceを変更させる
        /// </summary>
        public void SourceChanged(SceneTitle title)
        {
            var currentTitle = title == SceneTitle.None ? SceneTitle.StageOne : title;
            var audioList = this.audioList.AudioDics[currentTitle];
            Loop.clip = audioList.loopClip;
            Loop.loop = true;
            if (audioList.introFlag)
            {
                Intro.clip = audioList.introClip;
                Intro.loop = false;
                Intro.Play();
                Loop.PlayScheduled(AudioSettings.dspTime + Intro.clip.length);
            }
            else { Loop.Play(); }
        }

        protected override void Awake()
        {
            base.Awake();
            _audioManager = AudioManager.Instance;
            _audioManager.RegistPlayer(this);

            SceneFader.OnSceneLoaded += SourceChanged;
        }
        /// <summary>
        /// 
        /// </summary>
        public void OnDisable()
        {
            //Loop.Stop();
            _audioManager?.UnregistPlayer(this);
        }

        /// <summary>
        /// 再生を行う。
        /// </summary>
        public void Play()
        {
            // 自分がBgmの場合には、他のBgmの再生を停止させます。
            var bgm = AudioManager.Instance.AudioBgmPlayers;
            if (bgm.Contains(this))
            {
                foreach (var player in bgm)
                {
                    if (player == this) { continue; }

                    if (player.Loop.isPlaying)
                    {
                        {
                            player.Stop();
                        }
                    }
                }
            }


            //BaseVolume = 1;
            //FadeDeltaTime = 0;
            Loop.volume = 0;
            Loop.Play();
            //IsFadePlaying = true;
            //IsFadeStopping = false;
        }

        /// <summary>
        /// 停止を行う。
        /// </summary>
        public void Stop()
        {
            Loop.Stop();
        }

        /// <summary>
        /// 停止を行う。
        /// <param name="fadeSec">フェードアウト完了までの秒数。</param>
        /// </summary>
        //public void StopFadeOut(double fadeSec)
        //{
        //    BaseVolume = Source.volume;
        //    FadeDeltaTime = 0;
        //    FadeOutSeconds = fadeSec;
        //    IsFadeStopping = true;
        //    IsFadePlaying = false;
        //}

        /// <summary>
        /// 一時停止する
        /// </summary>
        public void Pause()
        {
            Loop.Pause();
        }
    }
}
