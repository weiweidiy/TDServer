using System;

namespace JFramework
{
    /// <summary>
    /// type = 3 bp属性伤害公式傷害公式
    /// </summary>
    public class FormulaBpDamage : FormulaDamage
    {
        public override double GetHitValue(CombatExtraData extraData)
        {
            var baseValue = base.GetHitValue(extraData);
            var bpDamage = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.BPDamage);
            var bpDamageAnti = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.BPDamageAnit);
            var dmgRate = (bpDamage - bpDamageAnti);
            dmgRate = Math.Max(-1, dmgRate); //不能小于-1，否则伤害小于0了
            return baseValue * (1 + dmgRate);
        }
    }

}