using JFramework;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TiktokGame2Server.Others;

namespace TDRoom
{
    public class JNetworkServer : BackgroundService
    {
        private readonly int port;
        private TcpListener? listener;

        INetworkMessageProcessStrate msgProcessStrate;

        IServiceProvider serviceProvider;

        INetworkMessageHandler handler;

        public JNetworkServer(IDataConverter dataConverter, IServiceProvider serviceProvider, INetworkMessageHandler handler, int port = 9999)
        {
            this.port = port;
            this.serviceProvider = serviceProvider;
            Console.WriteLine(handler.GetHashCode());
            var typeRegister = new ServerTypeRegister();
            var typeResolver = new JNetMessageJsonTypeResolver(dataConverter, typeRegister);
            this.msgProcessStrate = new JNetworkMessageProcessStrate(new JNetMessageJsonSerializerStrate(dataConverter), typeResolver, null, null);
            this.handler = handler;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return StartListening(stoppingToken);
        }

        /// <summary>
        /// 开始监听TCP连接，接收消息并回复。每个连接在独立的任务中处理，直到服务停止或连接关闭。
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        async Task StartListening(CancellationToken stoppingToken)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"JNetworkServer started on port {port}");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var client = await listener.AcceptTcpClientAsync(stoppingToken);
                    _ = Task.Run(async () => {

                        using var stream = client.GetStream();
                        byte[] buffer = new byte[1024];
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken);
                            if (bytesRead == 0) break;

                            //to do: 反序列化消息
                            var jNetMessage = msgProcessStrate.ProcessComingMessage(buffer);
                            var response =  HandleNetMessage(jNetMessage);
                            var reply = msgProcessStrate.ProcessOutMessage(response);
                            //string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            //Console.WriteLine($"收到消息: {msg}");
                            //byte[] reply = Encoding.UTF8.GetBytes("Server收到：" + msg);


                            await stream.WriteAsync(reply, 0, reply.Length, stoppingToken);
                        }

                        // 断开后可执行额外处理，如通知、清理等
                        // TODO: 这里可以调用通知服务或其他业务逻辑
                        Console.WriteLine($"client disconnect ");

                    }, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // 正常退出
            }
            finally
            {
                listener.Stop();
                Console.WriteLine("JNetworkServer stopped.");
            }
        }

        private IJNetMessage HandleNetMessage(IJNetMessage jNetMessage)
        {
            try
            {
                handler.Handle(jNetMessage);
            }
            catch
            {
                throw;
            }


            switch (jNetMessage.TypeId)
            {
                case (int)TDRoomProtocolType.ReqRoomReady:
                    {
                        return new ResRoomReady() { Code = 0 };
                    }
                    
                default:
                    throw new NotImplementedException("没有实现消息处理 msgTypeId:" + jNetMessage.TypeId);
            }
        }

    }
}
