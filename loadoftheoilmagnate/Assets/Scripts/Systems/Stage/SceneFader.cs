using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace OilMagnate
{
    /// <summary>
    /// Scene fader.
    /// </summary>
    public class SceneFader : MonoSingleton<SceneFader>
    {
        /// <summary>フェード時に使用するCanvas</summary>
        static private Canvas m_fadeCanvas;
        /// <summary>フェーディング演出に使うImage</summary>
        private Image m_fadeImage;
        private Color fadingColor = Color.black;

        /// <summary>m_fadeImadeのアルファ値</summary>
        private float m_alpha;
        /// <summary>フェーディング演出に掛ける時間</summary>
        private float m_fadeTime = .5f;
        /// <summary>遷移先のシーンタイトル</summary>
        private string m_nextSceneTitle;
        private SceneTitle _sceneTitle;

        public static System.Action<SceneTitle> OnSceneLoaded;

        /// <summary>
        /// CurrentScene
        /// </summary>
        public SceneTitle CurrentScene { get => _sceneTitle; private set => _sceneTitle = value; }


        private void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                SceneFader.Instance.FadeIn();
                //Debug.Log($"currentScene = {Instance.CurrentScene}");
            };
            if(!Enum.TryParse(SceneManager.GetActiveScene().name, out _sceneTitle))
            {
                //throw new ArgumentException("CurrentSceneがstringから取得出来ませんでした。");
                _sceneTitle = SceneTitle.StageOne;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        void Initialize()
        {
            // fade用のCanvas生成  ※(gameObjectとSceneFader.cs自体はMonoSingletonのInstanceプロパティ呼び出し時に生成,アタッチしている)
            m_fadeCanvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();

            m_fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // 最前面になるようLayer設定
            m_fadeCanvas.sortingLayerName = "UI";
            m_fadeCanvas.sortingOrder = 1000; // 適当な値(最前面,つまりこの値がSceneにあるどのUIレイヤーのsortingOrderよりも高ければなんでもよい)

            // fade用のImage生成
            m_fadeImage = new GameObject("ImageFade").AddComponent<Image>();
            m_fadeImage.color = Color.white;
            m_fadeImage.transform.SetParent(m_fadeCanvas.transform, false);
            m_fadeImage.rectTransform.anchoredPosition = Vector2.zero;

            // 画面のサイズに合わせてImageのサイズ設定
            m_fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            //Debug.Log(m_fadeImage.rectTransform.sizeDelta);
        }

        internal void FadeOut(object nextScene)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// BGMTest用
        ///// (SceneFader.csで実際にシーン管理するようになれば不要)
        ///// </summary>
        ///// <returns></returns>
        //SceneTitle Hoge()
        //{
        //    SceneTitle title = CurrentScene;
        //    string sceneName = SceneManager.GetActiveScene().name;
        //    if (title.ToString() != sceneName)
        //    {
        //        if (System.Enum.TryParse<SceneTitle>(sceneName, out var tmp))
        //        {
        //            title = tmp;
        //        }
        //    }
        //    return title;
        //}

        /// <summary>
        /// フェードイン
        /// </summary>
        public void FadeIn(float fadeTime = 1f)
        {
            //CurrentScene = Hoge();
            OnSceneLoaded?.Invoke(CurrentScene);
            m_fadeTime = fadeTime;
            SceneFader.Instance.StartCoroutine(FadingIn());
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="sceneTitle">遷移先のシーンタイトル</param>
        /// <param name="fadeTime">フェーディング処理に掛ける時間</param>
        public void FadeOut(SceneTitle sceneTitle, Color fadeColor, float fadeTime = 1f)
        {
            m_fadeTime = fadeTime;
            fadingColor = fadeColor;
            CurrentScene = sceneTitle;
            m_nextSceneTitle = sceneTitle.ToString();
            StartCoroutine(FadingOut());
        }

        public void FadeOut(SceneTitle sceneTitle, float fadeTime = 1f) => FadeOut(sceneTitle, Color.black, fadeTime);

        /// <summary>
        /// フェードインのコルーチン
        /// </summary>
        IEnumerator FadingIn()
        {
            if (m_fadeImage == null)
            {
                Initialize();
            }
            m_fadeImage.color = Color.white;
            m_alpha = 1f;
            while (m_alpha > 0f)
            {
                m_alpha -= Time.unscaledDeltaTime * m_fadeTime;
                m_fadeImage.color = new Color(fadingColor.r, fadingColor.g, fadingColor.b, m_alpha);
                if (m_alpha < Mathf.Epsilon)
                {
                    break;
                }
                yield return null;
            }
            m_fadeCanvas.enabled = false;
        }

        /// <summary>
        /// フェードアウトのコルーチン
        /// </summary>
        IEnumerator FadingOut()
        {
            if (m_fadeImage == null)
            {
                Initialize();
            }
            m_fadeCanvas.enabled = true;
            m_alpha = 0f;

            while (m_alpha < 1f)
            {
                m_alpha += Time.unscaledDeltaTime * m_fadeTime;
                m_fadeImage.color = new Color(fadingColor.r, fadingColor.g, fadingColor.b, m_alpha);
                yield return null;
            }
            SceneManager.LoadScene(m_nextSceneTitle);
        }
    }
}