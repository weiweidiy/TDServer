namespace Game.Share
{
    public class SamuraiDTO
    {
        public required int Id { get; set; }
        public required string BusinessId { get; set; }

        public required string SoldierBusinessId { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;      
        
        public int CurHp { get; set; }
    }
}