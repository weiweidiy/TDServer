using Game.Share;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(MyDbContext myDbContext, ITokenService tokenService
        , IAccountService accountService, IPlayerService playerService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        private readonly ITokenService tokenService = tokenService;
        IAccountService accountService = accountService;
        IPlayerService playerService = playerService;

        [HttpPost("FastLogin")]
        public async Task<IActionResult> FastLogin([FromBody] AccountDTO accountDto)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (accountDto == null || string.IsNullOrEmpty(accountDto.Uid))
            {
                return BadRequest("Invalid account data.");
            }

            // 将 Account account = await GetAccount(accountDto.FormationType); 改为可空类型
            Account? account = await accountService.GetAccountAsync(accountDto.Uid);
            Player? player = null;
            // 检查是否存在该账号
            if (account == null)
            {
                account = await accountService.CreateAccountAsync(accountDto.Uid, "Guest");
                player = await playerService.CreatePlayerAsync(Guid.NewGuid().ToString(), account.Uid, account.Id);
            }
            else
            {
                // 如果账号已存在，获取绑定的玩家
                player = await playerService.GetPlayerByAccountIdAsync(account.Id);
            }

            if (account == null)
                return BadRequest("create account failed");


            // 检查玩家是否存在
            if (player == null)
            {
                return NotFound("玩家不存在");
            }

            // 生成JWT token
            var token = tokenService.GenerateToken(account, player);

            stopwatch.Stop();
            Console.WriteLine($"FastLogin processed in {stopwatch.ElapsedMilliseconds} ms");

            return Ok(new AccountDTO
            {
                Token = token,
                Uid = account.Uid
                //Account = new { account.Id, account.FormationType, account.Role }
            });
        }
    }
}
