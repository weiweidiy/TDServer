using System.Collections.Generic;

namespace JFramework
{
    public interface ICombatFinder
    {
        List<CombatUnit> FindTargets(CombatExtraData extraData);
    }
}