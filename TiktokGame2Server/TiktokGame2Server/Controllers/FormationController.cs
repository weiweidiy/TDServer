using Game.Share;
using Microsoft.AspNetCore.Mvc;
using TiktokGame2Server.Others;
using JFramework;

namespace TiktokGame2Server.Controllers
{
     [ApiController]
    [Route("api/[controller]")]
    public class FormationController : Controller
    {
        ITokenService tokenService;
        IFormationService formationService;
        TiktokConfigManager tiktokConfigService;
        IFormationDeployService formationDeployService;
        TiktokNotifyService notifyService;
        IBuildingService buildingService;

        public FormationController(ITokenService tokenService, TiktokConfigManager tiktokConfigService, IFormationService formationService
            , IFormationDeployService formationDeployService, TiktokNotifyService notifyService, IBuildingService buildingService)
        {
            this.tokenService = tokenService;
            this.tiktokConfigService = tiktokConfigService;
            this.formationService = formationService;
            this.formationDeployService = formationDeployService ?? throw new ArgumentNullException(nameof(formationDeployService));
            this.notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            this.buildingService = buildingService ?? throw new ArgumentNullException(nameof(buildingService));
        }

        [HttpPost("SelectFormation")]
        public async Task<IActionResult> SelectFormation([FromBody] RequestSelectFormation request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");
            if (request.FormationType == FormationType.None)
            {
                return BadRequest("阵型类型不能为空");
            }
            //获取玩家的阵型数据
            var formationData = await formationService.GetFormationAsync(request.FormationType, playerId);
            if (formationData == null)
                return NotFound("阵型数据不存在");

            var targetFormationBusinessId = request.FormationBusinessId;

            formationData.FormationBusinessId = targetFormationBusinessId;
            //更新数据库
            var result = await formationService.UpdateFormationAsync(formationData);

            if (result == null)
                return BadRequest("更新阵型数据失败");

            //转成formationDTO
            var formationDTO = new FormationDTO
            {
                FormationType = result.FormationType,
                FormationBusinessId = result.FormationBusinessId,
            };

            //因为不同阵型的点位不同，所以需要重新设置formationDeploy数据
            //获取当前阵型的所有武将id列表
            var formationSamuraiIds = await formationDeployService.GetFormationSamuraiIdsAsync(request.FormationType, playerId);
            //清除当前阵型的所有点位
            var del = await formationDeployService.DeleteFormationAllSamuraiAsync(request.FormationType, playerId);
            if (!del)
            {
                return BadRequest("清除阵型点位失败");
            }

            //获取本丸建筑的数据，确认可用点位
            var homeData = await buildingService.GetBuildingAsync("1", playerId);
            if(homeData == null)
            {
                return BadRequest("获取本丸建筑数据失败");
            }
            for (int i = 0; i < 9; i++)
            {
                var valid = tiktokConfigService.GetFormationPointValidByIndex(targetFormationBusinessId, i, homeData.Level);
                if(valid)
                {
                    if(formationSamuraiIds != null && formationSamuraiIds.Count > 0)
                    {
                        var samuraiId = formationSamuraiIds.PopFirst();
                        var point = i;
                        //添加到阵型点位
                        var formationDeploy = await formationDeployService.AddOrUpdateFormationSamuraiAsync(request.FormationType, point, samuraiId, playerId);
                    }
                }
            }

            var formationDeploys = await formationDeployService.GetFormationDeployAsync(request.FormationType, playerId);
            if (formationDeploys == null)
            {
                return BadRequest("获取阵型点位数据失败");
            }
            //将formationDeploys转换为DTO
            var formationDeployDTOs = formationDeploys.Select(fd => new FormationDeployDTO
            {
                Id = fd.Id,
                FormationType = fd.FormationType,
                FormationPoint = fd.FormationPoint,
                SamuraiId = fd.SamuraiId
            }).ToList();

            await notifyService.NotifyFormationUpdate(playerId, formationDTO);
            //通知客户端更新阵型
            await notifyService.NotifyFormationDeployUpdate(playerId, formationDeployDTOs);

            return Ok(formationDTO);
        }
    }
}
