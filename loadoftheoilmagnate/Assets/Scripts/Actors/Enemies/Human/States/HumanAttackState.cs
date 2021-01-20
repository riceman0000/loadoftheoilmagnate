using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// <see cref="Human"/>の攻撃ステート。
    /// </summary>
    public class HumanAttackState : State<Human, Human.State>
    {
        public HumanAttackState(Human owner, Human.State identity) : base(owner, identity) { }

        /// <summary>
        /// <see cref="State{TOwner, TEnum}.Enter"/>だけはbaseを呼び出す。
        /// このステートが呼ばれた最初のフレームにコールバックする
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            //ここでイベント登録。破棄出来るよう別メソッドにしておく。
            owner.OnTriExit2D += Owner_OnTriExit2D;
        }

        private void Owner_OnTriExit2D(Collider2D col)
        {
            // 何かしらの処理。
        }

        /// <summary>
        /// このステート中毎フレームコールバック
        /// </summary>
        public override void Execute()
        {
            // TODO:
        }

        /// <summary>
        /// このステートが呼ばれる最後のフレームにコールバックする
        /// </summary>
        public override void Exit()
        {
            // ここでイベント破棄。
            owner.OnTriExit2D -= Owner_OnTriExit2D;
        }
    }
}