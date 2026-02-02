using Game.Share;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IFormationDeployService
    {
        /// <summary>
        /// 更新指定玩家的阵型
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="formation"></param>
        /// <returns></returns>
        Task<FormationDeploy> AddOrUpdateFormationSamuraiAsync(FormationType formationType, int formationPoint, int samuraiId, int playerId);

        /// <summary>
        /// 删除一个点位的配置
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="formationPoint"></param>
        /// <returns></returns>
        Task<bool> DeleteFormationSamuraiAsync(FormationType formationType, int formationPoint, int playerId);

        /// <summary>
        /// 删除指定玩家指定阵型类型的所有武将
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<bool> DeleteFormationAllSamuraiAsync(FormationType formationType, int playerId);

        Task DeleteFormationAsync(List<FormationDeploy> formationDataToDelete);


        Task UpdateFormationAsync(FormationDeploy existingFormation);

        Task<List<FormationDeploy>> UpdateFormationDeployAsync(FormationType formationType, List<FormationDeployDTO> newFormations, int playerId);

        Task<List<FormationDeploy>?> UpdateFormationDeployAsync(FormationType formationType, int point, int samuraiId, int playerId);

        /// <summary>
        /// 获取指定玩家的阵型
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<List<FormationDeploy>?> GetFormationDeployAsync(FormationType formationType, int playerId);
        Task<int> GetFormationPoint(FormationType formationType, int samuraiId);
        Task<List<int>> GetFormationSamuraiIdsAsync(int playerId);
        Task<List<int>> GetFormationSamuraiIdsAsync(FormationType formationType, int playerId);

        Task<int> GetFormationSamuraiIdAsync(FormationType formationType, int point, int playerId);
        Task<bool> IsSamuraiInFormationAsync(int samuraiId, int playerId, FormationType formationType);
        Task<bool> IsSamuraiInFormationPointAsync(int samuraiId, int playerId, FormationType formationType, int point);
    }
}
