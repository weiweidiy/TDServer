using Game.Share;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others; // 假设TokenService在此命名空间

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(MyDbContext myDbContext
        , ITokenService tokenService
        , IPlayerService playerService
        , ILevelNodesService levelNodeService
        , ISamuraiService samuraiService
        , IFormationDeployService formationDeployService
        , TiktokConfigManager tiktokConfigService
        , IBagService bagService
        , ICurrencyService currencyService
        , IFormationService formationService
        , IBuildingService buildingService
        , IGuideService guideService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        ITokenService tokenService = tokenService;
        IPlayerService playerService = playerService;
        ILevelNodesService levelNodeService = levelNodeService;
        ISamuraiService samuraiService = samuraiService;
        IFormationDeployService formationDeployService = formationDeployService;
        TiktokConfigManager tiktokConfigService = tiktokConfigService;
        IBagService bagService = bagService;
        ICurrencyService currencyService = currencyService;
        IFormationService formationService = formationService;
        IBuildingService buildingService = buildingService;
        IGuideService guideService = guideService;

        [HttpPost("EnterGame")]
        public async Task<ActionResult<ResponseGame>> EnterGame()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                return BadRequest("Token不能为空");
            }
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            if (string.IsNullOrEmpty(accountUid))
            {
                return BadRequest("账号Uid不能为空");
            }

            // 检查账号是否存在
            var account = await myDbContext.Accounts.Include(a => a.Player).Where(a => a.Uid == accountUid).FirstOrDefaultAsync();
            if (account == null)
            {
                return NotFound("账号不存在");
            }


            //游戏登录数据汇总对象
            var gameDto = new ResponseGame
            {
                PlayerDTO = new PlayerDTO
                {
                    Uid = playerUid,
                    Username = account.Player?.Name
                },
                LevelNodesDTO = await GetLevelNodeDTOs(playerId),
                SamuraisDTO = await GetSamuraiDTOs(playerId),
                AtkFormationDTO = await GetFormationDeployDTOs(playerId, tiktokConfigService.GetAtkFormationType()),
                //DefFormationDTO = await GetFormationDeployDTOs(playerId, tiktokConfigService.GetDefFormationType()),
                HpPoolDTO = await GetHpPoolDTO(playerId),
                CurrencyDTO = await GetCurrencyDTO(playerId),
                BagDTOs = await GetBagDTO(playerId),
                FormationDTOs = await GetFormationDTOs(playerId),
                BuildingDTOs = await GetBuildingDTOs(playerId),
                CurrentGuideBusinessId = await GetCurrentGuideBusinessId(playerId),
                ServerTime = GetServerTime()
            };

            stopwatch.Stop();
            Console.WriteLine($"EnterGame 耗时: {stopwatch.ElapsedMilliseconds} ms");
            return Ok(gameDto);
        }



        private async Task<List<FormationDTO>> GetFormationDTOs(int playerId)
        {
            var result = new List<FormationDTO>();

            //获取玩家的阵型
            var atkFormation = await formationService.GetFormationAsync(tiktokConfigService.GetAtkFormationType(), playerId);
            //如果是空，则创建一个默认的阵型
            if (atkFormation == null)
            {
                atkFormation = await formationService.AddFormationAsync(tiktokConfigService.GetAtkFormationType(), tiktokConfigService.GetDefaultFormationBusinessId(), playerId);
            }
            //如果atkFormation == null 返回异常
            if (atkFormation == null)
            {
                throw new Exception("获取玩家攻击阵型失败");
            }

            //将阵型转换为DTO
            var atkFormationDTO = new FormationDTO
            {
                FormationType = atkFormation.FormationType,
                FormationBusinessId = atkFormation.FormationBusinessId,
            };
            result.Add(atkFormationDTO);

            ////获取玩家的防御阵型
            //var defFormation = await formationDeployService.GetFormationAsync(tiktokConfigService.GetDefFormationType(), playerId);
            ////如果是空，则创建一个默认的阵型
            //if (defFormation == null)
            //{
            //    defFormation = await formationDeployService.AddFormationAsync(tiktokConfigService.GetDefFormationType(), tiktokConfigService.GetDefaultFormationBusinessId(), playerId);
            //}
            ////如果defFormation == null 返回异常
            //if (defFormation == null)
            //{
            //    throw new Exception("获取玩家防御阵型失败");
            //}
            ////将阵型转换为DTO
            //var defFormationDTO = new FormationDTO
            //{
            //    FormationType = defFormation.FormationType,
            //    FormationBusinessId = defFormation.FormationBusinessId,
            //};
            //result.Add(defFormationDTO);

            return result;
        }





        /// <summary>
        /// 获取玩家的关卡节点信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        async Task<List<LevelNodeDTO>> GetLevelNodeDTOs(int playerId)
        {
            // 获取玩家的关卡节点
            var levelNodes = await levelNodeService.GetLevelNodesAsync(playerId);
            var levelNodeDtos = levelNodes?.Select(n => new LevelNodeDTO
            {
                Id = n.Id,
                BusinessId = n.BusinessId,
                Process = n.Process,
            }).ToList();

            return levelNodeDtos ?? new List<LevelNodeDTO>();
        }

        /// <summary>
        /// 获取玩家的武士信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        async Task<List<SamuraiDTO>> GetSamuraiDTOs(int playerId)
        {
            //获取玩家samurai
            var samurais = await samuraiService.GetAllSamuraiAsync(playerId);
            if (samurais == null || samurais.Count == 0)
            {
                samurais = new List<Samurai>();
                var defaultBusinessIds = tiktokConfigService.GetDefaultSamuraiBusinessIds();
                foreach (var businessId in defaultBusinessIds)
                {
                    var defaultSamurai = await samuraiService.AddSamuraiAsync(businessId
                    , tiktokConfigService.GetDefaultSoldierBusinessId(businessId), playerId);
                    samurais.Add(defaultSamurai);
                }
                
            }
            var samuraisDTO = samurais?.Select(n => new SamuraiDTO
            {
                Id = n.Id,
                BusinessId = n.BusinessId,
                SoldierBusinessId = n.SoldierBusinessId,
                Level = n.Level,
                Experience = n.Experience,
                CurHp = n.CurHp,
            }).ToList();

            return samuraisDTO ?? new List<SamuraiDTO>();
        }

        async Task<List<CurrencyDTO>> GetCurrencyDTO(int playerId)
        {
            var result = new List<CurrencyDTO>();

            //获取玩家的货币
            var currencyCoin = await myDbContext.Currencies.FirstOrDefaultAsync(c => c.PlayerId == playerId && c.CurrencyType == CurrencyType.Coin);
            if (currencyCoin == null)
            {
                //创建一个新的货币
                currencyCoin = await currencyService.AddCurrency(playerId, CurrencyType.Coin, tiktokConfigService.GetDefaultCurrencyCoin());

            }

            //获取玩家的小判
            var currencyPan = await myDbContext.Currencies.FirstOrDefaultAsync(c => c.PlayerId == playerId && c.CurrencyType == CurrencyType.Pan);
            if (currencyPan == null)
            {
                //创建一个新的货币
                currencyPan = await currencyService.AddCurrency(playerId, CurrencyType.Pan, tiktokConfigService.GetDefaultCurrencyPan());

            }



            // 将货币转换为DTO
            result.Add(new CurrencyDTO
            {
                CurrencyType = CurrencyType.Coin,
                Count = currencyCoin.Count,
            });
            result.Add(new CurrencyDTO
            {
                CurrencyType = CurrencyType.Pan,
                Count = currencyPan.Count,
            });

            return result;

        }

        async Task<HpPoolDTO> GetHpPoolDTO(int playerId)
        {
            //获取玩家的生命池
            var hpPool = await myDbContext.HpPools.FirstOrDefaultAsync(hp => hp.PlayerId == playerId);
            if (hpPool == null)
            {
                //创建一个新的生命池
                hpPool = new HpPool { PlayerId = playerId, Hp = tiktokConfigService.GetDefaultHpPoolHp(), MaxHp = tiktokConfigService.GetDefaultHpPoolMaxHp() };
                myDbContext.HpPools.Add(hpPool);
                myDbContext.SaveChanges();
            }

            // 将HpPool转换为DTO
            var hpPoolDTO = new HpPoolDTO
            {
                Hp = hpPool.Hp,
                MaxHp = hpPool.MaxHp,
            };

            return hpPoolDTO;

        }


        async Task<List<BagSlotDTO>> GetBagDTO(int playerId)
        {
            //获取玩家的背包
            var bagSlots = await bagService.GetAllBagSlotsAsync(playerId);
            if (bagSlots == null || bagSlots.Count == 0)
            {
                //创建一个默认的背包槽
                bagSlots = await bagService.AddBagSlotsAsync(playerId, tiktokConfigService.GetDefaultBagSlotCount());
            }

            // 简单映射 BagSlot -> BagSlotDTO
            var bagSlotDTOs = bagSlots.Select(slot => new BagSlotDTO
            {
                Id = slot.Id,
                ItemDTO = slot.ItemId.HasValue
                    ? new ItemDTO
                    {
                        Id = slot.ItemId.Value,
                        ItemBusinessId = slot.BagItem?.ItemBusinessId,
                        Count = slot.BagItem?.Count ?? 0
                    }
                    : null,
            }).ToList();

            return bagSlotDTOs;

        }

        /// <summary>
        /// 获取玩家的阵型信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="formationType"></param>
        /// <returns></returns>
        async Task<List<FormationDeployDTO>> GetFormationDeployDTOs(int playerId, FormationType formationType)
        {
            //获取玩家formation
            var formations = await formationDeployService.GetFormationDeployAsync(formationType, playerId);
            if (formations == null || formations.Count == 0)
            {
                formations = new List<FormationDeploy>();

                //获取所有武士
                var samurais = await samuraiService.GetAllSamuraiAsync(playerId);
                var first = samurais.FirstOrDefault();
                if (first == null)
                {
                    var samuraiBusinessIds = tiktokConfigService.GetDefaultDeplySamuraiBusinessId();
                    first = await samuraiService.AddSamuraiAsync(samuraiBusinessIds
                            , tiktokConfigService.GetDefaultSoldierBusinessId(samuraiBusinessIds), playerId);
                }

                var defaultFormation = await formationDeployService.AddOrUpdateFormationSamuraiAsync(formationType, tiktokConfigService.GetDefaultFormationPoint(), first.Id, playerId);
                formations.Add(defaultFormation);
            }

            // 将Formation转换为DTO
            var formationDTOs = formations.Select(f => new FormationDeployDTO
            {
                Id = f.Id,
                FormationType = f.FormationType,
                FormationPoint = f.FormationPoint,
                SamuraiId = f.SamuraiId,
            }).ToList();

            return formationDTOs ?? new List<FormationDeployDTO>();

        }

        private async Task<List<BuildingDTO>> GetBuildingDTOs(int playerId)
        {
            var buildings = await buildingService.GetAllBuildingsAsync(playerId);
            if (buildings == null || buildings.Count == 0)
            {
                buildings = await buildingService.AddBuildingsAsync(tiktokConfigService.GetDefaultBuildingsBusinessId(), playerId);
            }

            var buildingDTOs = new List<BuildingDTO>();

            //遍历buildingDTOs，获取每个建筑的升级信息
            foreach (var building in buildings)
            {
                var upgradeSchedule = await buildingService.GetBuildingUpgradeScheduleAsync(building.Id, playerId);
                //将Building转换为DTO
                var buildingDTO = new BuildingDTO
                {
                    BusinessId = building.BusinessId,
                    Level = building.Level,
                    UpgradeEndTime = null,
                };
                if (upgradeSchedule != null)
                {
                    buildingDTO.UpgradeEndTime = upgradeSchedule.UpgradeEndAt;
                }

                buildingDTOs.Add(buildingDTO);
            }

            return buildingDTOs ?? new List<BuildingDTO>();
        }


        private async Task<string> GetCurrentGuideBusinessId(int playerId)
        {
            var currentGuideBusinessId = await guideService.GetCurrentCompletedGuideStepAsync(playerId);
            return currentGuideBusinessId ?? string.Empty;
        }

        private long GetServerTime()
        {
            //获取时间戳，单位毫秒
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}