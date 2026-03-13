using Game.Share;
using JFramework;

namespace TDRoom
{
    public enum TDRoomProtocolType
    {
        ReqRoomReady = 1,
        ResRoomReady = 2,
    }

    public class ReqRoomReady : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ReqRoomReady; }
        public string RoomId { get; set; } = string.Empty;
    }

    public class ResRoomReady : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ResRoomReady; }

        public int Code;
    }

    public class ServerTypeRegister : ITypeRegister
    {
        public Dictionary<int, Type> GetTypes()
        {
            var tables = new Dictionary<int, Type>();
            tables.Add((int)TDRoomProtocolType.ReqRoomReady, typeof(ReqRoomReady));
            tables.Add((int)TDRoomProtocolType.ResRoomReady, typeof(ResRoomReady));
            return tables;
        }
    }
}

