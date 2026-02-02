using System.Collections.Generic;

namespace JFramework
{
    public class PVPBattleReport
    {
        public Dictionary<BattlePoint, BattleUnitInfo> attacker;
        public Dictionary<BattlePoint, BattleUnitInfo> defence;
        public List<IBattleReportData> report;
        public int winner;
    }

    
}