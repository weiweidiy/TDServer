using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 普通执行器 只能固定打1次 参数：0 执行周期
    /// </summary>
    public abstract class ExecutorCombatNormal : CombatBaseExecutor
    {
        protected int count = 0;


        public ExecutorCombatNormal(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }


        protected virtual float GetCountArg()
        {
            return 1;
        }

        public override void Reset()
        {
            base.Reset();
            count = 0;
        }

        protected override void OnUpdate(CombatFrame frame)
        {
            base.OnUpdate(frame);

            if (!isExecuting)
                return;

            if (count >= GetCountArg())
            {
                isExecuting = false;
                return;
            }
            Hit();
        }


        /// <summary>
        /// 命中
        /// </summary>
        protected CombatUnit Hit()
        {
            List<CombatUnit> targets = GetTargets(extraData);

            var ExecutorExtraData = extraData.Clone() as CombatExtraData;

            //double baseValue = 1;
            //获取数值
            ExecutorExtraData.Value = /*baseValue **/ GetExecutorValue()  *  extraData.FoldCount;

            ///首目标
            CombatUnit primaryUnit = null;

            //即将命中
            NotifyHittingTargets(ExecutorExtraData);

            foreach (var target in targets)
            {
                //记录存活的首要单位
                if(primaryUnit == null && target.IsAlive())
                    primaryUnit = target;

                var data = ExecutorExtraData.Clone() as CombatExtraData;
                data.Target = target;

                SetValueType(data);

                if (formula != null)
                {
                    var miss = !formula.IsHit(data);
                    data.IsMiss = miss;
                    if (miss)
                    {
                        data.Value = 0;
                    }

                    else
                        data.Value = formula.GetHitValue(data);
                } 
                
                NotifyHittingTarget(data); //即将命中单个单位
                DoHit(target, data);
                NotifyTargetHittedComplete(data);
            }
            //命中完成了
            NotifyTargetsHittedComplete(ExecutorExtraData);
            count++;

            return primaryUnit;
        }

        /// <summary>
        /// 设置本次执行的类型：伤害，治疗，反击等
        /// </summary>
        /// <param name="data"></param>
        protected abstract void SetValueType(CombatExtraData data);

        /// <summary>
        /// 执行参数倍率
        /// </summary>
        /// <returns></returns>
        protected abstract double GetExecutorValue();

        

        protected abstract void DoHit(CombatUnit target, CombatExtraData data);

    }
}