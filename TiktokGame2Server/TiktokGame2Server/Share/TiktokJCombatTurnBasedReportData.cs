using JFramework.Game;

namespace Game.Share
{
    public class TiktokJCombatTurnBasedReportData : JCombatTurnBasedReportData<TiktokJCombatUnitData>
    {
        public string? CombatSceneBusinessId { get; set; } // 可能需要在其他地方使用
        public int Evaluation { get; set; }
    }
}


