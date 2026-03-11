using Game.Share;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
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

        public MatchController(ITokenService tokenService, TiktokNotifyService notifyService)
        {
            this.tokenService = tokenService;
            this.notifyService = notifyService;
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
                foreach (var p in candidates)
                    waitingPlayers.TryDequeue(out _);

                ushort port = FindAvailablePort(6000, 7000);
                usedPorts[port] = true;

                var process = new RoomServerProcess().StartServerWithVisibleWindow(
                    Guid.NewGuid().ToString(), port, "E:\\UnityProjects\\TDGame\\TDRoom\\Bin\\TDFor4P.exe", "", MatchCount);

                var startFightNtf = new StartFightNtf();
                startFightNtf.Port = port;

                foreach (var p in candidates)
                {
                    // 推送消息放到后台任务，避免阻塞响应
                    Task.Run(() => notifyService.SendNotificationAsync(p.PlayerId, startFightNtf));
                }

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

    public class RoomServerProcess
    {
        public Process? StartServerWithVisibleWindow(string roomId, int port, string path, string args, int matchCount)
        {
            string? workingDir = Path.GetDirectoryName(path);

            var processInfo = new ProcessStartInfo
            {
                FileName = path,
                Arguments = $"-roomId {roomId} -port {port} -maxPlayers {matchCount} " +
                            "-batchmode -nographics -logFile \"\"",
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingDir
            };
            return Process.Start(processInfo);
        }

        /// <summary>
        /// 停止房间进程
        /// </summary>
        public void Stop(Process process)
        {
            if (process != null && !process.HasExited)
            {
                try
                {
                    // 优先尝试关闭主窗口（如果有）
                    if (!process.CloseMainWindow())
                    {
                        // 如果没有主窗口或未响应，则强制杀死
                        process.Kill();
                    }
                    process.WaitForExit(3000); // 最多等待3秒
                }
                catch
                {
                    // 忽略异常
                    throw;
                }
            }
        }
    }

}