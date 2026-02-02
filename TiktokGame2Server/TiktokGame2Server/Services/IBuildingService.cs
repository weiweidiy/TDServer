using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IBuildingService
    {
        Task<Building> AddBuildingAsync(string buildingBusinessId, int playerId);
        Task<List<Building>> AddBuildingsAsync(List<string> buildingBusinessIds, int playerId);

        Task<bool> RemoveBuildingAsync(int buildingId, int playerId);
        Task<bool> RemoveBuildingsAsync(List<int> buildingIds, int playerId);

        Task<BuildingUpgradSchedule> BeginUpgrade(string buildingBusinessId, int playerId, TimeSpan upgradeDuration);
        Task<bool> TryToCompleteUpgrade(MyDbContext db, int buildingId, int playerId);
        Task<bool> CompleteUpdateImmediately(int buildingId, int playerId);


        Task<List<Building>> GetAllBuildingsAsync(int playerId);
        Task<Building?> GetBuildingAsync(int buildingId, int playerId);
        Task<Building?> GetBuildingAsync(string buildingBusinessId, int playerId);
        Task<BuildingUpgradSchedule?> GetBuildingUpgradeScheduleAsync(int buildingId, int playerId);

        //Task<Building> AddOrUpgradeBuildingAsync(string buildingBusinessId, int playerId);


    }
}
