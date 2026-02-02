namespace Game.Share
{
    public class ResponseGame
    {
        public required PlayerDTO PlayerDTO { get; set; }
        public List<LevelNodeDTO>? LevelNodesDTO { get; set; }

        public List<SamuraiDTO>? SamuraisDTO { get; set; } = new List<SamuraiDTO>();

        public List<FormationDeployDTO>? AtkFormationDTO { get; set; } = new List<FormationDeployDTO>();

        //public List<FormationDeployDTO>? DefFormationDTO { get; set; } = new List<FormationDeployDTO>();

        public HpPoolDTO? HpPoolDTO { get; set; }

        public List<CurrencyDTO>? CurrencyDTO { get; set; }

        public List<BagSlotDTO>? BagDTOs { get; set; }

        public List<FormationDTO>? FormationDTOs { get; set; }

        public List<BuildingDTO>? BuildingDTOs { get; set; }

        public string CurrentGuideBusinessId { get; set; } = string.Empty;

        public required long ServerTime { get; set; }
    }
}