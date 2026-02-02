
using Game.Share;

namespace TiktokGame2Server.Others
{
    public interface IAchievement
    {
        /// <summary>
        /// 成就是否完成
        /// </summary>
        public bool IsCompleted(string playerUid, TiktokJCombatTurnBasedReportData reportData);
    }
}
