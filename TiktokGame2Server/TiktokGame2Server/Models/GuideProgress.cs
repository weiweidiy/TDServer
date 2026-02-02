namespace TiktokGame2Server.Entities
{
    /// <summary>
    /// 引导进度
    /// </summary>
    public class GuideProgress
    {
        public int Id { get; set; }
        /// <summary>
        /// 引导业务ID
        /// </summary>
        public required string GuideBusinessId { get; set; }
        public int PlayerId { get; set; } // 外键，关联玩家
    }
}
