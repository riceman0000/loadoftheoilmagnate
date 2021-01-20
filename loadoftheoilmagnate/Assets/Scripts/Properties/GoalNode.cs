using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// 触れるとGoalする。
    /// </summary>
    public class GoalNode : MonoBehaviour
    {
        StageManager _stage;

        private void Awake()
        {
            _stage = StageManager.Instance;

            this.OnTriggerEnter2DAsObservable()
                .Where(col=>col.tag.Equals(ObjTag.Player.ToString()))
                .Take(1)
                .Subscribe(_ =>
                {
                    _stage.Clear();
                })
                .AddTo(this);
        }
    }
}