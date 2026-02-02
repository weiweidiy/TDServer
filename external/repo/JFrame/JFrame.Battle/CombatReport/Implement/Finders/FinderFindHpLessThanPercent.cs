using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 查找血量百分比少于一定值的单位 , 参数：0=队伍(0友军，1敌军，2所有)   1=主类型  2=子类型  3模式(0模式单位， 1逻辑单位) 4=个数 5=百分比
    /// </summary>
    public class FinderFindHpLessThanPercent : FinderFindUnits
    {
        public override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 1;
        }

        protected float GetHpPercentArg()
        {
            return GetCurArg(5);
        }

        protected override bool IsHit(CombatUnit unit, CombatExtraData extraData)
        {
            return unit.IsAlive() && unit.GetHpPercent() <= GetHpPercentArg();
        }

        protected override List<CombatUnit> OnSortUnits(List<CombatUnit> units, CombatExtraData extraData)
        {
            return units;
        }
    }
}