using JFramework;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TiktokGame2Server.Others;

namespace TDRoom
{
    public class TDRoomNetworkService : BackgroundService
    {
        private readonly ushort port;
        IJNetworkServer server;
        public TDRoomNetworkService(ITypeRegister typeRegister, IDataConverter dataConverter, IServiceProvider serviceProvider, INetworkServerMessageHandler handler,ushort port = 9999)
        {
            var networkserverBuilder = new NetworkServerBuilder();
            networkserverBuilder.SetProtocolRegister(typeRegister);
            networkserverBuilder.SetMessageHandler(handler);
            networkserverBuilder.SetSocket(new NetCoreServerListener());
            server = networkserverBuilder.Build();
            this.port = port;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //return Task.CompletedTask;
            return server.StartListening(port, stoppingToken);
            //return StartListening(stoppingToken);
        }

        ///// <summary>
        ///// 开始监听TCP连接，接收消息并回复。每个连接在独立的任务中处理，直到服务停止或连接关闭。
        ///// </summary>
        ///// <param name="stoppingToken"></param>
        ///// <returns></returns>
        //async Task StartListening(CancellationToken stoppingToken)
        //{
        //    listener = new TcpListener(IPAddress.Any, port);
        //    listener.Start();
        //    Console.WriteLine($"JNetworkServer started on port {port}");

        //    try
        //    {
        //        while (!stoppingToken.IsCancellationRequested)
        //        {
        //            var client = await listener.AcceptTcpClientAsync(stoppingToken);
        //            _ = Task.Run(async () => {

        //                using var stream = client.GetStream();
        //                byte[] buffer = new byte[1024];
        //                while (!stoppingToken.IsCancellationRequested)
        //                {
        //                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken);
        //                    if (bytesRead == 0) break;

        //                    //to do: 反序列化消息
        //                    var jNetMessage = msgProcessStrate.ProcessComingMessage(buffer);
        //                    var response =  HandleNetMessage(jNetMessage);
        //                    var reply = msgProcessStrate.ProcessOutMessage(response);
        //                    //string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //                    //Console.WriteLine($"收到消息: {msg}");
        //                    //byte[] reply = Encoding.UTF8.GetBytes("Server收到：" + msg);


        //                    await stream.WriteAsync(reply, 0, reply.Length, stoppingToken);
        //                }

        //                // 断开后可执行额外处理，如通知、清理等
        //                // TODO: 这里可以调用通知服务或其他业务逻辑
        //                Console.WriteLine($"client disconnect ");

        //            }, stoppingToken);
        //        }
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        // 正常退出
        //    }
        //    finally
        //    {
        //        listener.Stop();
        //        Console.WriteLine("JNetworkServer stopped.");
        //    }
        //}

        //private IJNetMessage HandleNetMessage(IJNetMessage jNetMessage)
        //{
        //    try
        //    {
        //        handler.Handle(jNetMessage);
        //    }
        //    catch
        //    {
        //        throw;
        //    }


        //    switch (jNetMessage.TypeId)
        //    {
        //        case (int)TDRoomProtocolType.ReqRoomReady:
        //            {
        //                return new ResRoomReady() { Code = 0 };
        //            }
        //        case (int)TDRoomProtocolType.ReqPlayerData:
        //            {
        //                var req = jNetMessage as ReqPlayerData;
        //                return new ResPlayerData() { PlayerId = req.PlayerId, PlayerName = $"Player{req.PlayerId}" };
        //            }

        //        default:
        //            throw new NotImplementedException("没有实现消息处理 msgTypeId:" + jNetMessage.TypeId);
        //    }
        //}

    }
}
