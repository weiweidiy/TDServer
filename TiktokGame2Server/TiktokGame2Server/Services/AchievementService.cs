
using Game.Share;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class AchievementService : IAchievementService
    {
        TiktokConfigManager tiktokConfigService;
        public AchievementService(TiktokConfigManager tiktokConfigService)
        {
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }

        public bool IsAchievementCompleted(string playerUid, TiktokJCombatTurnBasedReportData reportData, string achievementBusinessId)
        {
            //to do:根据星数，获取达成条件对象
            var achievement = CreateAchievement(achievementBusinessId);
            return achievement.IsCompleted(playerUid, reportData);
        }

        private IAchievement CreateAchievement(string achievementBusinessId)
        {
            var achievementName = tiktokConfigService.GetAchievementClassName(achievementBusinessId);
            var args = tiktokConfigService.GetAchievementArgs(achievementBusinessId);
            object[] ctorArgs = new object[] { args };
            return (IAchievement)TypeHelper.CreateInstanceByClassName(achievementName, ctorArgs);
        }
    }
}
