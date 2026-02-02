using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 8 参数 參數0：actionGroupId 参数1：sortID 
    /// </summary>
    public class FinderFindSelfActions : FinderFindSelf
    {
        public override int GetValidArgsCount()
        {
            return 2;
        }

        protected int GetGroupIdArg()
        {
            return (int)GetCurArg(0);
        }

        protected int GetSortIdArg()
        {
            return (int)GetCurArg(1);
        }

        public override List<CombatUnit> FindTargets(CombatExtraData extraData)
        {
            var result = new List<CombatUnit>();

            result.Add(extraData.Owner);


            foreach (var action in extraData.Owner.GetActions())
            {

                if (action.GroupId != GetGroupIdArg() && GetGroupIdArg() != 0)
                    continue;

                if (action.SortId != GetSortIdArg() && GetSortIdArg() != 0)
                    continue;

                if (!extraData.TargetActions.Contains(action))
                    extraData.TargetActions.Add(action);
            }

            return result;
        }

        //protected override bool IsHit(CombatUnit unit, CombatExtraData extraData)
        //{
        //    bool isAlive = unit.IsAlive();
        //    if (isAlive)
        //    {
        //        foreach (var action in unit.GetActions())
        //        {

        //            if (action.GroupId != GetGroupIdArg() && GetGroupIdArg() != 0)
        //                continue;

        //            if (action.SortId != GetSortIdArg() && GetSortIdArg() != 0)
        //                continue;

        //            if (!extraData.TargetActions.Contains(action))
        //                extraData.TargetActions.Add(action);
        //        }
        //    }
        //    return isAlive;
        //}
    }
}