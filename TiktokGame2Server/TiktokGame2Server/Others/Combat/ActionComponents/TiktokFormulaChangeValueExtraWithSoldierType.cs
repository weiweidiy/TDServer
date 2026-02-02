using Game.Share;
using JFramework.Game;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Others
{
    /// <summary>
    /// 根据兵种类型，额外增加数值的公式变更类
    /// </summary>
    public class TiktokFormulaChangeValueExtraWithSoldierType : JCombatFormulaChangeValue
    {
        public TiktokFormulaChangeValueExtraWithSoldierType(float[] args) : base(args)
        {
        }

        protected override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 2;
        }

        int GetSoldierType()
        {
            return (int)GetArg(2);
        }

        float GetExtraValue()
        {
            return GetArg(3);
        }

        protected override float GetCalcValueArg(IJAttributeableUnit target, IJCombatExecutorExecuteArgs executeArgs)
        {
            var unit = target as JCombatTurnBasedUnit;
            if (unit == null)
            {
                throw new System.Exception("TiktokFormulaChangeValueExtraWithSoldierType can only be used with JCombatTurnBasedUnit targets.");
                //return base.GetCalcValueArg(target, executeArgs);
            }

            var unitInfo = unit.GetUnitInfo() as TiktokJCombatUnitInfo;
            if (unitInfo == null)
            {
                throw new System.Exception("TiktokFormulaChangeValueExtraWithSoldierType can only be used with TiktokJCombatUnitInfo targets.");
            }

            //var soldierBusinessId = unitInfo.SoldierBusinessId;
            if (unitInfo.SoldierType == (SoldierType)GetSoldierType())
            {
                return GetExtraValue();
            }
            return base.GetCalcValueArg(target, executeArgs);
        }
    }
}
