using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// <see cref="Human"/>の追跡ステート。
    /// </summary>
    public class HumanPursuitState : State<Human, Human.State>
    {
        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="identity"></param>
        public HumanPursuitState(Human owner, Human.State identity) : base(owner, identity) { }

        /// <summary>
        /// <see cref="State{TOwner, TEnum}.Enter"/>だけはbaseを呼び出す。
        /// このステートが呼ばれた最初のフレームにコールバックする
        /// </summary>
        public override void Enter()
        {
            base.Enter();

        }

        /// <summary>
        /// このステート中毎フレームコールバック
        /// </summary>
        public override void Execute()
        {
            // TODO:プレイヤーの位置まで近づく,y座標が同じになったら攻撃ステートに遷移
            //崖のトリガーに触れていたら足を止めて反対方向に200動く
            //オブジェクトのコリジョンに触れていたら足を止めて反対方向に200動く
            //owner.ChangeState(Human.State.Attack);
            
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