namespace Game.Share
{
    public class RequestChangeFormation
    {
        public FormationType FormationType { get; set; }
        public required string FormationBusinessId { get; set; }
    }
}