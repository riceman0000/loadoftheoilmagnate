using UnityEngine;
using UnityEngine.UI;

namespace OilMagnate.StageSelect
{
    /// <summary>
    /// ステージボタン
    /// </summary>
    public class StageButton : ManagedMono
    {
        /// <summary>
        /// 使用するボタンコンポーネント
        /// </summary>
        [SerializeField]
        [Header(" 使用するボタンコンポーネント")]
        Button _button;

        /// <summary>
        /// 使うImageコンポーネント。
        /// 各種ステージの画像を入れておく。
        /// </summary>
        [SerializeField]
        [Header("使うImageコンポーネント。")]
        Image _mainImage;

        [SerializeField]
        [Header("フラグが建っている時のテクスチャ")]
        Sprite _enableTex;
        [SerializeField]
        [Header("フラグが建っていない時のテクスチャ")]
        Sprite _disableTex;

        /// <summary>
        /// フレーム用コンポーネント。
        /// サイズはnativeにしておいて、場所をあわせて上に重ねておく。
        /// </summary>
        [SerializeField]
        [Header("フレーム用コンポーネント。")]
        Image _frameImage;

        [SerializeField]
        [Header("遷移先のシーン")]
        SceneTitle _nextScene;

        [SerializeField]
        [Header("セーブデータがこのフラグをクリアしているかで判断する")]
        SceneTitle _enabledCondition;
        /// <summary>
        /// <see cref="CurrentState"/>のバッキングフィールド
        /// </summary>
        ButtonState _currentState = ButtonState.None;

        Game _game;

        public Button Button => _button;
        public Image MainImage => _mainImage;
        public Image FrameImage => _frameImage;
        /// <summary>
        /// ボタンの状態。
        /// </summary>
        public ButtonState CurrentState => _currentState;

        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
            _game = Game.Instance;
            _mainImage.sprite = _enableTex;
        }

        private void Start()
        {
            _frameImage.enabled = false;
            if (!_game.SaveData.HasFlag(_enabledCondition)) // TODO: セーブデータ実装したらtrue消す
            {
                _mainImage.sprite = _disableTex;
                _button.interactable = false;
            }
        }

        /// <summary>
        /// `現在のステートが指定ステートと同値かどうか。
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsState(ButtonState state) => _currentState.HasFlag(state);

        public void ChangeState(ButtonState nextState)
        {
            _currentState = nextState;

            OnChangeState();
        }

        /// <summary>
        /// <see cref="CurrentState"/>が更新された際のコールバック
        /// </summary>
        void OnChangeState()
        {
            switch (CurrentState)
            {
                case ButtonState.None:
                    _frameImage.enabled = false;
                    break;
                case ButtonState.Selected:
                    _frameImage.enabled = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 登録されたシーンへ遷移する。
        /// </summary>
        public void TransitionScene()
        {
            SceneFader.Instance.FadeOut(_nextScene);
        }
    }
}