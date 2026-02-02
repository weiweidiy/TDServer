using Game.Share;

namespace TiktokGame2Server.Entities
{
    public class Formation
    {
        public int Id { get; set; }

        public FormationType FormationType { get; set; } // 1: 攻阵 2：防阵
        public required string FormationBusinessId { get; set; } // 阵型业务ID
        public int PlayerId { get; set; } // 外键，关联玩家
    }
}
