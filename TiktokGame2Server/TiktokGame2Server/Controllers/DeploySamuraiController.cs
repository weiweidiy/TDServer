using Game.Share;
using Microsoft.AspNetCore.Mvc;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeploySamuraiController : Controller
    {
        ITokenService tokenService;
        IFormationDeployService formationDeployService;
        TiktokConfigManager tiktokConfigService;
        TiktokNotifyService notifyService;
        ISamuraiService samuraiService;
        IBuildingService buildingService;
        IFormationService formationService;

        public DeploySamuraiController(ITokenService tokenService, TiktokConfigManager tiktokConfigService, IFormationDeployService formationDeployService
            , TiktokNotifyService notifyService, ISamuraiService samuraiService, IBuildingService buildingService, IFormationService formationService)
        {
            this.tokenService = tokenService;
            this.tiktokConfigService = tiktokConfigService;
            this.formationDeployService = formationDeployService;
            this.notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            this.samuraiService = samuraiService ?? throw new ArgumentNullException(nameof(samuraiService));
            this.buildingService = buildingService ?? throw new ArgumentNullException(nameof(buildingService));
            this.formationService = formationService ?? throw new ArgumentNullException(nameof(formationService));
        }

        [HttpPost("Deploy")]
        public async Task<IActionResult> Deploy([FromBody] RequestDeploy request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            var formationType = (FormationType)request.FormationType;
            var formationPoint = request.FormationPoint;
            var targetSamuraiId = request.SamuraiId;

            //检查点位是否已经解锁
            var homeData = await buildingService.GetBuildingAsync("1", playerId);
            if(homeData == null)
            {
                return BadRequest(new { message = "本丸数据不存在" });
            }
            var formationData = await formationService.GetFormationAsync(formationType, playerId);
            if (formationData == null)
            {
                return BadRequest(new { message = "阵型数据不存在" });
            }
            var isValidPoint = tiktokConfigService.GetFormationPointValidByIndex(formationData.FormationBusinessId, formationPoint, homeData.Level);
            if (!isValidPoint)
            {
                return BadRequest(new { message = "该点位未解锁，不能部署武将" });
            }

            //如果武将已经在相同点位，则不能操作
            if (await formationDeployService.IsSamuraiInFormationPointAsync(targetSamuraiId, playerId, formationType, formationPoint))
            {
                return BadRequest(new { message = "武将已经在阵型中该点位，不能重复" });
            }

            var formationSamuraiIds = await formationDeployService.GetFormationSamuraiIdsAsync(formationType, playerId);
            //如果samuraiId<0，且在阵武将只有1个时，则不能操作
            if (targetSamuraiId < 0)
            {        
                if (formationSamuraiIds.Count <= 1)
                {
                    return BadRequest(new { message = "阵型中至少需要一个武将" });
                }
            }


            if (targetSamuraiId > 0 && !formationSamuraiIds.Contains(targetSamuraiId))
            {
                var targetSamurai = await samuraiService.GetSamuraiAsync(targetSamuraiId);
                if (targetSamurai == null)
                {
                    return BadRequest(new { message = "目标武将不存在" + targetSamuraiId });
                }

                //如果阵上已经有相同的businessId的武将，则不能操作
                var deployDataList = await formationDeployService.GetFormationDeployAsync(formationType, playerId);
                if (deployDataList != null && deployDataList.Count > 0)
                {
                    foreach (var deploy in deployDataList)
                    {
                        if (deploy.Samurai == null)
                        {
                            continue;
                        }

                        var samuraiBusinessId = deploy.Samurai.BusinessId;
                        var group = tiktokConfigService.GetSamuraiGroup(samuraiBusinessId);
                        var targetGroup = tiktokConfigService.GetSamuraiGroup(targetSamurai.BusinessId);
                        if (group == targetGroup)
                        {
                            return BadRequest(new { message = "同一阵型中不能部署相同类型的武将" });
                        }
                    }
                }
            }


            var dataList = await formationDeployService.UpdateFormationDeployAsync(formationType, formationPoint, targetSamuraiId, playerId);

            //如果data为null，表示更新失败
            if (dataList == null)
            {
                return BadRequest(new { message = "更新阵型失败" });
            }

            ////从dataList过滤掉不是当前formationType的
            //dataList = dataList.Where(fd => fd.FormationType == formationType).ToList();

            //转换成 FormationDeployDTO
            var formationNewDataList = dataList.Select(fd => new FormationDeployDTO
            {
                Id = fd.Id,
                FormationType = fd.FormationType,
                FormationPoint = fd.FormationPoint,
                SamuraiId = fd.SamuraiId,
            }).ToList();



            //构造responseDeploy
            var responseDeploy = new ResponseDeploy
            {
                FormationAtkDTO = formationType == FormationType.FormationAtk? formationNewDataList : null,
                FormationDefDTO = formationType == FormationType.FormationDef ? formationNewDataList : null,
            };

            await notifyService.NotifyFormationDeployUpdate(playerId, formationNewDataList);

            return Ok(responseDeploy);
        }

    }
}

//更新规则：如果formationNewDataList中的数据在formationDataList中不存在，则添加；如果存在，则更新，如果数据库中有的数据在formationNewDataList中不存在，则删除 (用playerId和formationType和formationPoint作为条件进行更新)
