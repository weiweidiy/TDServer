using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IPlayerService
    {
        Task<Player?> GetPlayerAsync(string playerUid);
        Task<Player> CreatePlayerAsync(string playerUid, string name, int accountId);
        Task<Player?> GetPlayerByAccountIdAsync(int id);
    }
}
