using System;
using System.Diagnostics;

namespace JFramework
{
    /// <summary>
    ///     /// <summary>
    ///  type =11  参数：0 执行周期 1=attrId, 2=倍率 , 3=係數1， 4=係數2
    /// </summary>
    /// </summary>
    public class ExecutorCombatChangeAttrByLevel : ExecutorCombatChangeAttribute
    {
        //string uid = "ExecutorCombatChangeAttrByLevel";
        public ExecutorCombatChangeAttrByLevel(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
           // UnityEngine.Debug.LogError("ExecutorCombatChangeAttrByLevel");
        }

        public override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 2;
        }

        public float GetK1()
        {
            return GetCurArg(3);
        }

        public float GetK2()
        {
            return GetCurArg(4);
        }

        protected override float GetRateArg()
        {
            var baseArg = base.GetRateArg();
            //UnityEngine.Debug.LogError("baseArg * GetLevel() " + baseArg * GetLevel());
            return baseArg * GetLevel();
        }

        private int GetLevel()
        {
            var curHp = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.CurHp);
            var maxHp = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.MaxHP);
            var totalDamage = maxHp - curHp;
            //縂伤害/30000）^0.6
            return (int)Math.Ceiling(Math.Pow((totalDamage / GetK1()), GetK2()));
        }

        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            target.RemoveExtraValue((CombatAttribute)GetAttrIdArg(), uid);

            base.DoHit(target, data);

            // UnityEngine.Debug.LogError("ExecutorCombatChangeAttrByLevel DoHit  atk: " + target.GetAttributeCurValue(CombatAttribute.ATK));
        }
    }
}