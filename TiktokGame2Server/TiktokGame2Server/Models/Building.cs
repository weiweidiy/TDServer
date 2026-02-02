namespace TiktokGame2Server.Entities
{
    public class Building
    {
        public int Id { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public required string BusinessId { get; set; }
        public int Level { get; set; }
        /// <summary>
        /// 上次升级开始时间
        /// </summary>
        public DateTime CreateAt { get; set; }

        public int PlayerId { get; set; }
    }

}
