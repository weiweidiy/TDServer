using System.Collections.Generic;
using System;

namespace JFramework.BattleReportSystem
{
    public interface IActionManager
    {
        event Action<IBattleAction, List<IBattleUnit>, float> onStartCast;

        /// <summary>
        /// 被打断事件
        /// </summary>
        event Action<List<IBattleAction>, float> onInterrupt;


        event Action<IBattleAction, float> onStartCD;

        /// <summary>
        /// 即将命中目标
        /// </summary>
        event Action<IBattleAction, IBattleUnit, ExecuteInfo> onHittingTarget;

        /// <summary>
        /// 标记是否释放队列正在繁忙
        /// </summary>
        bool IsBusy { get; }

        void Update(CombatFrame frame);

        void Initialize(IBattleUnit owner);

        /// <summary>
        /// unit死亡时调用
        /// </summary>
        void OnDead();

        /// <summary>
        /// 眩晕时调用
        /// </summary>
        void OnStunning(ActionType actionType, float duration);

        /// <summary>
        /// 眩晕恢复
        /// </summary>
        void OnResumeFromStunning(ActionType actionType);

        /// <summary>
        /// 沉默时调用
        /// </summary>
        void OnSilence();

        /// <summary>
        /// 打断动作
        /// </summary>
        List<IBattleAction> Interrupt(List<IBattleAction> actions);

        /// <summary>
        /// 获取指定类型技能（普攻，技能）
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        List<IBattleAction> GetActionsByType(ActionType actionType);

        /// <summary>
        /// 根据动作ID获取动作
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        IBattleAction GetAction(int actionId);
        IBattleAction[] GetAll();
    }
}