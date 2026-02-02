namespace TiktokGame2Server.Entities
{
    public class BuildingUpgradSchedule
    {
        public int Id { get; set; }
        public required int BuildingId { get; set; }
        public required DateTime UpgradeBeginAt { get; set; }
        public required DateTime UpgradeEndAt { get; set; }
        public required int PlayerId { get; set; }
    }

}
