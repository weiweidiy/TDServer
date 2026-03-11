using System.Net;
using System.Net.Sockets;
using System.Text;


public class JNetworkServer : BackgroundService
{
    private readonly int port;
    private TcpListener? listener;

    public JNetworkServer(int port = 9999)
    {
        this.port = port;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"JNetworkServer started on port {port}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(stoppingToken);
                _ = Task.Run(() => HandleClient(client, stoppingToken), stoppingToken);
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

    private async Task HandleClient(TcpClient client, CancellationToken token)
    {
        using var stream = client.GetStream();
        byte[] buffer = new byte[1024];
        while (!token.IsCancellationRequested)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
            if (bytesRead == 0) break;
            string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"收到消息: {msg}");
            byte[] reply = Encoding.UTF8.GetBytes("Server收到：" + msg);
            await stream.WriteAsync(reply, 0, reply.Length, token);
        }
    }
}