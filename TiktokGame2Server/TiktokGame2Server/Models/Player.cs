namespace TiktokGame2Server.Entities
{

    public class Player
    {
        public int Id { get; set; }
        public required string Uid { get; set; } 
        public string? Name { get; set; }         

        public ICollection<LevelNode>? LevelNodes { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; }
    }


}
