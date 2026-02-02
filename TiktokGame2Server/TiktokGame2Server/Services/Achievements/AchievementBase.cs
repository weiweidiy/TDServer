
using Game.Share;

namespace TiktokGame2Server.Others
{
    public abstract class AchievementBase : IAchievement
    {
        float[] args;
        public AchievementBase(float[] args)
        {
            this.args = args;
        }

        protected float GetArg(int index) => args[index];

        public abstract bool IsCompleted(string playerUid, TiktokJCombatTurnBasedReportData reportData);
    }
}
