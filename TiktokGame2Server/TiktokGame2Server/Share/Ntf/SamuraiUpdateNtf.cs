using JFramework;

namespace Game.Share
{
    public class SamuraiUpdateNtf : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)ProtocolType.SamuraiUpdateNtf; }
        public List<SamuraiDTO>? SamuraiDTOs { get; set; }
    }
}