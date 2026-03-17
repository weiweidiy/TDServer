using JFramework;
using NetCoreServer;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;

namespace TDRoom
{
    public class NetCoreServerListener : IJSocketListener
    {
        public event Action<IJSocketListener>? onListening;
        public event Action<IJSocketListener, SocketStatusCodes, string>? onClosed;
        public event Action<IJSocketListener, string>? onError;
        public event Action<IJSocketListener, string, byte[]>? onBinary;

        private EchoServer? _server;
        private readonly ConcurrentDictionary<string, EchoSession> _sessions = new();

        public object Clone()
        {
            // 简单实现，实际可根据需要深拷贝
            return MemberwiseClone();
        }

        public void StartListening(ushort port, CancellationToken stoppingToken)
        {
            _server = new EchoServer(IPAddress.Any, port, this, _sessions);
            _server.Start();
            onListening?.Invoke(this);

            // 优雅关闭
            _ = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(500, stoppingToken);
                }
                _server.Stop();
                onClosed?.Invoke(this, SocketStatusCodes.NormalClosure, "Server stopped");
            }, stoppingToken);
        }

        public void StopListening()
        {
            _server?.Stop();
            onClosed?.Invoke(this, SocketStatusCodes.NormalClosure, "Server stopped");
        }

        public void Send(byte[] data)
        {
            // 广播给所有客户端
            foreach (var session in _sessions.Values)
            {
                session.SendAsync(data);
            }
        }

        public bool Send(string clientId, byte[] data)
        {
            if (_sessions.TryGetValue(clientId, out var session))
            {
                session.SendAsync(data);
                return true;
            }
            return false;
        }

        // 内部会话类
        public class EchoSession : TcpSession
        {
            private readonly NetCoreServerListener _listener;
            public EchoSession(TcpServer server, NetCoreServerListener listener) : base(server)
            {
                _listener = listener;
            }

            protected override void OnConnected()
            {
                _listener._sessions[Id.ToString()] = this;
            }

            protected override void OnDisconnected()
            {
                _listener._sessions.TryRemove(Id.ToString(), out _);
                _listener.onClosed?.Invoke(_listener, SocketStatusCodes.NormalClosure, $"Client {Id} disconnected");
            }

            protected override void OnReceived(byte[] buffer, long offset, long size)
            {
                // 触发二进制消息事件
                _listener.onBinary?.Invoke(_listener, Id.ToString(), buffer.Skip((int)offset).Take((int)size).ToArray());
            }

            protected override void OnError(SocketError error)
            {
                _listener.onError?.Invoke(_listener, $"Session error: {error}");
            }
        }

        // 内部服务器类
        public class EchoServer : TcpServer
        {
            private readonly NetCoreServerListener _listener;
            private readonly ConcurrentDictionary<string, EchoSession> _sessions;

            public EchoServer(IPAddress address, int port, NetCoreServerListener listener, ConcurrentDictionary<string, EchoSession> sessions)
                : base(address, port)
            {
                _listener = listener;
                _sessions = sessions;
            }

            protected override TcpSession CreateSession()
            {
                return new EchoSession(this, _listener);
            }

            protected override void OnError(SocketError error)
            {
                _listener.onError?.Invoke(_listener, $"Server error: {error}");
            }
        }
    }
}

//namespace TDRoom
//{
//    /// <summary>
//    /// 基于 NetCoreServer 的 TCP 服务器实现，用于服务器与客户端的交互和消息处理
//    /// </summary>
//    public class GameNetworkServer : IJNetworkServer
//    {
//        private readonly int _port;
//        private EchoServer? _server;

//        private readonly INetworkMessageProcessStrate _processStrategy;
//        private readonly INetworkMessageHandler _messageHandler;

//        public GameNetworkServer(INetworkMessageProcessStrate processStrategy, INetworkMessageHandler messageHandler, int port = 9999)
//        {
//            _port = port;
//            this._processStrategy = processStrategy;
//            this._messageHandler = messageHandler;
//        }

//        public Task StartListening(CancellationToken stoppingToken)
//        {
//            _server = new EchoServer(IPAddress.Any, _port, _processStrategy, _messageHandler);
//            _server.Start();

//            // 支持优雅关闭
//            _ = Task.Run(async () =>
//            {
//                while (!stoppingToken.IsCancellationRequested)
//                {
//                    await Task.Delay(500, stoppingToken);
//                }
//                _server.Stop();
//            }, stoppingToken);

//            Console.WriteLine($"GameNetworkServer started on port {_port}");
//            return Task.CompletedTask;
//        }

//        public class EchoSession : TcpSession
//        {
//            private readonly INetworkMessageProcessStrate _processStrategy;
//            private readonly INetworkMessageHandler _messageHandler;

//            // 新增：会话绑定的玩家ID
//            public string? PlayerId { get; private set; }

//            public EchoSession(TcpServer server, INetworkMessageProcessStrate processStrategy, INetworkMessageHandler messageHandler)
//                : base(server)
//            {
//                _processStrategy = processStrategy;
//                _messageHandler = messageHandler;
//            }

//            protected override void OnReceived(byte[] buffer, long offset, long size)
//            {
//                //string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
//                //Console.WriteLine("[会话 {0}] 收到消息: {1}", Id, message);

//                // 1. 通过策略解析消息
//                var netMsg = _processStrategy.ProcessComingMessage(buffer);

//                // 2. 交给消息处理器处理
//                _messageHandler.Handle(netMsg);

//                // 3. 可根据需要回发响应
//                // SendAsync(...);
//            }
//        }


//        // 2. 定义服务器类：负责监听和创建会话
//        public class EchoServer : TcpServer
//        {
//            private readonly INetworkMessageProcessStrate _processStrategy;
//            private readonly INetworkMessageHandler _messageHandler;

//            public EchoServer(IPAddress address, int port, INetworkMessageProcessStrate processStrategy, INetworkMessageHandler messageHandler)
//                : base(address, port)
//            {
//                _processStrategy = processStrategy;
//                _messageHandler = messageHandler;
//            }

//            protected override TcpSession CreateSession()
//            {
//                return new EchoSession(this, _processStrategy, _messageHandler);
//            }

//            protected override void OnError(SocketError error)
//            {
//                Console.WriteLine("[服务器] 捕获到错误: {0}", error);
//            }
//        }
//    }
//}