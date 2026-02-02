using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface ITokenService
    {
        string GenerateToken(Account account, Player player);

        bool ValidateToken(string token);

        string? GetAccountUidFromToken(string token);

        string? GetPlayerUidFromToken(string token);

        int? GetPlayerIdFromToken(string token);
    }


}
