namespace Game.Share
{

    public class ResponseFight 
    {
        public required string LevelNodeBusinessId { get; set; }

        public TiktokJCombatTurnBasedReportData? ReportData { get; set; }

        public  LevelNodeDTO? LevelNodeDTO { get; set; }

        /// <summary>
        /// 战斗后更新的武士信息列表
        /// </summary>
        public List<SamuraiDTO>? SamuraiDTOs { get; set; } 

        public HpPoolDTO? HpPoolDTO { get; set; }

        public RewardDTO? WinRewardDTO { get; set; }

        public RewardDTO? AchievementRewardDTO { get; set; }

        public List<CurrencyDTO>? Currencies { get; set; }
    }
}