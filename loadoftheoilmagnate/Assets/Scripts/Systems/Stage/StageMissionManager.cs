using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;

namespace OilMagnate.StageScene
{
    public class StageMissionManager : ManagedMono
    {
        /// <summary>
        /// First mission
        /// </summary>
        [SerializeField]
        Mission _firstMission = null;

        /// <summary>
        /// Second mission.
        /// </summary>
        [SerializeField]
        Mission _secondMission = null;

        /// <summary>
        /// Third mission.
        /// </summary>
        [SerializeField]
        Mission _thirdMission = null;

        /// <summary>
        /// Prefab of <see cref="TargetIndicator"/>.
        /// </summary>
        [SerializeField]
        GameObject _indicatorPrefab;

        /// <summary>
        /// Gimmick Observer.
        /// </summary>
        GimmickObserver _gimmickObserver;

        /// <summary>
        /// First mission
        /// </summary>
        public Mission FirstMission => _firstMission;

        /// <summary>
        /// Second mission
        /// </summary>
        public Mission SecondMission => _secondMission;

        /// <summary>
        /// Third mission
        /// </summary>
        public Mission ThirdMission => _thirdMission;

        /// <summary>
        /// Disposable of Observer
        /// </summary>
        SingleAssignmentDisposable _disposable = new SingleAssignmentDisposable();

        /// <summary>
        /// ファースト・ミッションクリアしているか
        /// </summary>
        public bool IsFirstMissionComplete => _firstMission.IsCompleteMission;
        /// <summary>
        /// セカンド・ミッションクリアしているか
        /// </summary>
        public bool IsSecondMissionComplete => _secondMission.IsCompleteMission;
        /// <summary>
        /// サード・ミッションクリアしているか
        /// </summary>
        public bool IsThirdMissionComplete => _thirdMission.IsCompleteMission;

        /// <summary>
        /// 全てのミッションクリアしているかどうか。
        /// </summary>
        public bool IsAllComplete => _firstMission.IsCompleteMission
               && _secondMission.IsCompleteMission
               && _thirdMission.IsCompleteMission;

        private void Start()
        {
            _gimmickObserver = StageManager.Instance.GimmickObserver;
            //GenerateIndicators(); 
        }

        List<TargetIndicator> _targetIndicators = new List<TargetIndicator>();
        List<IGimmick> GenerateIndicators()
        {
            var player = StageManager.Instance.Player;
            var gimmicks = _gimmickObserver.Gimmicks.FindAll(
            g => g.ID == FirstMission.ID || g.ID == SecondMission.ID || g.ID == ThirdMission.ID);

            var orderedTargets = gimmicks.OrderBy(g => (g.Transform.position - player.transform.position).sqrMagnitude);

            foreach (var target in orderedTargets)
            {
                // 生成時画面に入らない様に生成
                var indicator = Instantiate(_indicatorPrefab, new Vector3(-100f, -100f, 0f), Quaternion.identity)
                    .GetComponent<TargetIndicator>();
                _targetIndicators.Add(indicator);
                indicator.Initialize(player.transform, target);
            }
            return orderedTargets.ToList();
        }


        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        public void CheckMission(IGimmick gimmick)
        {
            _firstMission?.CheckID(gimmick.ID);
            _secondMission?.CheckID(gimmick.ID);
            _thirdMission?.CheckID(gimmick.ID);
        }
    }

    public interface IMission
    {
        bool IsCompleteMission { get; }
    }
}