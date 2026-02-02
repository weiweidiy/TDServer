using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class PlayerService : IPlayerService
    {
        private readonly MyDbContext _dbContext;
        public PlayerService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Player?> GetPlayerAsync(string playerUid)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.Uid == playerUid);
        }
        public async Task<Player> CreatePlayerAsync(string playerUid, string name, int accountId)
        {
            var player = new Player
            {
                Uid = playerUid,
                Name = name,
                AccountId = accountId,
            };
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }

        public async Task<Player?> GetPlayerByAccountIdAsync(int accountId)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.AccountId == accountId);
        }
    }

    
}
