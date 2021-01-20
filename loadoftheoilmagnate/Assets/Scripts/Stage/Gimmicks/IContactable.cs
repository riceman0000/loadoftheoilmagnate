using UnityEngine;

namespace OilMagnate.StageScene
{
    public interface IContactable
    {

        /// <summary>
        /// 耐久値があるかどうか。自身が破壊可能かどうか。吹き飛ばし可能かどうか。
        /// </summary>
        bool IsBreakable { get; }

        /// <summary>
        /// <paramref name="force"/>の力積を与えて自身を吹き飛ばす.
        /// </summary>
        /// <param name="force">力積.</param>
        /// <param name="blowMode "><see cref="GimmickParam.BlowForce"/>を使用するかどうか.</param>
        /// /// <param name="forceMode">力の掛け方. 外力か,力積か.</param>
        void BlowOff(Vector2 force,BlowMode blowMode = BlowMode.FromParam, ForceMode2D forceMode = ForceMode2D.Impulse);
    }
}