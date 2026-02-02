namespace Game.Share
{
    public class BuildingDTO
    {
        //public required int Id { get; set; }
        public required string BusinessId { get; set; }
        public int Level { get; set; }
        public DateTime? UpgradeEndTime { get; set; }
    }
}