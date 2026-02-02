using Microsoft.AspNetCore.SignalR;

namespace TiktokGame2Server.Hubs
{
    public class GameHub : Hub
    {
        //// 服务器主动推送消息给所有客户端
        //public async Task BroadcastMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

        //// 客户端调用此方法，服务器可以响应
        //public async Task SendMessage(string message)
        //{
        //    var user = Context.ConnectionId;
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
        // 客户端连接时会自动调用
        public override async Task OnConnectedAsync()
        {
            // 这里可以写你自己的逻辑，比如记录日志、发送欢迎消息等
            await base.OnConnectedAsync();
            // 例如：await Clients.Caller.SendAsync("ReceiveMessage", "系统", "欢迎连接GameHub！");
        }

        // 客户端断开时会自动调用
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // 这里可以写断开连接时的逻辑
            await base.OnDisconnectedAsync(exception);
        }
    }
}