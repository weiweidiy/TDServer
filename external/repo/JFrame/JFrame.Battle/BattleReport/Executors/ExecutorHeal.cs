
using System;
using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// 治疗效果 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：加血量施法者血量上限百分比  type = 3
    /// </summary>
    public class ExecutorHeal : ExecutorDamage
    {
        public ExecutorHeal(FormulaManager formulaManager, float[] args) : base(formulaManager, args) { }

        public override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            return caster.MaxHP * arg;
        }

        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] arg = null)
        {
            foreach (var target in targets)
            {
                var heal = GetValue(caster, action, target);
                //to do: unit.getbuffvalue(bufftype, dmg) 返回最终受伤值
                var needHp = target.MaxHP - target.HP;

                heal = Math.Min(heal, needHp);

                target.OnHeal(caster, action, new ExecuteInfo() { Value = (int)heal });
            }

        }
    }
}


///// <summary>
///// 治疗效果 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：加血量施法者血量上限百分比  type = 3
///// </summary>
//public class ExecutorHeal : ExecutorDamage
//{
//    public ExecutorHeal(FormulaManager formulaManager, float[] args) : base(formulaManager, args) { }

//    public override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
//    {
//        return caster.MaxHP * arg; 
//    }

//    public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
//    {
//        foreach (var target in targets)
//        {
//            var heal = GetValue(caster, action, target);
//            //to do: unit.getbuffvalue(bufftype, dmg) 返回最终受伤值
//            var needHp = target.MaxHP - target.HP;

//            heal = Math.Min(heal, needHp);

//            target.OnHeal(caster, action, new ExecuteInfo() { Value = (int)heal });
//        }

//    }
//}