using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 找持有者 type 1  
    /// </summary>
    public class FinderFindSelf : CombatBaseFinder
    {
        public override List<CombatUnit> FindTargets(CombatExtraData extraData)
        {
            var result = new List<CombatUnit>();

            result.Add(extraData.Owner);

            return result;
        }

        public override int GetValidArgsCount()
        {
            return 0;
        }
    }
}