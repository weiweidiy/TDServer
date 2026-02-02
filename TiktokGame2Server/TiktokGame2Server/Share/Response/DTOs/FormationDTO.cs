namespace Game.Share
{
    public class FormationDTO
    {
        public required FormationType FormationType { get; set; } // 阵型类型 1: 攻阵 2：防阵
        public required string FormationBusinessId { get; set; } // 业务ID
    }
}