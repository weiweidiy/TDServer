namespace TiktokGame2Server.Others
{
    public interface INotifyService
    {
        Task SendNotificationAsync(int playerId, object message);
    }
}
