using System.Collections.Generic;

namespace JFramework
{
    public class CombatJFrameDebugTeamBuider : CombatTeamInfoBuilder
    {
        public override List<CombatUnitInfo> Build()
        {
            var result = new List<CombatUnitInfo>();


            var unitInfoBuilder = new CombatUnitInfoBuilder(new CombatJFrameActionArgSourceBuilder(), null);


            return result;
        }
    }

    public class CombatJFrameActionArgSourceBuilder : CombatActionArgSourceBuilder
    {
        public override Dictionary<int, CombatActionArgSource> Build()
        {
            throw new System.NotImplementedException();
        }
    }
}