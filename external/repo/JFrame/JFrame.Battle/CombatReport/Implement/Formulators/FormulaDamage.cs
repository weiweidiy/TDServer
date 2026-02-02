using System;
using static System.Net.Mime.MediaTypeNames;

namespace JFramework
{

    /// <summary>
    /// type = 2 普通傷害公式
    /// </summary>
    public class FormulaDamage : CombatBaseFormula
    {
        Utility utility = new Utility();
        public override double GetHitValue(CombatExtraData extraData)
        {
            var caster = extraData.Caster;
            var target = extraData.Target;

            var atk = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.ATK);//攻擊屬性
            var actiongRate = extraData.Value; //技能參數
            var cri = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.Critical);
            var criAnti = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.CriticalAnti);
            var criDamage = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.CriticalDamage);
            var dmgAdvance = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.DamageAdvance);
            var dmgAnti = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.DamageAnti);
            var monsterAdd = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.MonsterAdd);
            var bossAdd = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.BossAdd);
            var enemyAdd = target.IsMainType(UnitMainType.Monster) ? monsterAdd : target.IsMainType(UnitMainType.Boss) ? bossAdd : 0;
            var elemt = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.Elemt);
            var elemtResist = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.ElemtResist);
            var elemtRate = elemt - elemtResist;
            elemtRate = Math.Max(elemtRate - 1, elemtRate);
            if ((int)extraData.ValueType < (int)CombatValueType.FireDamage)
                elemtRate = 0;

            //暴擊判定
            bool isCri = true;
            if (cri - criAnti <= 0)
                isCri = false;
            else
                isCri = utility.RandomHit((float)(cri - criAnti) * 100);

            extraData.IsCri = isCri;

            long damage = (long)(atk * actiongRate);

            if (isCri)
                damage = (long)(damage * (float)(1 + criDamage));

            var dmgRate = (dmgAdvance - dmgAnti);
            dmgRate = Math.Max(-1, dmgRate); //不能小于-1，否则伤害小于0了

            return damage * (1 + dmgRate) * (1 + enemyAdd) * (1+ elemtRate);
        }

        public override int GetValidArgsCount()
        {
            return 0;
        }
    }

}