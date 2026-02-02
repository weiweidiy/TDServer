namespace Game.Share
{
    public class FormationDeployDTO
    {
        public required int Id { get; set; }
        public FormationType FormationType { get; set; } // 1: 攻阵 2：防阵

        public int FormationPoint { get; set; }

        public int SamuraiId { get; set; }
    }
}