using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// ザコ敵 : 人型
    /// </summary>
    public class Human :Enemy<Human,Human.State>
    {
        [SerializeField] [Header("攻撃方法")] HumanAttackMethod attackMethod;
        /// <summary>
        /// Awakeの最後に呼ばれるコールバック
        /// base.Initialize();は必須。
        /// </summary>
        protected override void OnAwake()
        {
            stateList.Add(new HumanAttackState(this, State.Attack));
            stateList.Add(new HumanPursuitState(this, State.Pursuit));
            stateList.Add(new HumanWanderState(this, State.Search));
            // TODO: Add another states.
        }

        private void Start()
        {
            ChangeState(State.Search);
        }

        /// <summary>
        /// 攻撃処理
        /// </summary>
        public override void Attack()
        {
            // TODO: 攻撃の演出や処理を実装。
        }

        /// <summary>
        /// 自分自身の破壊処理
        /// </summary>
        public override void Destruction()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// ダメージ処理
        /// </summary>
        /// <param name="amount"></param>
        public override void TakeDamage(int amount)
        {
            Status.Hp -= amount;
            if(Status.Hp <= 0)
            {
                Destruction();
            }
        }

        public override void TakeDamage(float amount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 人型エネミーの攻撃方法
        /// </summary>
        public enum HumanAttackMethod
        {
            Gun,
            Knife,
        }

        /// <summary>
        /// ステート
        /// </summary>
        public enum State
        {
            Search,
            Pursuit,
            Attack,
            Escape,
            ToDie,
            Idle,
        }
    }
}