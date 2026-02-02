using JFramework;

namespace Game.Share
{
    public class CurrentGuideStepUpdateNtf : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)ProtocolType.CurrentGuideStepUpdateNtf; }
        public required string CurrentGuideStepBusinessId { get; set; }
    }
}