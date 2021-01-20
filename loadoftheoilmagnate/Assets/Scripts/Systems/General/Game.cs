using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using OilMagnate.Player;

namespace OilMagnate
{
    /// <summary>
    /// The Game class.
    /// Facade pattern.
    /// </summary>
    public class Game : MonoSingleton<Game>
    {
        public SceneTitle GetCurrentScene
        {
            get => CurrentScene;
        }

        /// <summary>
        /// touchとgestureを管理する
        /// </summary>
        TouchGestureDetector _touchGestureDetector;
        SceneFader sceneFader;

        public PlayerData SaveData { get; private set; }
        /// <summary>
        /// currentScene
        /// </summary>
        public SceneTitle CurrentScene => sceneFader.CurrentScene;


        private void Awake()
        {
            var gm = Game.Instance;
            sceneFader = SceneFader.Instance;
            SaveData = PlayerData.Instance;
        }

        private void Start()
        {
            SaveData = PlayerData.Instance;
            SaveData.Save();
        }

        /// <summary>
        /// タッチジェスチャーのイベントを登録する
        /// </summary>
        /// <param name="touchEvent"></param>
        public void RegisterTouchEvent(Action<TouchGestureDetector.Gesture, TouchGestureDetector.TouchInfo> touchEvent)
        {
            if (_touchGestureDetector == null) return;
            _touchGestureDetector.OnDetectGesture += touchEvent;
        }


        /// <summary>
        /// 指定IDとステージが一緒かどうか  
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public bool IsSameStage(SceneTitle scene) => CurrentScene == scene;

        /// <summary>
        /// 
        /// </summary>
        public override void OnInitialize() => DontDestroyOnLoad(gameObject);
    }
}