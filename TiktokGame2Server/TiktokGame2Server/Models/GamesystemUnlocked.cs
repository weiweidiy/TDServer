namespace TiktokGame2Server.Entities
{
    public class GamesystemUnlocked
    {
        public int Id { get; set; }

        /// <summary>
        /// 已解锁的系统业务ID
        /// </summary>
        public string? UnlockedSystemBusinessId { get; set; }

        public int PlayerId { get; set; } // 外键，关联玩家
    }
}
