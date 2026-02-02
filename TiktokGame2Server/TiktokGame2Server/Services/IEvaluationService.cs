using Game.Share;

namespace TiktokGame2Server.Others
{
    public interface IEvaluationService
    {
        int GetEvaluation(string playerUid, TiktokJCombatTurnBasedReportData reportData);
    }
}
