
using Game.Share;

namespace TiktokGame2Server.Others
{

    public class AchievementWin : AchievementBase
    {
        public AchievementWin(float[] args) : base(args)
        {
        }

        public override bool IsCompleted(string playerUid, TiktokJCombatTurnBasedReportData reportData)
        {
            return reportData.winnerTeamUid == playerUid;
        }
    }
}
