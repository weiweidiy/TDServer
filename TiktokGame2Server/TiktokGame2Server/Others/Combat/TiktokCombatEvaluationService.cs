using Game.Share;

namespace TiktokGame2Server.Others
{
    public class TiktokCombatEvaluationService : IEvaluationService
    {
        public int GetEvaluation(string playerUid, TiktokJCombatTurnBasedReportData reportData)
        {
            var formation = reportData.FormationData[playerUid];
            float allCurHp = 0;
            float allMaxHp = 0;
            foreach (var unit in formation)
            {
                allCurHp += unit.CurHp;
                allMaxHp += unit.MaxHp;
            }

            switch (allCurHp / allMaxHp)
            {
                case > 0.8f:
                    return 3; // Excellent
                case > 0.5f:
                    return 2; // Good
                case > 0.2f:
                    return 1; // Fair
                default:
                    return 0;
            }
        }
    }
}
