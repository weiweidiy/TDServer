using Game.Share;
using Microsoft.AspNetCore.Mvc;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SamuraiController : Controller
    {
        ITokenService tokenService;
        ISamuraiService samuraiService;
        TiktokConfigManager tiktokConfigService;
        IHpPoolService hpPoolService;
        IFormationDeployService formationService;
        public SamuraiController(ITokenService tokenService, ISamuraiService samuraiService
            , IHpPoolService hpPoolService, IFormationDeployService formationService, TiktokConfigManager tiktokConfigService)
        {
            this.tokenService = tokenService;
            this.samuraiService = samuraiService;
            this.tiktokConfigService = tiktokConfigService;
            this.hpPoolService = hpPoolService ?? throw new ArgumentNullException(nameof(hpPoolService));
            this.formationService = formationService ?? throw new ArgumentNullException(nameof(formationService));
        }

        [HttpPost("AddExperience")]
        public async Task<IActionResult> AddExperience([FromBody] RequestAddSamuraiExp request)
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            var targetSamuraiId = request.TargetSamuraiId;
            var expSamuraiIds = request.ExpSamuraisIds;
            //检查这些ID是否是当前账号的武将ID
            if (targetSamuraiId <= 0 || expSamuraiIds == null || expSamuraiIds.Count == 0)
            {
                return BadRequest("Invalid request data.");
            }
            //检查武将是否存在
            var targetSamurai = await samuraiService.GetSamuraiAsync(targetSamuraiId);
            if (targetSamurai == null || targetSamurai.PlayerId != playerId)
            {
                return NotFound("Target samurai not found or does not belong to the player.");
            }
            //添加经验前的武将等级
            var targetSamuraiLevel = targetSamurai.Level;

            //检查经验武将是否存在
            var expSamuraisValid = await samuraiService.CheckSamurais(expSamuraiIds, playerId);
            if (!expSamuraisValid)
            {
                return NotFound("One or more experience samurai IDs are invalid or do not belong to the player.");
            }

            //检查经验武将是否在formation中，如果是，则不能作为经验武将
            var formationSamuraiIds = await formationService.GetFormationSamuraiIdsAsync(playerId);
            var invalidExpSamuraiIds = expSamuraiIds.Where(id => formationSamuraiIds.Contains(id)).ToList();
            if (invalidExpSamuraiIds.Count > 0)
            {
                return BadRequest($"The following samurai IDs are in formation and cannot be used for experience: {string.Join(", ", invalidExpSamuraiIds)}");
            }


            //获取所有经验值武将的经验值总和
            var totalExp = 0;
            var expSamurais = await samuraiService.GetSamuraisAsync(expSamuraiIds);
            foreach (var expSamurai in expSamurais)
            {
                totalExp += expSamurai.Experience;
                totalExp += tiktokConfigService.GetSamuraiExpAddValue(expSamurai.BusinessId);
            }

            //添加经验值到目标武将
            var updatedSamurai = await samuraiService.AddSamuraiExperience(targetSamuraiId, totalExp);

            if (updatedSamurai == null)
            {
                return StatusCode(500, "Failed to add experience to the samurai.");
            }

            // 删除经验武将
            await samuraiService.DeleteSamuraisAsync(expSamuraiIds);

            var hpPool = await hpPoolService.GetHpPoolAsync(playerId);

            // 检查武将是否升级,如果升级了，则更新武将的当前血量
            if (updatedSamurai.Level != targetSamuraiLevel)
            {
                //从hpPool数据库中补充HP给武将
                var maxHp = tiktokConfigService.FormulaMaxHpByLevel(updatedSamurai.Level);
                if (updatedSamurai.CurHp < maxHp)
                {
                    var offset = maxHp - updatedSamurai.CurHp;
                    //检查hpPool中是否有足够的血量
                    
                    var finalValue = 0;
                    if (hpPool != null && hpPool.Hp >= offset)
                    {
                        finalValue = offset;
                    }
                    else if (hpPool != null && hpPool.Hp < offset)
                    {
                        finalValue = hpPool.Hp;
                    }

                    // 从hpPool中扣除血量
                    var result = await hpPoolService.SubtractHpPoolAsync(playerId, finalValue);
                    // 更新武将的当前血量
                    updatedSamurai.CurHp += finalValue;
                    //保存更新后的武将信息
                    updatedSamurai = await samuraiService.UpdateSamuraiHpAsync(updatedSamurai.Id, updatedSamurai.CurHp);
                }
            }

            // 返回更新后的武将信息 samuraiDTO
            var samuraiDTO = new SamuraiDTO
            {
                Id = updatedSamurai.Id,

                Level = tiktokConfigService.GetFormulaLevel(updatedSamurai.Experience),
                Experience = updatedSamurai.Experience,
                CurHp = updatedSamurai.CurHp,
                BusinessId = updatedSamurai.BusinessId,
                SoldierBusinessId = updatedSamurai.SoldierBusinessId,
            };

            //构建hpPoolDTO
            var hpPoolDTO = new HpPoolDTO
            {
                Hp = hpPool?.Hp ?? 0,
                MaxHp = hpPool?.MaxHp ?? tiktokConfigService.GetDefaultHpPoolMaxHp()
            };

            ////获取所有武将信息，客户端需要更新
            //var allSamurais = await samuraiService.GetAllSamuraiAsync(playerId);
            //var samuraiDTOs = allSamurais.Select(s => new SamuraiDTO
            //{
            //    Id = s.Id,
            //    BusinessId = s.BusinessId,
            //    SoldierBusinessId = s.SoldierBusinessId,
            //    Level = s.Level,
            //    Experience = s.Experience,
            //    CurHp = s.CurHp
            //}).ToList();

            //返回ResponseAddSamuraiExp
            var response = new ResponseAddSamuraiExp
            {
                SamuraiDTO = samuraiDTO,
                HpPoolDTO = hpPoolDTO,
                //AllSamuraiDTOs = samuraiDTOs
            };
            return Ok(response);
        }
    }
}