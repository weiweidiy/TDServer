using System.Diagnostics;
using JFramework;
using TiktokGame2Server.Controllers;
namespace TDRoom
{
    /// <summary>
    /// 进程参数，包含房间ID，端口号和玩家列表等信息。当房间准备就绪时，会通过 onRoomReady 事件传递给监听者。
    /// </summary>
    public class RoomProcessData
    {
        public string? roomId;
        public ushort port;
        public MatchPlayer[]? players;
    }

    /// <summary>
    /// 处理启动房间服务器进程的逻辑，监听房间准备就绪的消息，并触发 onRoomReady 事件。提供 StartServerWithVisibleWindow 方法启动进程，Stop 方法停止进程。
    /// </summary>
    public class RoomServerProcess : INetworkMessageHandler
    {
        public event Action<RoomProcessData> onRoomReady;

        Dictionary<string, RoomProcessData> roomDataMap = new Dictionary<string, RoomProcessData>();

        public void Handle(IJNetMessage message)
        {
            switch (message.TypeId)
            {
                case (int)TDRoomProtocolType.ReqRoomReady:
                    var req = message as ReqRoomReady;

                    if (!roomDataMap.ContainsKey(req.RoomId))
                        throw new Exception($"房间 {req.RoomId} 不存在");

                    onRoomReady?.Invoke(roomDataMap[req.RoomId]);

                    roomDataMap.Remove(req.RoomId);
                    break;
            }
        }

        /// <summary>
        /// 启动进程，启动后会等待房间准备就绪的消息，收到消息后会触发 onRoomReady 事件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <param name="matchCount"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Process? StartServerWithVisibleWindow(RoomProcessData data, string path, int matchCount)
        {
            var roomId = data.roomId;
            var port = data.port;
            if(data.players == null || data.players.Length == 0)
                throw new Exception($"房间 {roomId} 玩家列表不能为空");

            if(roomId == null)
                throw new Exception($"房间 {roomId} 房间ID不能为空");

            var playerIds = data.players.Select(p => p.PlayerId).ToArray();

            string? workingDir = Path.GetDirectoryName(path);
            string playerIdsArg = $"-playerIds {string.Join(",", playerIds)}";

            var processInfo = new ProcessStartInfo
            {
                FileName = path,
                Arguments = $"-roomId {roomId} -port {port} -maxPlayers {matchCount} {playerIdsArg}" +
                            "-batchmode -nographics -logFile \"\"",
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingDir
            };

            if (roomDataMap.ContainsKey(roomId))
            {
                throw new Exception($"房间 {roomId} 已经存在");
            }

            roomDataMap.Add(roomId, data);

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