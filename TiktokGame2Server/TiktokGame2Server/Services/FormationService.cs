using Game.Share;
using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class FormationService : IFormationService
    {
        private readonly MyDbContext _dbContext;
        public FormationService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Formation?> AddFormationAsync(FormationType formationType, string formationBusinessId, int playerId)
        {
            // 检查是否已存在相同类型的阵型
            var existingFormation = _dbContext.Formations
                .FirstOrDefault(f => f.FormationType == formationType && f.PlayerId == playerId);
            if (existingFormation != null)
            {
                // 如果存在，则更新现有阵型
                existingFormation.FormationBusinessId = formationBusinessId;
                return await UpdateFormationAsync(existingFormation);
            }
            else
            {
                // 如果不存在，则创建新的阵型
                var newFormation = new Formation
                {
                    FormationType = formationType,
                    FormationBusinessId = formationBusinessId,
                    PlayerId = playerId
                };
                _dbContext.Formations.Add(newFormation);
                return await _dbContext.SaveChangesAsync().ContinueWith(t => newFormation);
            }

        }

        public async Task<Formation?> GetFormationAsync(FormationType formationType, int playerId)
        {
            // 查找指定玩家的阵型
            var formation = await _dbContext.Formations
                .Where(f => f.FormationType == formationType && f.PlayerId == playerId)
                .FirstOrDefaultAsync();

            return formation;
        }

        public Task<Formation> UpdateFormationAsync(Formation formationData)
        {
            // 更新或添加阵型数据
            _dbContext.Formations.Update(formationData);
            return _dbContext.SaveChangesAsync().ContinueWith(t => formationData);
        }
    }
}


