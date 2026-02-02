using System;

namespace JFramework
{
    /// <summary>
    ///  type 8 数值加成加成 参数0：执行周期 参数1：数值类型（1=伤害， 2=治疗） 参数2：加成倍率
    /// </summary>
    public class ExecutorCombatValueMulti : ExecutorCombatNormal
    {
        public ExecutorCombatValueMulti(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        public override int GetValidArgsCount()
        {
            return 3;
        }


        protected int GetValueTypeArg()
        {
            return (int)GetCurArg(1);
        }
        protected float GetRate()
        {
            return GetCurArg(2);
        }


        protected override double GetExecutorValue()
        {
            return GetRate();
        }

        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            if (data.ValueType != (CombatValueType)GetValueTypeArg())
                return;

            data.Value *= GetRate();
        }

        protected override void SetValueType(CombatExtraData data)
        {
            //不改变原有类型
        }
    }
}