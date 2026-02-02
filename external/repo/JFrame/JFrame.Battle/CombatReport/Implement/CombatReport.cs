using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    public class CombatReport
    {

        public KeyValuePair<CombatTeamType, List<CombatUnitInfo>> attacker;
        public KeyValuePair<CombatTeamType, List<CombatUnitInfo>> defence;
        public List<ICombatReportData> report;
        public Dictionary<string, long> damageStatistics = new Dictionary<string, long>();
        public int winner;
        public float deltaTime; //每帧的时间

        public long GetTotalDamage(int teamId)
        {
            long totalDamage = 0;
            foreach (var unitUid in damageStatistics.Keys)
            {
                var dmg = damageStatistics[unitUid];
                var team = GetUnitTeamId(unitUid);
                if (teamId == team)
                    totalDamage += dmg;
            }

            return totalDamage;
        }

        int GetUnitTeamId(string unitUid)
        {
            var unitInfo = attacker.Value.Where(info => info.uid == unitUid).SingleOrDefault();
            if (unitInfo != null)
                return 0;

            unitInfo = defence.Value.Where(info => info.uid == unitUid).SingleOrDefault();
            if (unitInfo != null)
                return 1;

            return -1;
        }
    }
}