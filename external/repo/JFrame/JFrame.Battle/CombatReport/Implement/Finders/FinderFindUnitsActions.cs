using System.Reactive;

namespace JFramework
{
    /// <summary>
    /// type 7   参数：0=队伍(0友军，1敌军，2所有, 3不包含自己的友军)   1=主类型  2=子类型  3模式(0模式单位， 1逻辑单位) 4=个数  , 參數5：actionGroupId 参数6：sortID 
    /// </summary>
    public class FinderFindUnitsActions : FinderFindUnits
    {
        public override int GetValidArgsCount()
        {
            return 7;
        }

        protected int GetGroupIdArg()
        {
            return (int)GetCurArg(5);
        }

        protected int GetSortIdArg()
        {
            return (int)GetCurArg(6);
        }


        protected override bool IsHit(CombatUnit unit, CombatExtraData extraData)
        {
            bool isAlive = unit.IsAlive();
            if (isAlive)
            {
                foreach (var action in unit.GetActions())
                {

                    if (action.GroupId != GetGroupIdArg() && GetGroupIdArg() != 0)
                        continue;

                    if (action.SortId != GetSortIdArg() && GetSortIdArg() != 0)
                        continue;

                    if (!extraData.TargetActions.Contains(action))
                        extraData.TargetActions.Add(action);
                }
            }
            return isAlive;
        }


    }
}