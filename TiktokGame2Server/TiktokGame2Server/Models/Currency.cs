using Game.Share;

namespace TiktokGame2Server.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        public CurrencyType CurrencyType { get; set; }

        public int Count { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

    }
}
