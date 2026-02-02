namespace TiktokGame2Server.Entities
{
    public class BagItem
    {
        public int Id { get; set; }

        public required string ItemBusinessId { get; set; }

        public int Count { get; set; } = 1;

        public int BagSlotId { get; set; }
        public BagSlot? BagSlot { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }
    }
}
