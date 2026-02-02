using Game.Share;
using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class FormationDeployService : IFormationDeployService
    {
        private readonly MyDbContext _dbContext;
        ISamuraiService samuraiService;
        public FormationDeployService(MyDbContext dbContext, ISamuraiService samuraiService)
        {
            _dbContext = dbContext;
            this.samuraiService = samuraiService;
        }
       

        /// <summary>
        /// 添加一个阵型点位数据
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="formationPoint"></param>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public async Task<FormationDeploy> AddOrUpdateFormationSamuraiAsync(FormationType formationType, int formationPoint, int samuraiId, int playerId)
        {
            //先查询是否存在相同的阵型和位置
            var existingFormation = await _dbContext.FormationDeploy
                .FirstOrDefaultAsync(f => f.FormationType == formationType && f.FormationPoint == formationPoint /*&& f.SamuraiId == targetSamuraiId*/ && f.PlayerId == playerId);

            //如果存在，则修改对应的samuraiId, 替换武将
            if (existingFormation != null)
            {
                existingFormation.SamuraiId = samuraiId;
                _dbContext.FormationDeploy.Update(existingFormation);
                await _dbContext.SaveChangesAsync();
                return existingFormation;
            }
            else
            //如果不存在，则添加新的上阵武将
            {
                var formation = new FormationDeploy
                {
                    FormationType = formationType,
                    FormationPoint = formationPoint,
                    SamuraiId = samuraiId,
                    PlayerId = playerId
                };
                _dbContext.FormationDeploy.Add(formation);
                await _dbContext.SaveChangesAsync();
                return formation;
            }
        }

        public async Task<bool> DeleteFormationSamuraiAsync(FormationType formationType, int formationPoint, int playerId)
        {
            var formation = await _dbContext.FormationDeploy
                .FirstOrDefaultAsync(f => f.FormationType == formationType && f.FormationPoint == formationPoint && f.PlayerId == playerId);

            if (formation != null)
            {
                var result = _dbContext.FormationDeploy.Remove(formation);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public Task<bool> DeleteFormationAllSamuraiAsync(FormationType formationType, int playerId)
        {
                       // 删除指定玩家指定阵型类型的所有武将
            var formations = _dbContext.FormationDeploy
                .Where(f => f.FormationType == formationType && f.PlayerId == playerId);
            if (formations.Any())
            {
                _dbContext.FormationDeploy.RemoveRange(formations);
                return _dbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            return Task.FromResult(false);
        }

        public async Task DeleteFormationAsync(List<FormationDeploy> formationDataToDelete)
        {
            if (formationDataToDelete == null || formationDataToDelete.Count == 0)
            {
                return; // 如果没有要删除的数据，直接返回
            }
            // 批量删除
            _dbContext.FormationDeploy.RemoveRange(formationDataToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFormationAsync(FormationDeploy existingFormation)
        {
            if (existingFormation == null)
            {
                throw new ArgumentNullException(nameof(existingFormation), "Existing formation cannot be null");
            }
            // 更新阵型数据
            _dbContext.FormationDeploy.Update(existingFormation);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 更新一个阵型的数据
        /// </summary>
        /// <param name="newFormations"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<FormationDeploy>> UpdateFormationDeployAsync(FormationType formationType, List<FormationDeployDTO> newFormations, int playerId)
        {
            //to do:检查formationDTO中的samuraiId是否有重复，数据库是否拥有



            // 如果没有要更新的数据，返回当前数据库中的数据
            if (newFormations == null || newFormations.Count == 0)
            {
                return await _dbContext.FormationDeploy
                    .Where(f => f.FormationType == formationType && f.PlayerId == playerId)
                    .ToListAsync();
            }

            //检查阵型点位和武将ID是否合法
            foreach (var formationDTO in newFormations)
            {
                if (formationDTO.FormationPoint < 0 || formationDTO.SamuraiId <= 0)
                {
                    throw new ArgumentException("Invalid formation point or samurai ID in the provided data.");
                }
            }


            // 首先删除所有旧的指定类型的阵型数据
            var existingFormations = await _dbContext.FormationDeploy
                .Where(f => f.FormationType == formationType && f.PlayerId == playerId)
                .ToListAsync();
            //从数据库中删除旧的阵型数据existingFormations
            if (existingFormations.Count > 0)
            {
                _dbContext.FormationDeploy.RemoveRange(existingFormations);
            }


            // 遍历新的阵型数据，进行添加 
            var addedFormations = new List<FormationDeploy>();
            foreach (var formationDTO in newFormations)
            {
                //var targetSamuraiId = await samuraiService.QuerySamuraiId(formationDTO.SamuraiId, playerId);
                //从数据库中查询该玩家是否有该武将
                var samurai = await samuraiService.GetSamuraiAsync(formationDTO.SamuraiId);
                if (samurai == null)
                    continue;

                var newFormation = new FormationDeploy
                {
                    FormationType = formationDTO.FormationType,
                    FormationPoint = formationDTO.FormationPoint,
                    SamuraiId = samurai.Id, 
                    PlayerId = playerId
                };
                _dbContext.FormationDeploy.Add(newFormation);
                addedFormations.Add(newFormation);
            }

            await _dbContext.SaveChangesAsync();
            return addedFormations;
        }


        public async Task<List<FormationDeploy>?> UpdateFormationDeployAsync(FormationType formationType, int targetPoint, int targetSamuraiId, int playerId)
        {
            //case1: 如果samuraiId小于等于0，表示移除该点位的武将
            if (targetSamuraiId <= 0)
            {
                await DeleteFormationSamuraiAsync(formationType, targetPoint, playerId);
                return await GetFormationDeployAsync(formationType, playerId) ?? new List<FormationDeploy>();
            }


            //如果samuraiId大于0，检查该武将是否属于该玩家
            var samurai = await samuraiService.GetSamuraiAsync(targetSamuraiId);
            if (samurai == null)
            {
                //该武将不存在，返回null表示失败
                return null;
            }

            if (samurai.PlayerId != playerId)
            {
                //该武将不属于该玩家，返回null表示失败
                return null;
            }

            //检查该武将是否已经在阵型中
            var originalPoint = await GetFormationPoint(formationType, targetSamuraiId);
            //检查目标点位是否已经有武将
            var existingSamuraiId = await GetFormationSamuraiIdAsync(formationType, targetPoint, playerId);

            //case2: 交换武将位置（2个都在阵型中）
            if (originalPoint >= 0 && existingSamuraiId >= 0)
            {
                //如果该武将已经在阵型中，并且目标点位也有武将，则交换位置
                var formation1 = await _dbContext.FormationDeploy
                    .FirstOrDefaultAsync(f => f.FormationType == formationType && f.FormationPoint == originalPoint && f.PlayerId == playerId);
                var formation2 = await _dbContext.FormationDeploy
                    .FirstOrDefaultAsync(f => f.FormationType == formationType && f.FormationPoint == targetPoint && f.PlayerId == playerId);
                if (formation1 != null && formation2 != null)
                {
                    var tempSamuraiId = formation1.SamuraiId;
                    formation1.SamuraiId = formation2.SamuraiId;
                    formation2.SamuraiId = tempSamuraiId;
                    _dbContext.FormationDeploy.Update(formation1);
                    _dbContext.FormationDeploy.Update(formation2);
                    await _dbContext.SaveChangesAsync();
                }
                return await GetFormationDeployAsync(formationType, playerId) ?? new List<FormationDeploy>();
            }

            //case3: 阵型中位置变更
            if (originalPoint >= 0 && existingSamuraiId < 0)
            {
                //删除原来的位置
                var del = await DeleteFormationSamuraiAsync(formationType, originalPoint,playerId);
                if (!del)
                    return null;

                //添加新的位置
                var result = await AddOrUpdateFormationSamuraiAsync(formationType, targetPoint, targetSamuraiId, playerId);
                return await GetFormationDeployAsync(formationType, playerId) ?? new List<FormationDeploy>();
            }


            //case4: 新上阵（1个在阵型中，1个不在阵型中；或者2个都不在阵型中）
            if (originalPoint < 0)
            {
                var result = await AddOrUpdateFormationSamuraiAsync(formationType, targetPoint, targetSamuraiId, playerId);
                return await GetFormationDeployAsync(formationType, playerId) ?? new List<FormationDeploy>();
            }

            return null;
        }


        public Task<List<int>> GetFormationSamuraiIdsAsync(int playerId)
        {
            // 获取指定玩家的所有阵型中的武将ID
            return _dbContext.FormationDeploy
                .Where(f => f.PlayerId == playerId)
                .Select(f => f.SamuraiId)
                .Distinct()
                .ToListAsync();

        }


        public Task<int> GetFormationSamuraiIdAsync(FormationType formationType, int point, int playerId)
        {
            // 获取指定阵型类型和点位的武将ID
            var data = _dbContext.FormationDeploy
                .Where(f => f.FormationType == formationType && f.FormationPoint == point && f.PlayerId == playerId).ToList();

            if(data.Count == 0)
                return Task.FromResult(-1);
            else
                return Task.FromResult(data[0].SamuraiId);
        }

        public Task<List<int>> GetFormationSamuraiIdsAsync(FormationType formationType, int playerId)
        {
            // 获取指定阵型类型和玩家的所有武将ID
            return _dbContext.FormationDeploy
                .Where(f => f.FormationType == formationType && f.PlayerId == playerId)
                .Select(f => f.SamuraiId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<FormationDeploy>?> GetFormationDeployAsync(FormationType formationType, int playerId)
        {
            // 查找指定玩家的阵型
            var formations = await _dbContext.FormationDeploy
                .Where(f => f.FormationType == formationType && f.PlayerId == playerId)
                .Include(f => f.Samurai)
                .ToListAsync();
            return formations;


        }

        /// <summary>
        /// 根据阵型类型和武士ID获取阵型点位
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public async Task<int> GetFormationPoint(FormationType formationType, int samuraiId)
        {
            // 查找指定阵型类型和武士ID的阵型点位
            var formation = await _dbContext.FormationDeploy
                .FirstOrDefaultAsync(f => f.FormationType == formationType && f.SamuraiId == samuraiId);
            if (formation != null)
            {
                return formation.FormationPoint;
            }
            return -1; // 如果没有找到，返回-1表示未设置点位
        }

        public Task<bool> IsSamuraiInFormationAsync(int samuraiId, int playerId, FormationType formationType)
        {
            // 检查指定武将是否在指定玩家的阵型中
            return _dbContext.FormationDeploy
                .AnyAsync(f => f.SamuraiId == samuraiId && f.PlayerId == playerId && f.FormationType == formationType);

        }

        public Task<bool> IsSamuraiInFormationPointAsync(int samuraiId, int playerId, FormationType formationType, int point)
        {
            // 检查指定武将是否在指定玩家的阵型点位中
            return _dbContext.FormationDeploy
                .AnyAsync(f => f.SamuraiId == samuraiId && f.PlayerId == playerId && f.FormationType == formationType && f.FormationPoint == point);
        }
    }
}


