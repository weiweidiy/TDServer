using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IAccountService
    {
        Task<Account?> GetAccountAsync(string accountUid);
        Task<Account> CreateAccountAsync(string accountUid, string role);
    }
}
