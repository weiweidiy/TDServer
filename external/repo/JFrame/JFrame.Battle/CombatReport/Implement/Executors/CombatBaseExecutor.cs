using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 参数：0 执行周期
    /// </summary>
    public abstract class CombatBaseExecutor : BaseActionComponent, ICombatExecutor
    {
        public event Action<CombatExtraData> onHittingTargets;
        protected void NotifyHittingTargets(CombatExtraData extraData) => onHittingTargets?.Invoke(extraData);

        public event Action<CombatExtraData> onTargetsHittedComplete;

        protected void NotifyTargetsHittedComplete(CombatExtraData extraData) => onTargetsHittedComplete?.Invoke(extraData);

        public event Action<CombatExtraData> onHittingTarget;
        protected void NotifyHittingTarget(CombatExtraData extraData) => onHittingTarget?.Invoke(extraData);

        public event Action<CombatExtraData> onTargetHittedComplete;
        protected void NotifyTargetHittedComplete(CombatExtraData extraData) => onTargetHittedComplete?.Invoke(extraData);


        /// <summary>
        /// 查找器
        /// </summary>
        protected CombatBaseFinder finder;

        /// <summary>
        /// 公式计算
        /// </summary>
        protected CombatBaseFormula formula;

        /// <summary>
        /// 是否在执行中
        /// </summary>
        protected bool isExecuting;

        /// <summary>
        /// 透传数据，从trigger而来
        /// </summary>
        protected CombatExtraData extraData;

        public CombatBaseExecutor(CombatBaseFinder combinFinder, CombatBaseFormula formula)
        {
            this.finder = combinFinder;
            this.formula = formula;
        }

        /// <summary>
        /// 开始释放
        /// </summary>
        /// <param name="extraData"></param>
        public virtual void Execute(CombatExtraData extraData)
        {
            this.extraData = extraData;
            isExecuting = true;
        }

        /// <summary>
        /// 重置為未觸發
        /// </summary>
        public virtual void Reset()
        {
            isExecuting = false;
            extraData = null;
        }

        /// <summary>
        /// 獲取執行周期
        /// </summary>
        /// <returns></returns>
        public virtual float GetDuration()
        {
            return GetCurArg(0);
        }

        /// <summary>
        /// 獲取目標
        /// </summary>
        /// <param name="extraData"></param>
        /// <returns></returns>
        protected List<CombatUnit> GetTargets(CombatExtraData extraData)
        {
            List<CombatUnit> targets = new List<CombatUnit>();

            if (finder != null)
            {
                targets = finder.FindTargets(extraData);
                if (targets != null && targets.Count > 0) //换新的目标了
                    extraData.Targets = targets;
            }
            else
            {
                if (extraData.Targets == null || extraData.Targets.Count == 0)
                    return targets;

                targets = extraData.Targets;
            }

            return targets;
        }

        protected override void OnUpdate(CombatFrame frame)
        {
            //throw new NotImplementedException();
        }

        public override void OnAttach(CombatAction target)
        {
            base.OnAttach(target);
            if(finder != null) { finder.OnAttach(target); }
            if(formula != null) { formula.OnAttach(target); }
        }
    }
}