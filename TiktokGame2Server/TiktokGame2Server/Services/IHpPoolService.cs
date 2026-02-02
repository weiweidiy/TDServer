using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IHpPoolService
    {
        Task<HpPool> GetHpPoolAsync(int playerId);
        Task<int> GetHpPoolCurHpAsync(int playerId);
        Task<HpPool> AddHpPoolAsync(int playerId, int amount);
        Task<HpPool> SubtractHpPoolAsync(int playerId, int amount);
    }
}
