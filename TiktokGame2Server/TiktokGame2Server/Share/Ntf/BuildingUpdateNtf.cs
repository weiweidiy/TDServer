using JFramework;

namespace Game.Share
{
    public class BuildingUpdateNtf : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)ProtocolType.BuildingUpdateNtf; }
        public required BuildingDTO BuildingDTO { get; set; }
    }
}