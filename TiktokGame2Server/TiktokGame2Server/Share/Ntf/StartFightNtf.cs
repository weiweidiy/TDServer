using JFramework;

namespace Game.Share
{
    public class StartFightNtf : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)ProtocolType.StartFightNtf; }

        public ushort Port { get; set; }
    }
}