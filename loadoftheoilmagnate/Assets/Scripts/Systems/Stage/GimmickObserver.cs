using System.Collections.Generic;

namespace OilMagnate.StageScene
{
    /// <summary>
    /// 今の所未使用
    /// </summary>
    public class GimmickObserver : ManagedMono
    {
        List<IGimmick> _gimmicks = new List<IGimmick>();

        /// <summary>
        /// ギミック一覧
        /// </summary>
        public List<IGimmick> Gimmicks => _gimmicks;

        /// <summary>
        /// 購読
        /// </summary>
        /// <param name="gimmick"></param>
        public void SubscribeGimmick(IGimmick gimmick) => _gimmicks.Add(gimmick);

        /// <summary>
        /// 購読解除
        /// </summary>
        /// <param name="gimmick"></param>
        public void UnsubscribeGimmick(IGimmick gimmick) => _gimmicks.Remove(gimmick);
    }
}