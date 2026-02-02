
using Game.Share;

namespace TiktokGame2Server.Others
{
    public interface IAchievementService
    {
        /// <summary>
        /// 根据当前挑战的星数，从战报中判断成就是否完成
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="star"></param>
        /// <returns></returns>
        public bool IsAchievementCompleted(string playerUid, TiktokJCombatTurnBasedReportData reportData, string achievementBusinessId);
    }
}
