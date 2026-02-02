using Game.Share;
using Microsoft.AspNetCore.Mvc;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : Controller
    {
        ITokenService tokenService;

        TiktokConfigManager tiktokConfigService;
        ICurrencyService currencyService;
        TiktokNotifyService notifyService;
        IBuildingService buildingService;
        ILevelNodesService levelNodesService;
        public BuildingController(ITokenService tokenService, TiktokConfigManager tiktokConfigService
            , ICurrencyService currencyService, TiktokNotifyService notifyService, IBuildingService buildingService, ILevelNodesService levelNodesService)
        {
            this.tokenService = tokenService;
            this.tiktokConfigService = tiktokConfigService;
            this.currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            this.notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            this.buildingService = buildingService ?? throw new ArgumentNullException(nameof(buildingService));
            this.levelNodesService = levelNodesService ?? throw new ArgumentNullException(nameof(levelNodesService));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RequestCreateBuilding request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                return BadRequest("Token不能为空");
            }
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            var buildingBusinessId = request.BuildingBusinessId;

            //检查建筑是否存在
            if (!CheckBuildingBusinessId(buildingBusinessId))
            {
                return BadRequest("建筑配置不存在" + buildingBusinessId);
            }
            //检查玩家是否已经拥有该建筑
            var existingBuilding = await buildingService.GetBuildingAsync(buildingBusinessId, playerId);
            if (existingBuilding != null)
            {
                return BadRequest("玩家已经拥有该建筑" + buildingBusinessId);
            }

            //获取建筑解锁所需关卡level
            var targetUnlockLevel = tiktokConfigService.GetBuildingUnlockLevel(buildingBusinessId);
            //判断关卡进度是否足够
            var requiredLevelNodeUid = tiktokConfigService.GetLevelFirstNodeBusinessId(targetUnlockLevel.ToString());
            var requiredLevelNode = await levelNodesService.GetLevelNodeAsync(requiredLevelNodeUid, playerId);

            //如果levelNode为null，说明关卡节点还没有解锁
            if (requiredLevelNode == null)
            {
                var previousNodeBusinessId = tiktokConfigService.GetPreviousLevelNode(requiredLevelNodeUid);
                //数据库查询是否存在该节点
                var previousNode = await levelNodesService.GetLevelNodeAsync(previousNodeBusinessId, playerId);
                if (previousNode == null)
                {
                    return BadRequest($"建筑解锁失败，关卡进度不足，需要先通关前置关卡节点 {previousNodeBusinessId}");
                }

            }


            //创建时候免费，直接创建建筑
            var newBuilding = await buildingService.AddBuildingAsync(buildingBusinessId, playerId);
            if (newBuilding == null)
            {
                return BadRequest("创建建筑失败");
            }
            var buildingDTO = new BuildingDTO
            {
                BusinessId = newBuilding.BusinessId,
                Level = newBuilding.Level,
            };

            //创建response
            var response = new ResponseCreateBuilding
            {
                BuildingDTO = buildingDTO
            };

            //通知前端建筑数据更新
            await notifyService.NotifyBuildingUpdate(playerId, buildingDTO);

            return Ok(response);
        }


        [HttpPost("Upgrade")]
        public async Task<IActionResult> Upgrade([FromBody] RequestUpgradeBuilding request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                return BadRequest("Token不能为空");
            }
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            var buildingBusinessId = request.BuildingBusinessId;
            //检查建筑是否存在
            if (!CheckBuildingBusinessId(buildingBusinessId))
            {
                return BadRequest("建筑配置不存在" + buildingBusinessId);
            }
            //检查玩家是否拥有该建筑
            var existingBuilding = await buildingService.GetBuildingAsync(buildingBusinessId, playerId);
            if (existingBuilding == null)
            {
                return BadRequest("玩家不拥有该建筑");
            }
            //检查是否已经在升级中
            var existingUpgradeSchedule = await buildingService.GetBuildingUpgradeScheduleAsync(existingBuilding.Id, playerId);
            if (existingUpgradeSchedule != null)
            {
                return BadRequest("建筑正在升级中");
            }

            //获取升级所需资源
            var upgradeCost = tiktokConfigService.GetBuildingUpgradeCost(buildingBusinessId, existingBuilding.Level);
            var currencyType = (CurrencyType)int.Parse(upgradeCost.Item1);
            var costAmount = upgradeCost.Item2;



            //检查玩家是否有足够资源
            var hasEnough = await currencyService.HasEnoughCurrency(playerId, currencyType, costAmount);
            if (!hasEnough)
            {
                return BadRequest($"{currencyType} 资源不足,需要{costAmount}");
            }

            //扣除资源
            var updatedCurrency = await currencyService.SpendCurrency(playerId, currencyType, costAmount);
            if (updatedCurrency == null)
            {
                return BadRequest("扣除资源失败");
            }
            //把updatedCurrency转换为CurrencyDTO
            var currencyDTO = new CurrencyDTO
            {
                CurrencyType = updatedCurrency.CurrencyType,
                Count = updatedCurrency.Count
            };

            //获取升级所需时间
            var upgradeDurationSeconds = tiktokConfigService.GetFormulaBuildingUpgradeDuration(buildingBusinessId, existingBuilding.Level);
            var upgradeDuration = TimeSpan.FromSeconds(upgradeDurationSeconds);
            var upgradeSchedule = await buildingService.BeginUpgrade(buildingBusinessId, playerId, upgradeDuration);
            if (upgradeSchedule == null)
            {
                return BadRequest("创建升级任务失败");
            }

            //构建BuildingDTO
            var buildingDTO = new BuildingDTO
            {
                BusinessId = existingBuilding.BusinessId,
                Level = existingBuilding.Level,
                UpgradeEndTime = upgradeSchedule.UpgradeEndAt
            };


            var response = new ResponseUpgradeBuilding
            {
                BuildingDTO = buildingDTO
            };


            //通知前端资源数据更新
            await notifyService.NotifyCurrenciesUpdate(playerId, currencyDTO);
            //通知前端建筑数据更新
            await notifyService.NotifyBuildingUpdate(playerId, buildingDTO);

            return Ok(response);

        }


        [HttpPost("UpgradeImmediately")]
        public async Task<IActionResult> UpgradeImmediately([FromBody] RequestUpgradeBuilding request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                return BadRequest("Token不能为空");
            }
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            var buildingBusinessId = request.BuildingBusinessId;
            //检查建筑是否存在
            if (!CheckBuildingBusinessId(buildingBusinessId))
            {
                return BadRequest("建筑不存在");
            }
            //检查玩家是否拥有该建筑
            var existingBuilding = await buildingService.GetBuildingAsync(buildingBusinessId, playerId);
            if (existingBuilding == null)
            {
                return BadRequest("玩家不拥有该建筑");
            }
            //检查是否在升级中
            var existingUpgradeSchedule = await buildingService.GetBuildingUpgradeScheduleAsync(existingBuilding.Id, playerId);
            if (existingUpgradeSchedule == null)
            {
                return BadRequest("建筑没有在升级");
            }

            //获取剩余升级时间
            var remainingTime = existingUpgradeSchedule.UpgradeEndAt - DateTime.UtcNow;

            //获取升级所需资源
            var upgradeCost = tiktokConfigService.GetFormulaBuildingUpgradeImmediatelyCost(remainingTime.TotalSeconds);
            var currencyType = (CurrencyType)int.Parse(upgradeCost.Item1);
            var costAmount = upgradeCost.Item2;

            //检查玩家是否有足够资源
            var hasEnough = await currencyService.HasEnoughCurrency(playerId, currencyType, costAmount);
            if (!hasEnough)
            {
                return BadRequest($"{currencyType} 资源不足,需要{costAmount}");
            }

            //扣除资源
            var updatedCurrency = await currencyService.SpendCurrency(playerId, currencyType, costAmount);
            if (updatedCurrency == null)
            {
                return BadRequest("扣除资源失败");
            }
            //把updatedCurrency转换为CurrencyDTO
            var currencyDTO = new CurrencyDTO
            {
                CurrencyType = updatedCurrency.CurrencyType,
                Count = updatedCurrency.Count
            };

            var success = await buildingService.CompleteUpdateImmediately(existingBuilding.Id, playerId);
            if (!success)
            {
                return BadRequest("立即完成升级失败");
            }
            //构建BuildingDTO
            var buildingDTO = new BuildingDTO
            {
                BusinessId = existingBuilding.BusinessId,
                Level = existingBuilding.Level, //升级成功，等级+1
                UpgradeEndTime = null //升级完成，结束时间为空
            };
            var response = new ResponseUpgradeBuilding
            {
                BuildingDTO = buildingDTO
            };
            //通知前端资源数据更新
            await notifyService.NotifyCurrenciesUpdate(playerId, currencyDTO);
            //通知前端建筑数据更新
            await notifyService.NotifyBuildingUpdate(playerId, buildingDTO);
            return Ok(response);

        }


        bool CheckBuildingBusinessId(string buildingBusinessId)
        {
            return tiktokConfigService.HasBuildingBusinessId(buildingBusinessId);
        }
    }
}