using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OilMagnate.StageScene
{
    public interface IGimmick : IDestructible, IReflective, IContactable
    {
        /// <summary>
        /// 自分自身のトランスフォーム
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// そのギミックの識別子
        /// </summary>
        GimmickIdentifier ID { get; }

        /// <summary>
        /// ギミックのパラメーター
        /// </summary>
        GimmickParam Param { get; }

        /// <summary>
        /// 耐久値を削る。削りきったら<see cref="true"/>を返す。
        /// </summary>
        /// <param name="opponent">接触相手.</param>
        /// <param name="force"><paramref name="opponent"/>の速度.</param>
        /// <returns>耐久値を削りきったかどうか。</returns>
        void ReduceDurability(Rigidbody2D opponent, Vector3 force);
        void ReduceDurability(Rigidbody2D opponent, float attack);

    }
}
