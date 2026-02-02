using System;

namespace JFramework
{
    /// <summary>
    /// type 10: 反射伤害：參數：0：执行時間 1：傷害加成 
    /// </summary>
    public class ExecutorCombatTurnBackDamage : ExecutorCombatNormal
    {
        public override int GetValidArgsCount()
        {
            return 2;
        }

        protected float GetRateArg()
        {
            return GetCurArg(1);
        }

        public ExecutorCombatTurnBackDamage(CombatBaseFinder combinFinder, CombatBaseFormula formulua) : base(combinFinder, formulua)
        {
        }

        protected override void SetValueType(CombatExtraData data)
        {
            data.ValueType = CombatValueType.TurnBackDamage;
        }

        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            //这里的释放者是携带反射技能的单位
            var attr = (double)data.Caster.GetAttributeCurValue(CombatAttribute.FightBackCoef);
            //target是需要反射给的目标
            //var antiAttr = (double)target.GetAttributeCurValue(CombatAttribute.FightBackAnti);

            //var finalAttr = Math.Max(0, attr - antiAttr);

            //这里extra是收到的伤害
            data.Value = data.ExtraArg * (GetRateArg() + attr);
            target.OnDamage(data);
        }

        protected override double GetExecutorValue()
        {
            return GetRateArg();
        }
    }
}