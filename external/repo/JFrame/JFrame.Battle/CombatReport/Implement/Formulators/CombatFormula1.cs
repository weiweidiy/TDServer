using System;
using static System.Net.Mime.MediaTypeNames;

namespace JFramework
{
    /// <summary>
    /// 1 ： 参数1：来源（0=释放者，1=目标）    吃命中 
    /// </summary>
    public class CombatFormula1 : CombatBaseFormula
    {
        public override int GetValidArgsCount()
        {
            return 2;
        }

        int GetTeamArg()
        {
            return (int)GetCurArg(0);
        }

        int GetAttrTypeArg()
        {
            return (int)GetCurArg(1);
        }

        public override double GetHitValue(CombatExtraData extraData)
        {
            var teamArg = GetTeamArg();
            CombatUnit unit = null;
            switch (teamArg)
            {
                case 0:
                    unit = extraData.Caster;
                    break;
                case 1:
                    unit = extraData.Target;
                    break;
                default:
                    throw new System.Exception($"{GetType()}没有实现队伍参数 {teamArg}");
            }

            if (unit == null)
                throw new System.Exception($"{GetType()} 没有找到对应的unit目标，无法计算数值");

            var attrId = GetAttrTypeArg();
            var type = (CombatAttribute)attrId;
            return (double)unit.GetAttributeCurValue(type) * extraData.Value; //這個value是執行器參數
        }
    }

    /// <summary>
    /// 2：  参数1：来源（0=释放者，1=目标）    吃伤害加成，吃小怪Boss
    /// </summary>
    public class CombatFormula2 : CombatFormula1
    {
        public override bool IsHit(CombatExtraData extraData)
        {
            return true;
        }

        public override double GetHitValue(CombatExtraData extraData)
        {
            var dmg = base.GetHitValue(extraData);

            var caster = extraData.Caster;
            var target = extraData.Target;

            var dmgAdvance = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.DamageAdvance);
            var dmgAnti = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.DamageAnti);
            var monsterAdd = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.MonsterAdd);
            var bossAdd = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.BossAdd);
            var enemyAdd = target.IsMainType(UnitMainType.Monster) ? monsterAdd : target.IsMainType(UnitMainType.Boss) ? bossAdd : 0;

            var dmgRate = (dmgAdvance - dmgAnti);
            dmgRate = Math.Max(-1, dmgRate); //不能小于-1，否则伤害小于0了

            return dmg * (1 + dmgRate) * (1 + enemyAdd);
        }
    }

    /// <summary>
    /// 参数1：来源（0=释放者，1=目标）    吃伤害加成，吃小怪Boss，吃元素
    /// </summary>
    public class CombatFormula3 : CombatFormula2
    {
        public override double GetHitValue(CombatExtraData extraData)
        {
            var dmg = base.GetHitValue(extraData);

            var elemt = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.Elemt);
            var elemtResist = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.ElemtResist);
            var elemtRate = elemt - elemtResist;
            elemtRate = Math.Max(elemtRate - 1, elemtRate);
            if ((int)extraData.ValueType < (int)CombatValueType.FireDamage)
                elemtRate = 0;

            return dmg * (1 + elemtRate);
        }
    }

    /// <summary>
    /// 4 ： 参数1：来源（0=释放者，1=目标）    吃命中 ，吃伤害加成，吃小怪Boss,  吃暴击，
    /// </summary>
    public class CombatFormula4 : CombatFormula1
    {
        Utility utility = new Utility();
        public override double GetHitValue(CombatExtraData extraData)
        {
            var dmg = base.GetHitValue(extraData);

            var caster = extraData.Caster;
            var target = extraData.Target;

            var dmgAdvance = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.DamageAdvance);
            var dmgAnti = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.DamageAnti);
            var monsterAdd = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.MonsterAdd);
            var bossAdd = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.BossAdd);
            var enemyAdd = target.IsMainType(UnitMainType.Monster) ? monsterAdd : target.IsMainType(UnitMainType.Boss) ? bossAdd : 0;
            var cri = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.Critical);
            var criAnti = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.CriticalAnti);
            var criDamage = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.CriticalDamage);

            bool isCri = true;
            if (cri - criAnti <= 0)
                isCri = false;
            else
                isCri = utility.RandomHit((float)(cri - criAnti) * 100);

            extraData.IsCri = isCri;
            if (isCri)
                dmg = (long)(dmg * (float)(1 + criDamage));

            var dmgRate = (dmgAdvance - dmgAnti);
            dmgRate = Math.Max(-1, dmgRate); //不能小于-1，否则伤害小于0了

            return dmg * (1 + dmgRate) * (1 + enemyAdd);
        }
    }

    /// <summary>
    /// 5 ： 参数1：来源（0=释放者，1=目标）    吃命中 ，吃伤害加成，吃小怪Boss，吃暴击，吃臂炮加成
    /// </summary>
    public class CombatFormula5 : CombatFormula4
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

    /// <summary>
    /// 6 ： 参数1：来源（0=释放者，1=目标）    吃命中 ，吃伤害加成，吃小怪Boss，吃暴击，吃元素
    /// </summary>
    public class CombatFormula6 : CombatFormula4
    {
        public override double GetHitValue(CombatExtraData extraData)
        {
            var dmg = base.GetHitValue(extraData);
            var elemt = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.Elemt);
            var elemtResist = (double)extraData.Target.GetAttributeCurValue(CombatAttribute.ElemtResist);
            var elemtRate = elemt - elemtResist;
            elemtRate = Math.Max(elemtRate - 1, elemtRate);
            if ((int)extraData.ValueType < (int)CombatValueType.FireDamage)
                elemtRate = 0;

            return dmg * (1 + elemtRate);
        }
    }


    //public class CombatFormula7 : CombatFormula6
    //{
    //    public override double GetHitValue(CombatExtraData extraData)
    //    {
    //        var dmg = base.GetHitValue(extraData);

    //        return dmg * GetRateByHp();
    //    }


    //    float GetRateByHp()
    //    {
    //        return 2f;
    //    }
    //}
}