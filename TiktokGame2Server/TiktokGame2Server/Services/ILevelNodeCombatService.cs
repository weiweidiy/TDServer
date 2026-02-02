using Game.Share;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public interface ILevelNodeCombatService
    {
        Task<TiktokJCombatTurnBasedReportData> GetReport(int playerId, string levelNodeBusinessId);
    }
}
