using Game.Share;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using TDRoom;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private static readonly ConcurrentQueue<MatchPlayer> waitingPlayers = new();
        private static readonly ConcurrentDictionary<ushort, bool> usedPorts = new();
        private const int MatchCount = 2; // 设定匹配人数

        ITokenService tokenService;
        TiktokNotifyService notifyService;
        RoomServerProcess roomServerProcess;

        public MatchController(ITokenService tokenService, TiktokNotifyService notifyService, RoomServerProcess roomServerProcess)
        {
            this.tokenService = tokenService;
            this.notifyService = notifyService;
            this.roomServerProcess = roomServerProcess;

            Console.WriteLine(roomServerProcess.GetHashCode());
            this.roomServerProcess.onRoomReady += RoomServerProcess_onRoomReady;
        }

        private void RoomServerProcess_onRoomReady(RoomProcessData data)
        {
            var startFightNtf = new StartFightNtf();
            startFightNtf.Port = data.port;
            if(data.players == null || data.players.Length == 0)
                throw new Exception($"房间 {data.roomId} 玩家列表不能为空");

            foreach (var p in data.players)
            {
                notifyService.SendNotificationAsync(p.PlayerId, startFightNtf);
            }
        }


        [HttpPost("Match")]
        public async Task<IActionResult> Match([FromBody] RequestMatch request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token) ?? throw new Exception("解析token异常 playerUid");
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常 playerId");

            int playerLevel = 1; // 可从 request 或数据库获取

            var self = new MatchPlayer
            {
                PlayerId = playerId,
                PlayerUid = playerUid,
                Level = playerLevel,
                RequestTime = DateTime.UtcNow
            };

            //添加到等待队列
            waitingPlayers.Enqueue(self);

            // 公平匹配：筛选等级差距在2以内的玩家
            var candidates = waitingPlayers.Where(p => Math.Abs(p.Level - self.Level) <= 2).Take(MatchCount).ToList();

            // 如果找到足够的玩家，开始匹配
            if (candidates.Count == MatchCount)
            {
                // 从队列移除这些玩家
                var players = new List<MatchPlayer>();
                foreach (var p in candidates)
                {
                    waitingPlayers.TryDequeue(out _);
                    players.Add(p);
                }
                    
                ushort port = FindAvailablePort(6000, 7000);
                usedPorts[port] = true;
                var roomId = "room_001";

                RoomProcessData data = new RoomProcessData
                {
                    roomId = roomId,
                    port = port,
                    players = players.ToArray()
                };
                // 启动房间服务器
                roomServerProcess.StartServerWithVisibleWindow(data, "E:\\UnityProjects\\TDGame\\TDRoom\\Bin\\TDFor4P.exe",  MatchCount);

                //to do: 等待JNetworkServer接收到该roomId的消息后，才发送StartFightNtf通知玩家连接房间服务器

                

                var res = new ResponseMatch()
                {
                    Ip = "127.0.0.1",
                    Port = port,
                };
                return Ok(res);
            }
            else
            {
                await Task.Delay(5000); // 5秒等待
                if (waitingPlayers.Contains(self))
                {
                    waitingPlayers.TryDequeue(out _);
                    await notifyService.SendNotificationAsync(self.PlayerId, new { Message = "匹配超时，请重试" });
                    return Ok(new { Message = "匹配超时，请重试" });
                }
                else
                {
                    return Ok(new { Message = "匹配中..." });
                }
            }
        }

        private ushort FindAvailablePort(ushort startPort, ushort endPort)
        {
            for (ushort port = startPort; port <= endPort; port++)
            {
                if (IsPortAvailable(port) && !usedPorts.ContainsKey(port))
                {
                    return port;
                }
            }
            throw new Exception("没有可用端口");
        }

        private bool IsPortAvailable(ushort port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpListeners = ipGlobalProperties.GetActiveTcpListeners();
            return !tcpListeners.Any(ep => ep.Port == port);
        }
    }

    public class MatchPlayer
    {
        public int PlayerId { get; set; }
        public string PlayerUid { get; set; } = string.Empty;
        public int Level { get; set; }
        public DateTime RequestTime { get; set; }
    }

}