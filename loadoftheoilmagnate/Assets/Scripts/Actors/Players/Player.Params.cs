using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using OilMagnate.StageScene;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;

namespace OilMagnate.Player
{
    /// <summary>
    /// Parameters.
    /// </summary>
    public partial class Player
    {
        #region Components
        /// <summary>
        /// Rigid body 2d
        /// </summary>
        [SerializeField]
        Rigidbody2D _rb2d;

        /// <summary>
        /// Animator
        /// </summary>
        [SerializeField]
        Animator _animator;

        /// <summary>
        /// inputProvider
        /// </summary>
        [SerializeField]
        Joystick _inputProvider;

        /// <summary>
        /// Oil geyser generator.
        /// </summary>
        [SerializeField]
        OilGenerator _oilGenerator;

        /// <summary>
        /// 服の着せ替えを行うコンポーネント
        /// </summary>
        [SerializeField]
        PlayerCoordinator _clothesManager;

        /// <summary>
        /// セーブされるプレイヤーのデータ
        /// </summary>
        [SerializeField]
        PlayerData _playerData;


        /// <summary>
        /// セーブデータがなかった場合、このステータスを参照する
        /// </summary>
        [SerializeField]
        [Header("セーブデータがなかった場合、このステータスを参照する")]
        Status _initialStatus = null;




        /// <summary>
        /// stage facade
        /// </summary>
        StageManager _stage;

        /// <summary>
        /// スマホ操作のジェスチャー検知コンポーネント
        /// </summary>
        TouchGestureDetector _touchGestureDetector;
        #endregion

        #region Parameters
  
        /// <summary>
        /// currnet state of player.
        /// </summary>
        /// <value></value>
        public ReactiveProperty<PhysicalState> CurrentState { get; set; }
            = new ReactiveProperty<PhysicalState>(PhysicalState.None);

        #endregion
        public PlayerMPManager MPManager{ get; set; }


        public PlayerData MyData => _playerData;

        public Animator Animator => _animator;

    }
}
