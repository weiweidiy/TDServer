using System;

namespace JFramework
{
    public interface ICombatExecutor
    {
        /// <summary>
        /// 即将命中目标
        /// </summary>
        event Action<CombatExtraData> onHittingTargets;

        event Action<CombatExtraData> onHittingTarget;

        /// <summary>
        /// 命中完成
        /// </summary>
        event Action<CombatExtraData> onTargetsHittedComplete;
        event Action<CombatExtraData> onTargetHittedComplete;

        /// <summary>
        /// 执行
        /// </summary>
        void Execute(CombatExtraData extraData);
    }


}