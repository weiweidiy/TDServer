using JFramework.Game;

namespace Game.Share
{
    public class TiktokJCombatUnitData : IJCombatUnitData
    {
        public required string Uid { get; set; }
        public int Seat { get; set; }
        public required string SamuraiBusinessId { get; set; }
        public required string SoldierBusinessId { get; set; } // 可能需要在其他地方使用
        public int CurHp { get; set; }
        public int MaxHp { get; set; }
        public List<KeyValuePair<string, string>>? Actions { get; set; } 
    }
}


