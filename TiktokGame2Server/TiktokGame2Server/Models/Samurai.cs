namespace TiktokGame2Server.Entities
{
    public class Samurai
    {
        public int Id { get; set; }

        public required string BusinessId { get; set; }


        public int Level { get; set; } = 1; //计算值，通过经验值计算

        public int Experience { get; set; } = 0;

        /// <summary>
        /// 当前生命值
        /// </summary>
        public int CurHp { get; set; }

        public required string SoldierBusinessId { get; set; } 

        /// <summary>
        /// 外键，关联玩家
        /// </summary>·
        public int PlayerId { get; set; }

        public Player? Player { get; set; } 
    }
}
