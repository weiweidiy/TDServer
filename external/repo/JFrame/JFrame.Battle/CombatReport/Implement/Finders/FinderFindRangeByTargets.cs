using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type = 5   参数：0=队伍(0友军，1敌军，2所有)   1=主类型  2=子类型  3模式(0模式单位， 1逻辑单位) 4=个数  5:宽度
    /// </summary>
    public class FinderFindRangeByTargets : FinderFindUnits
    {
        public override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 1;
        }

        protected float GetWidthArg()
        {
            return GetCurArg(5);
        }

        public override List<CombatUnit> FindTargets(CombatExtraData extraData)
        {
            if (extraData.Targets == null || extraData.Targets.Count == 0)
                return extraData.Targets;

            var targetTeamId = context.CombatManager.GetFriendTeamId(extraData.Targets[0]);
            var units = context.CombatManager.GetUnits(targetTeamId, GetFindModeArg());
            return FiltUnitType(units, extraData);

        }

        protected override bool IsHit(CombatUnit unit, CombatExtraData extraData)
        {
            var targetX = extraData.Targets[0].GetPosition().x;

            var x = unit.GetPosition().x;
            var widthX = GetWidthArg();

            if (x < targetX - widthX / 2 || x > targetX + widthX / 2)
                return false;

            return true;


        }
    }
}