
using System.Collections.Generic;

namespace JFramework
{
  


    /// <summary>
    /// 伤害递增 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：初始伤害倍率 5：递增次数 6：递增倍率（与参数4相加） type = 7
    /// </summary>
    public class ExecutorIncrementalDamage :ExecutorDamage
    {
        /// <summary>
        /// 递增次数
        /// </summary>
        int incrementCount;

        /// <summary>
        /// 递增倍率
        /// </summary>
        float incrementRate;

        /// <summary>
        /// 当前次数
        /// </summary>
        int curCount = 0;

        public ExecutorIncrementalDamage(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args.Length >= 6)
            {
                incrementCount = (int)args[4];
                incrementRate = args[5];
            }
        }

        public override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            var value = arg + curCount * incrementRate;
            return caster.Atk * value;
        }

        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] arg = null)
        {
            base.Hit(caster, action, targets);

            if (curCount < incrementCount)
            {
                curCount++;
            }
        }
    }
}

///// <summary>
///// 伤害递增 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：初始伤害倍率 5：递增次数 6：递增倍率（与参数4相加） type = 7
///// </summary>
//public class ExecutorIncrementalDamage : ExecutorDamage
//{
//    /// <summary>
//    /// 递增次数
//    /// </summary>
//    int incrementCount;

//    /// <summary>
//    /// 递增倍率
//    /// </summary>
//    float incrementRate;

//    /// <summary>
//    /// 当前次数
//    /// </summary>
//    int curCount = 0;

//    public ExecutorIncrementalDamage(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
//    {
//        if(args.Length >= 6)
//        {
//            incrementCount = (int)args[4];
//            incrementRate = args[5];
//        }
//    }

//    public override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
//    {
//        var value = arg + curCount * incrementRate;
//        return caster.Atk * value;
//    }

//    public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
//    {
//        base.Hit(caster, action, targets);

//        if (curCount < incrementCount)
//        {
//            curCount++;
//        }
//    }
//}
