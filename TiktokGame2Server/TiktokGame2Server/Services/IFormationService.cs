using Game.Share;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IFormationService
    {
        Task<Formation?> AddFormationAsync(FormationType formationType, string formationBusinessId, int playerId);

        /// <summary>
        /// 获取指定玩家的阵型
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<Formation?> GetFormationAsync(FormationType formationType, int playerId);
        Task<Formation> UpdateFormationAsync(Formation formationData);
    }
}


