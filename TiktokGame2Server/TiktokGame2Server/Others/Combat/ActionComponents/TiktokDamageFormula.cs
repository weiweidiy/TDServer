using JFramework.Game;

namespace TiktokGame2Server.Others
{

    /// <summary>
    /// 常用的伤害计算公式，攻击-防御
    /// </summary>
    public class TiktokDamageFormula : JCombatFormulaBase
    {
        public TiktokDamageFormula(float[] args) : base(args)
        {
        }

        protected override int GetValidArgsCount()
        {
            return 1;
        }

        /// <summary>
        /// 获取伤害比率系数
        /// </summary>
        /// <returns></returns>
        protected float GetDamageRate()
        {
            return GetArg(0);
        }

        public override void CalcHitValue(IJAttributeableUnit target, ref float value, IJCombatExecutorExecuteArgs executeArgs)
        {
            //伤害= 释放者攻击力 * 释放者Power - 目标防御力 * 目标Def
            var caster = query.GetUnit(GetOwner().GetCaster());
            var hp = caster.GetAttribute(TiktokAttributesType.Hp.ToString()) as GameAttributeInt;
            var baseDmg = (hp.CurValue / 4);

            var power = caster.GetAttribute(TiktokAttributesType.Power.ToString()) as GameAttributeInt;
            var attack = caster.GetAttribute(TiktokAttributesType.Attack.ToString()) as GameAttributeInt;

            var targetDef = target.GetAttribute(TiktokAttributesType.Def.ToString()) as GameAttributeInt;
            var targetDefence = target.GetAttribute(TiktokAttributesType.Defence.ToString()) as GameAttributeInt;

            //兵种系数
            var soldierRate = (attack.CurValue - targetDefence.CurValue) / 100f;
            //samurai系数
            var samuraiRate = (power.CurValue - targetDef.CurValue) / 50f;

            //计算伤害f
            var damage = baseDmg * Math.Max(0.1f, (1+ soldierRate + samuraiRate));

            value = damage * GetDamageRate();
        }


    }
}



//public class FormationInfo
//{
//    public int FormationPoints { get; set; }

//    public required JCombatUnitInfo UnitInfo { get; set; }
//}

///// <summary>
///// 获取玩家武士在阵型中的坐标点位
///// </summary>
///// <returns></returns>
//Func<string, int> CreateFormationPointDelegate(int formationType)
//{
//    // 从formation中获取武士的点位
//    return (unitUid) => // to do: 需要所有的战斗单位（包括NPC）
//    {
//        //从atkFormations中获取对应的阵型点位
//        if (lstFormationQuery == null || lstFormationQuery.Count == 0)
//        {
//            throw new Exception("没有可用的阵型");
//        }
//        var formation = lstFormationQuery.FirstOrDefault(f =>  f.UnitInfo.Uid == unitUid);
//        return formation?.Point ?? -1; // 如果没有找到对应的阵型点位，返回-1
//    };
//}
