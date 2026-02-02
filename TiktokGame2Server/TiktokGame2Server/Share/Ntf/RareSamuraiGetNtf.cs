using JFramework;

namespace Game.Share
{
    public class RareSamuraiGetNtf : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)ProtocolType.RareSamuraiGetNtf; }
        public required string PlayerName { get; set; }
        public required SamuraiDTO SamuraiDTO { get; set; }
    }
}