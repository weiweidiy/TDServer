using Game.Share;
using Microsoft.AspNetCore.Mvc;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuideStepController : Controller
    {
        ITokenService tokenService;
        TiktokConfigManager tiktokConfigService;
        TiktokNotifyService notifyService;
        IGuideService guideService;

        public GuideStepController(ITokenService tokenService, TiktokConfigManager tiktokConfigService, IGuideService guideService,
            TiktokNotifyService notifyService)
        {
            this.tokenService = tokenService;
            this.tiktokConfigService = tiktokConfigService;
            this.notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            this.guideService = guideService ?? throw new ArgumentNullException(nameof(guideService));
        }

        [HttpPost("CompleteGuideStep")]
        public async Task<IActionResult> CompleteGuideStep([FromBody] RequestCompleteGuideStep request)
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            if (playerId == 0)
                return BadRequest(new { message = "解析token异常" });

            if (string.IsNullOrEmpty(request.GuideBusinessId))
                return BadRequest(new { message = "参数异常" });

            var guideBusinessId = request.GuideBusinessId;

            var isComplete = await guideService.IsGuideStepCompletedAsync(playerId, guideBusinessId);
            if(isComplete)
            {
                return BadRequest(new { message = "该引导步骤已完成,不能重复请求" });
            }

            var currentGuideStepBusinessId = await guideService.CompleteGuideStepAsync(playerId, guideBusinessId);
            if (currentGuideStepBusinessId == null || currentGuideStepBusinessId == string.Empty) 
                return BadRequest(new { message = "完成引导步骤失败" });


            //通知客户端引导步骤更新
            await notifyService.NotifyGuideStepUpdate(playerId, currentGuideStepBusinessId);
            return Ok(new ResponseCompleteGuideStep
            {
                GuideBusinessId = currentGuideStepBusinessId
            });

        }
    }
}
