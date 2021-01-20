using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;

namespace OilMagnate.StageScene
{
    [Serializable]
    public class Mission
    {
        //[SerializeField]
        //[Header("ミッションの内容")]
        //string _missionName;

        [SerializeField]
        GimmickIdentifier _id = GimmickIdentifier.None;

        [SerializeField]
        int _quantity = 0;

        int _progress = 0;

        int _missionAmount = 0;

        /// <summary>
        /// 量
        /// </summary>
        public int TargetQuantity => _quantity;

        /// <summary>
        /// 残りのターゲット数
        /// </summary>
        public int RemainTargets => _quantity - _progress;

        /// <summary>
        /// Identifier
        /// </summary>
        public GimmickIdentifier ID => _id;

        /// <summary>
        /// whether you completed the mission.
        /// </summary>
        public bool IsCompleteMission => TargetQuantity <= _progress;

        /// <summary>
        /// Informations of current progress.
        /// </summary>
        public string MissionProgress => $"{_progress} / {_quantity}";

        /// <summary>
        /// ミッションターゲットと同じIDかどうかチェックする。
        /// </summary>
        /// <param name="id"></param>
        public void CheckID(GimmickIdentifier id)
        {
            if (id == _id)
            {
                UpdateProgress();
            }
        }

        /// <summary>
        /// 進捗の更新
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateProgress(int amount = 1)
        {
            if (_progress < TargetQuantity)
            {
                _progress += amount;
                Debug.Log($"{ID}壊したよ");
            }
        }

        public string Id2String(GimmickIdentifier id)
        {
            switch (id)
            {
                case GimmickIdentifier.None:
                    break;
                case GimmickIdentifier.Drum:
                    break;
                case GimmickIdentifier.WoodenBox:
                    break;
                case GimmickIdentifier.SideContainer:
                    break;
                case GimmickIdentifier.FrontContainer:
                    break;
                case GimmickIdentifier.Crane:
                    break;
                case GimmickIdentifier.Pipeline1:
                    break;
                case GimmickIdentifier.Pipeline2:
                    break;
                case GimmickIdentifier.Pier:
                    break;
                case GimmickIdentifier.Humvee:
                    break;
                case GimmickIdentifier.ZakoGun:
                    break;
                case GimmickIdentifier.ZakoKnife:
                    break;
                case GimmickIdentifier.Fighter:
                    break;
                case GimmickIdentifier.Aburauru:
                    break;
                default:
                    break;
            }
            return "idを名前に変換して返す予定。一時停止とかprogress用。";
        }
    }
}
