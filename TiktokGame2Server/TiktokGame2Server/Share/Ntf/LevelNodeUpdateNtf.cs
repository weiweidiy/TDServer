using JFramework;

namespace Game.Share
{
    public class LevelNodeUpdateNtf : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)ProtocolType.LevelNodeUpdateNtf; }
        public required LevelNodeDTO LevelNodeDTO { get; set; }
    }
}