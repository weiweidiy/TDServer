using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class AccountService : IAccountService
    {
        private readonly MyDbContext _dbContext;
        public AccountService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Account?> GetAccountAsync(string accountUid)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Uid == accountUid);
        }
        public async Task<Account> CreateAccountAsync(string accountUid, string role)
        {
            var account = new Account
            {
                Uid = accountUid,
                Role = role
            };
            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }
    }
}
