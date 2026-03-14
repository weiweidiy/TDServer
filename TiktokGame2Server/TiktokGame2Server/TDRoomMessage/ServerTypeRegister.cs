using Game.Share;
using JFramework;

namespace TDRoom
{
    public enum TDRoomProtocolType
    {
        ReqRoomReady = 1,
        ResRoomReady = 2,
        ReqPlayerData = 3,
        ResPlayerData = 4,
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

    public class ReqPlayerData : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ReqPlayerData; }
        public int PlayerId { get; set; }
    }

    public class ResPlayerData : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ResPlayerData; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
    }



    public class ServerTypeRegister : ITypeRegister
    {
        public Dictionary<int, Type> GetTypes()
        {
            var tables = new Dictionary<int, Type>();
            tables.Add((int)TDRoomProtocolType.ReqRoomReady, typeof(ReqRoomReady));
            tables.Add((int)TDRoomProtocolType.ResRoomReady, typeof(ResRoomReady));
            tables.Add((int)TDRoomProtocolType.ReqPlayerData, typeof(ReqPlayerData));
            tables.Add((int)TDRoomProtocolType.ResPlayerData, typeof(ResPlayerData));
            return tables;
        }
    }
}

