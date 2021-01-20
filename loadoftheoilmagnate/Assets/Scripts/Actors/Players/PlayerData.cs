using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using OilMagnate.StageScene;

namespace OilMagnate.Player
{
    /// <summary>
    /// Player data
    /// </summary>
    [Serializable]
    public class PlayerData : SavableSingleton<PlayerData>
    {

        [SerializeField]
        SceneTitle _progress = SceneTitle.None;

        [HideInInspector]
        Status _status = null;

        /// <summary>
        /// Current progress.
        /// </summary>
        /// <value></value>
        public SceneTitle Progress { get => _progress; }

        /// <summary>
        /// Player's status.
        /// </summary>
        public Status Status { get => _status; }

        public void SetStatus(Status status) => _status = status;   

        /// <summary>
        /// 進捗フラグを確認する。
        /// </summary>
        /// <param name="flag"></param>
        public void UpdateProgress(SceneTitle flag)
        {
            _progress |= flag;
            //Debug.Log($"progress is {_progress}");
        }
        public bool HasFlag(SceneTitle flag) => _progress.HasFlag(flag);

    }
}
