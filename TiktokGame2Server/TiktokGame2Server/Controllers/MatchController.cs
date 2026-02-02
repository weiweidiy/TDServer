using Game.Share;
using Microsoft.AspNetCore.Mvc;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MatchController> _logger;

        ITokenService tokenService;

        public MatchController(ILogger<MatchController> logger, ITokenService tokenService)
        {
            _logger = logger;
            this.tokenService = tokenService;
        }

        [HttpPost("Match")]
        public async Task<IActionResult> Match([FromBody] RequestMatch request)
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token) ?? throw new Exception("解析token异常 playerUid");
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常 playerId");

            var res = new ResponseMatch()
            {
                Ip = "127.0.0.1",
                Port = 7777,
            };

            return Ok(res);
        }

    }
}
