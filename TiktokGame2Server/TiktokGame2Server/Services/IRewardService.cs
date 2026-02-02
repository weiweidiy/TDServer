using Game.Share;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IRewardService
    {
        Task<List<(ResourceType, string, int)>> AddReward(int playerId, string rewardBusinessId);
    }


}
