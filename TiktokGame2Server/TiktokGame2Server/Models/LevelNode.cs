namespace TiktokGame2Server.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class LevelNode
    {
        public int Id { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public required string BusinessId { get; set; }

        public int Process { get; set; }

        public int PlayerId { get; set; }

    }

}
