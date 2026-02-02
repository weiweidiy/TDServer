using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class BuildingService : IBuildingService
    {
        private readonly MyDbContext _dbContext;
        public BuildingService(MyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<List<Building>> GetAllBuildingsAsync(int playerId)
        {
            return await _dbContext.Buildings
                .Where(b => b.PlayerId == playerId)
                .ToListAsync();
        }
        public async Task<Building?> GetBuildingAsync(int buildingId, int playerId)
        {
            return await _dbContext.Buildings
                .FirstOrDefaultAsync(b => b.Id == buildingId && b.PlayerId == playerId);
        }

        public Task<Building?> GetBuildingAsync(string buildingBusinessId, int playerId)
        {
            return _dbContext.Buildings
                .FirstOrDefaultAsync(b => b.BusinessId == buildingBusinessId && b.PlayerId == playerId);
        }


        public Task<BuildingUpgradSchedule?> GetBuildingUpgradeScheduleAsync(int buildingId, int playerId)
        {
            return _dbContext.BuildingUpgradSchedules
                .FirstOrDefaultAsync(s => s.BuildingId == buildingId && s.PlayerId == playerId);
        }


        public async Task<Building> AddBuildingAsync(string buildingBusinessId, int playerId)
        {
            var newBuilding = new Building
            {
                BusinessId = buildingBusinessId,
                Level = 1,
                PlayerId = playerId,
                CreateAt = DateTime.UtcNow
            };
            _dbContext.Buildings.Add(newBuilding);
            await _dbContext.SaveChangesAsync();
            return newBuilding;
        }
        public async Task<List<Building>> AddBuildingsAsync(List<string> buildingBusinessIds, int playerId)
        {
            var newBuildings = buildingBusinessIds.Select(id => new Building
            {
                BusinessId = id,
                PlayerId = playerId,
                Level = 1,
                CreateAt = DateTime.UtcNow
                //CreatedAt = DateTime.UtcNow,
                //UpdatedAt = DateTime.UtcNow
            }).ToList();
            _dbContext.Buildings.AddRange(newBuildings);
            await _dbContext.SaveChangesAsync();
            return newBuildings;
        }
        public async Task<bool> RemoveBuildingAsync(int buildingId, int playerId)
        {
            var building = await _dbContext.Buildings
                .FirstOrDefaultAsync(b => b.Id == buildingId && b.PlayerId == playerId);
            if (building == null)
                return false;
            _dbContext.Buildings.Remove(building);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task<bool> RemoveBuildingsAsync(List<int> buildingIds, int playerId)
        {         
            var buildings = _dbContext.Buildings
                .Where(b => buildingIds.Contains(b.Id) && b.PlayerId == playerId)
                .ToList();
            if (buildings.Count == 0)
                return Task.FromResult(false);
            _dbContext.Buildings.RemoveRange(buildings);
            return _dbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }


        public async Task<BuildingUpgradSchedule> BeginUpgrade(string buildingBusinessId, int playerId, TimeSpan upgradeDuration)
        {
            var building = await GetBuildingAsync(buildingBusinessId, playerId);
            if (building == null)
                throw new InvalidOperationException("Building not found");

            var existingSchedule = _dbContext.BuildingUpgradSchedules
                .FirstOrDefault(s => s.BuildingId == building.Id && s.PlayerId == playerId);
            if (existingSchedule != null)
                throw new InvalidOperationException("Upgrade already in progress for this building");

            var newSchedule = new BuildingUpgradSchedule
            {
                BuildingId = building.Id,
                PlayerId = playerId,
                UpgradeBeginAt = DateTime.UtcNow,
                UpgradeEndAt = DateTime.UtcNow.Add(upgradeDuration)
            };
            _dbContext.BuildingUpgradSchedules.Add(newSchedule);
            await _dbContext.SaveChangesAsync();
            return newSchedule;
        }

        /// <summary>
        /// 尝试完成建筑升级
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Task<bool> TryToCompleteUpgrade(MyDbContext db, int buildingId, int playerId)
        {
            var building = db.Buildings
                .FirstOrDefault(b => b.Id == buildingId && b.PlayerId == playerId);
            if (building == null)
                return Task.FromResult(false);
            var schedule = db.BuildingUpgradSchedules
                .FirstOrDefault(s => s.BuildingId == buildingId && s.PlayerId == playerId);

            //检查是否存在升级计划且升级时间已到
            if (schedule == null || schedule.UpgradeEndAt > DateTime.UtcNow)
                return Task.FromResult(false);

            //升级建筑
            building.Level += 1;
            db.Update(building);
            db.BuildingUpgradSchedules.Remove(schedule);
            return db.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        /// <summary>
        /// 理解完成建筑升级（不检查时间）
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Task<bool> CompleteUpdateImmediately(int buildingId, int playerId)
        {
            var building = _dbContext.Buildings
                .FirstOrDefault(b => b.Id == buildingId && b.PlayerId == playerId);
            if (building == null)
                return Task.FromResult(false);

            var schedule = _dbContext.BuildingUpgradSchedules
                .FirstOrDefault(s => s.BuildingId == buildingId && s.PlayerId == playerId);
            //检查是否存在升级计划
            if (schedule == null)
                return Task.FromResult(false);

            //升级建筑
            building.Level += 1;
            _dbContext.Update(building);
            _dbContext.BuildingUpgradSchedules.Remove(schedule);
            return _dbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);

        }
    }
}
