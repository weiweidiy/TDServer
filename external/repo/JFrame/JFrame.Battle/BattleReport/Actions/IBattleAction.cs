using System;
using System.Collections.Generic;

namespace JFramework
{
    public interface IBattleAction : IUnique, IAttachOwner, IUpdateable
    {
        #region 委托事件
        /// <summary>
        /// 通知可以释放了
        /// </summary>
        event Action<IBattleAction> onCanCast;
        /// <summary>
        /// 通知条件满足，可以释放
        /// </summary>
        void NotifyCanCast();

        /// <summary>
        /// 触发了，群体也只会返回首目标
        /// </summary>
        event Action<IBattleAction, List<IBattleUnit>, float> onStartCast;

        /// <summary>
        /// 通知开始释放
        /// </summary>
        void NotifyStartCast(List<IBattleUnit> targets, float duration);

        /// <summary>
        /// 开始进入冷却
        /// </summary>
        event Action<IBattleAction, float> onStartCD;

        /// <summary>
        /// 通知进入冷却
        /// </summary>
        /// <param name="cd"></param>
        void NotifyStartCD(float cd);

        /// <summary>
        /// 即将命中目标
        /// </summary>
        event Action<IBattleAction, IBattleUnit, ExecuteInfo> onHittingTarget;

        /// <summary>
        /// 已经命中
        /// </summary>
        event Action<IBattleAction, IBattleUnit, ExecuteInfo, IBattleUnit> onHittedComplete;

        #endregion

        #region 生命周期
        /// <summary>
        /// 附加到持有者上时调用
        /// </summary>
        /// <param name="owner"></param>
        void OnAttach(IBattleUnit owner);

        void Update(CombatFrame frame);

        #endregion

        #region 属性
        ///// <summary>
        ///// 持有者
        ///// </summary>
        //IBattleUnit Owner { get;  }

        ///// <summary>
        ///// 名字
        ///// </summary>
        //string Name { get; }

        ///// <summary>
        ///// id
        ///// </summary>
        //int Id { get; }

        /// <summary>
        /// 类型 区分普通动作和技能动作
        /// </summary>
        ActionType Type { get; }

        /// <summary>
        /// 动作模式：主动，被动
        /// </summary>
        ActionMode Mode { get; }

        #endregion

        #region 状态切换
        /// <summary>
        /// 待机状态
        /// </summary>
        void Standby();

        /// <summary>
        /// 释放技能，返回释放周期
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        float Cast();

        /// <summary>
        /// 进入CD
        /// </summary>
        void EnterCD();

        /// <summary>
        /// 设置这个动作是否可触发
        /// </summary>
        /// <param name="active"></param>
        void SetDead(bool active);
        #endregion

        #region 查询
        /// <summary>
        /// 是否冷却完成了
        /// </summary>
        /// <returns></returns>
        bool IsCDComplete();

        /// <summary>
        /// 是否正在执行
        /// </summary>
        bool IsExecuting();

        /// <summary>
        /// 是否满足了触发条件
        /// </summary>
        /// <returns></returns>
        bool CanCast();


        /// <summary>
        /// 获取执行周期
        /// </summary>
        /// <returns></returns>
        float GetCastDuration();

        /// <summary>
        /// 获取当前状态
        /// </summary>
        /// <returns></returns>
        string GetCurState();

        /// <summary>
        /// 获取冷却触发器
        /// </summary>
        /// <returns></returns>
        IBattleTrigger GetCDTrigger();
        #endregion

        #region 功能接口
        /// <summary>
        /// 搜索目标
        /// </summary>
        /// <returns></returns>
        List<IBattleUnit> FindTargets(object[] args);

        /// <summary>
        /// 准备执行效果
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="targets"></param>
        void ReadyToExecute(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets);

        /// <summary>
        /// 打断
        /// </summary>
        void Interrupt();

        /// <summary>
        /// 设置是否可用
        /// </summary>
        void SetEnable(bool enable);

        #endregion


    }
}