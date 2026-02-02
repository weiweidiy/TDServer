using JFramework;

namespace Game.Share
{
    public class FormationDeployUpdateNtf : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)ProtocolType.FormationDeployUpdateNtf; }
        public required List<FormationDeployDTO> FormationDeployDTOs { get; set; }
    }
}