using System;
using System.Collections.Generic;

namespace JFramework
{

    public interface IBattleExecutor : IOldAttachable
    {
        /// <summary>
        /// 即将命中目标
        /// </summary>
        event Action<IBattleUnit, ExecuteInfo> onHittingTarget;

        /// <summary>
        /// 命中完成
        /// </summary>
        event Action<IBattleUnit, ExecuteInfo, IBattleUnit> onHittedComplete;

        /// <summary>
        /// 是否激活
        /// </summary>
        bool Executing { get; set; }

        /// <summary>
        ///  命中效果
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="target"></param>
        void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> target, object[] args = null);

        /// <summary>
        /// 准备开始执行
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="targets"></param>
        void ReadyToExecute(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] args = null);

        /// <summary>
        /// 更新帧，可以延迟命中
        /// </summary>
        /// <param name="frame"></param>
        void Update(CombatFrame frame);

        /// <summary>
        /// 重新激活技能
        /// </summary>
        void Reset();

        /// <summary>
        /// 获取CD
        /// </summary>
        /// <returns></returns>
        float[] GetArgs();

        /// <summary>
        /// 设置cd
        /// </summary>
        /// <param name="cd"></param>
        /// <exception cref="NotImplementedException"></exception>
        void SetArgs(float[] args);
    }

    ///// <summary>
    ///// 战斗效果执行器，实际的技能效果，比如伤害，加血，加BUFF等
    ///// </summary>
    //public interface IBattleExecutor
    //{
    //    /// <summary>
    //    /// 即将命中目标
    //    /// </summary>
    //    event Action<IBattleUnit, ExecuteInfo> onHittingTarget;

    //    /// <summary>
    //    /// 是否激活
    //    /// </summary>
    //    bool Active { get; }

    //    IBattleAction Owner { get; }

    //    /// <summary>
    //    ///  命中效果
    //    /// </summary>
    //    /// <param name="caster"></param>
    //    /// <param name="action"></param>
    //    /// <param name="target"></param>
    //    void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> target);

    //    /// <summary>
    //    /// 准备开始执行
    //    /// </summary>
    //    /// <param name="caster"></param>
    //    /// <param name="action"></param>
    //    /// <param name="targets"></param>
    //    void ReadyToExecute(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets);

    //    /// <summary>
    //    /// 更新帧，可以延迟命中
    //    /// </summary>
    //    /// <param name="frame"></param>
    //    void Update(BattleFrame frame);

    //    void OnAttach(IBattleAction action);

    //    /// <summary>
    //    /// 重新激活技能
    //    /// </summary>
    //    void Reset();

    //    ///// <summary>
    //    ///// 打断
    //    ///// </summary>
    //    //void Interrupt();


    //}
}