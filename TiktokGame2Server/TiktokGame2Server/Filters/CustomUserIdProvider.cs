using Microsoft.AspNetCore.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // ¿˝»Á”√ "playerId" claim
        return connection.User?.FindFirst("playerId")?.Value;
    }
}