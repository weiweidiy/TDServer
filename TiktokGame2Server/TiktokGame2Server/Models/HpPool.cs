namespace TiktokGame2Server.Entities
{
    /// <summary>
    /// 生命池，用于给samurai补充生命值
    /// </summary>
    public class HpPool
    {
        public int Id { get; set; }

        public int Hp { get; set; } // 生命值

        public int MaxHp { get; set; }

        public int PlayerId { get; set; } // 外键，关联玩家
        public Player? Player { get; set; }
    }
}
