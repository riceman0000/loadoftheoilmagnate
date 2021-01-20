using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// <see cref="Human"/>の彷徨いステート。
    /// </summary>
    public class HumanWanderState : State<Human, Human.State>
    {
        public HumanWanderState(Human owner, Human.State identity) : base(owner, identity) { }

        CheckAndCast checkT = new CheckAndCast();

        /// <summary>
        /// <see cref="State{TOwner, TEnum}.Enter"/>だけはbaseを呼び出す。
        /// このステートが呼ばれた最初のフレームにコールバックする
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            owner.OnTriEnter2D += Owner_OnTriEnter2D;
        }

        private void Owner_OnTriEnter2D(Collider2D col)
        {
            // 何かしらの処理。
            if (col.gameObject.tag == "Player")
            {
                owner.ChangeState(Human.State.Pursuit);
            }          
        }

        /// <summary>
        /// このステート中毎フレームコールバック
        /// </summary>
        public override void Execute()
        {
            
            // TODO:動作なし、プレイヤー近づいたら追う->Pursuitに
            //if (checkT.Get_dLength(owner.Player.position, owner.transform.position) < 100)
            //{
            //    owner.ChangeState(Human.State.Pursuit);
            //}
        }
        

        /// <summary>
        /// このステートが呼ばれる最後のフレームにコールバックする
        /// </summary>
        public override void Exit()
        {
            // TODO:
        }
    }
}