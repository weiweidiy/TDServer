using System;

namespace JFramework
{

    /// <summary>
    /// 属性计算器
    /// </summary>
    public abstract class CombatBaseFormula : BaseActionComponent, ICombatFormula
    {
        Utility utility = new Utility();

        public abstract double GetHitValue(CombatExtraData extraData);

        public virtual bool IsHit(CombatExtraData extraData)
        {
            var caster = extraData.Caster;
            var target = extraData.Target;

            var hit = (double)caster.GetAttributeCurValue(CombatAttribute.Hit);
            var dodge = (double)target.GetAttributeCurValue(CombatAttribute.Dodge);

            var value = hit - dodge;
            value = Math.Max(value, -1);
            return utility.RandomHit((float)(1 + value) * 100);
        }

        protected override void OnUpdate(CombatFrame frame)
        {
        }
    }

}