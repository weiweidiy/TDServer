using System.Collections.Generic;
using System;

namespace JFramework
{
    public interface ICombatAction
    {
        #region 委托事件
        /// <summary>
        /// 通知actionmanager 可以释放了
        /// </summary>
        event Action<CombatExtraData> onTriggerOn;

        /// <summary>
        /// 触发了，群体也只会返回首目标
        /// </summary>
        event Action<CombatExtraData> onStartExecuting;


        /// <summary>
        /// 开始进入冷却
        /// </summary>
        event Action<CombatExtraData> onStartCD;


        /// <summary>
        /// 即将命中目标
        /// </summary>
        event Action<CombatExtraData> onHittingTargets;

        /// <summary>
        /// 已经命中
        /// </summary>
        event Action<CombatExtraData> onTargetsHittedComplete;

        /// <summary>
        /// 即将命中目标
        /// </summary>
        event Action<CombatExtraData> onHittingTarget;

        /// <summary>
        /// 已经命中
        /// </summary>
        event Action<CombatExtraData> onTargetHittedComplete;

        #endregion


        int Id { get; }

        ActionType Type { get;  }

        ActionMode Mode { get;  }

        //#region 属性
        ///// <summary>
        ///// 类型 区分普通动作和技能动作
        ///// </summary>
        //ActionType Type { get; }

        ///// <summary>
        ///// 动作模式：主动，被动
        ///// </summary>
        //ActionMode Mode { get; }    

        //#region 生命周期
        //void OnStart(); //开始更新前只调用1次

        //void OnEnable(); //触发时调用

        ////void Update(BattleFrame frame);

        //void OnStop(); //停止更新

        //#endregion

        //#endregion

        #region 状态切换
        /// <summary>
        /// 待机状态
        /// </summary>
        void SwitchToTrigging();

        /// <summary>
        /// 释放技能，返回释放周期
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="units"></param>
        /// <returns>施放持续时间</returns>
        float SwitchToExecuting();

        /// <summary>
        /// 进入CD
        /// </summary>
        float SwitchToCd();

        /// <summary>
        /// 设置这个动作是否可触发
        /// </summary>
        /// <param name="active"></param>
        void SwitchToDisable();

        #endregion

        //#region 查询
        ///// <summary>
        ///// 是否冷却完成了
        ///// </summary>
        ///// <returns></returns>
        //bool IsCDComplete();

        ///// <summary>
        ///// 是否正在执行
        ///// </summary>
        //bool IsExecuting();

        ///// <summary>
        ///// 是否满足了触发条件
        ///// </summary>
        ///// <returns></returns>
        //bool CanCast();


        ///// <summary>
        ///// 获取执行周期
        ///// </summary>
        ///// <returns></returns>
        //float GetCastDuration();

        ///// <summary>
        ///// 获取当前状态
        ///// </summary>
        ///// <returns></returns>
        //string GetCurState();

        ///// <summary>
        ///// 获取冷却触发器
        ///// </summary>
        ///// <returns></returns>
        //IBattleTrigger GetCDTrigger();
        //#endregion

        //#region 功能接口
        ///// <summary>
        ///// 搜索目标
        ///// </summary>
        ///// <returns></returns>
        //List<ICombatUnit> FindTargets(object[] args);

        ///// <summary>
        ///// 准备执行效果
        ///// </summary>
        ///// <param name="caster"></param>
        ///// <param name="action"></param>
        ///// <param name="targets"></param>
        //void ReadyToExecute(ICombatUnit caster, ICombatAction action, List<ICombatUnit> targets);

        ///// <summary>
        ///// 打断
        ///// </summary>
        //void Interrupt();

        ///// <summary>
        ///// 设置是否可用
        ///// </summary>
        //void SetEnable(bool enable);

        //#endregion

        //#region 修改参数
        ///// <summary>
        ///// 修改参数
        ///// </summary>
        ///// <param name="args"></param>
        //void SetConditionTriggerArgs(float[] args);
        //void SetFinderArgs(float[] args);
        //void SetExecutorArgs(float[] args);
        //void SetCdArgs(float[] args);
        ///// <summary>
        ///// 获取参数
        ///// </summary>
        ///// <returns></returns>
        //float[] GetConditionTriggerArgs();
        //float[] GetFinderArgs();
        //float[] GetCdArgs();
        //float[] GetExecutorArgs();

        ///// <summary>
        ///// 重置参数
        ///// </summary>
        ///// <param name="args"></param>
        //void ResetConditionTriggerArgs(float[] args);
        //void ResetFinderArgs(float[] args);
        //void ResetExecutorArgs(float[] args);
        //void ResetCdArgs(float[] args);
        //#endregion
    }




}