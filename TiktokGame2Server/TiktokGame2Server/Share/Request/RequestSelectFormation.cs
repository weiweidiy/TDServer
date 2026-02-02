namespace Game.Share
{
    public class RequestSelectFormation
    {
        public FormationType FormationType { get; set; } // 阵型类型
        public required string FormationBusinessId { get; set; } // 阵型业务ID
    }
}