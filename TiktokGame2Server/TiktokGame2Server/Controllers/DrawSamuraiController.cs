using Game.Share;
using Microsoft.AspNetCore.Mvc;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DrawSamuraiController : Controller
    {
        ITokenService tokenService;
        IDrawSamuraiService drawSamuraiService;
        TiktokConfigManager tiktokConfigService;
        ICurrencyService currencyService;
        TiktokNotifyService notifyService;
        IPlayerService playerService;
        IBuildingService buildingService;
        ISamuraiService samuraiService;
        public DrawSamuraiController(ITokenService tokenService, IDrawSamuraiService drawSamuraiService, TiktokConfigManager tiktokConfigService
            , ICurrencyService currencyService, TiktokNotifyService notifyService, IPlayerService playerService, IBuildingService buildingService
            , ISamuraiService samuraiService)
        {
            this.tokenService = tokenService;
            this.drawSamuraiService = drawSamuraiService;
            this.tiktokConfigService = tiktokConfigService;
            this.currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            this.notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            this.playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
            this.buildingService = buildingService ?? throw new ArgumentNullException(nameof(buildingService));
            this.samuraiService = samuraiService ?? throw new ArgumentNullException(nameof(samuraiService));
        }

        [HttpPost("Draw")]
        public async Task<IActionResult> Draw([FromBody] RequestDrawSamurai request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token) ?? throw new Exception("解析token异常");
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            //获取主城当前等级的参数
            var home = await buildingService.GetBuildingAsync("1", playerId);
            if(home == null)
                return BadRequest(new { message = "没有找到本丸数据！" });

            var max = tiktokConfigService.GetBuildingArg("1", home.Level);
            var allSamurai =await samuraiService.GetAllSamuraiAsync(playerId);
            if(allSamurai != null && allSamurai.Count + request.Count > max)
                return BadRequest(new { message = "没有足够的武将空间，请升级本丸！" });


            //抽取的个数
            var poolType = request.DrawPoolType;
            var count = request.Count;

            //从配置表中获取抽取消耗的货币
            var drawCost = tiktokConfigService.GetDrawCost(poolType, count);
            var resourceType = drawCost.Item1;
            var businessId = drawCost.Item2;
            var costAmount = drawCost.Item3;



            //判断玩家货币是否足够
            CurrencyDTO remainCurrencyDTO = null;
            if (resourceType == ResourceType.Currency)
            {
                var currencyType = (CurrencyType)int.Parse(businessId);

                var currency = await currencyService.GetCurrency(playerId, currencyType);
                if (currency.Count < costAmount)
                {
                    return BadRequest(new { message = $"货币不足，当前{currencyType}数量：{currency.Count}，需要：{costAmount}" });
                }

                //货币足够，扣除货币
                currency = await currencyService.SpendCurrency(playerId, currencyType, costAmount);

                remainCurrencyDTO = new CurrencyDTO
                {
                    Count = currency.Count
                };
            }
            else if (resourceType == ResourceType.BagItem)
            {
                return BadRequest(new { message = "暂时没有实现道具抽卡" });
            }
            else
            {
                return BadRequest(new { message = "不支持的资源类型" });
            }

            //单抽
            var samurai = await drawSamuraiService.DrawSamurai(playerId);
            var samuraiDTOs = new List<SamuraiDTO>();
            var samuraiDTO = new SamuraiDTO()
            {
                Id = samurai.Id,
                BusinessId = samurai.BusinessId,
                SoldierBusinessId = samurai.SoldierBusinessId,
                Level = 1,
                Experience = 0,
                CurHp = tiktokConfigService.FormulaMaxHpByLevel(1)
            };
            samuraiDTOs.Add(samuraiDTO);


            var response = new ResponseDraw
            {
                SamuraiDTOs = samuraiDTOs,
                CurrencyDTO = remainCurrencyDTO,
            };


            await notifyService.NotifyCurrenciesUpdate(playerId);
            await notifyService.NotifySamuraisUpdate(playerId);

            //如果稀有度大于等于5，全服公告
            var rare = tiktokConfigService.GetSamuraiRare(samurai.BusinessId);
            if (rare >= 5)
            {
                //获取玩家昵称
                var player = await playerService.GetPlayerAsync(playerUid);
                if (player == null)
                {
                    return BadRequest(new { message = "玩家不存在" });
                }

                //var msg = $"玩家{player.Name}抽到了稀有武士{samurai.BusinessId}，快去看看吧！";

                await notifyService.SendGlobalAnnouncement(player.Name, samuraiDTO);
            }

            return Ok(response);
        }
    }
}