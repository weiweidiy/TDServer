using Game.Share;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TiktokGame2Server.Hubs;

namespace TiktokGame2Server.Others
{
    public class TiktokNotifyService : INotifyService
    {
        private readonly IHubContext<GameHub> _hubContext;
        private readonly ICurrencyService currencyService;
        private readonly ISamuraiService samuraiService;
        private readonly IFormationDeployService formationDeployService;
        public TiktokNotifyService(IHubContext<GameHub> hubContext, ICurrencyService currencyService, ISamuraiService samuraiService,
                IFormationDeployService formationDeployService)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            this.currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            this.samuraiService = samuraiService ?? throw new ArgumentNullException(nameof(samuraiService));
            this.formationDeployService = formationDeployService ?? throw new ArgumentNullException(nameof(formationDeployService));
        }

        public Task SendNotificationAsync(int playerId, object message)
        {
            // 先返回，后推送
            return Task.Run(async () =>
            {

                var bytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                try
                {
                    //Console.WriteLine($"推送货币更新通知给玩家 {playerId}: {currencyType} = {count}");
                    await _hubContext.Clients.User(playerId.ToString()).SendAsync("ReceiveBinary", bytes);
                }
                catch (Exception ex)
                {
                    // 可选：记录日志
                    Console.WriteLine($"ERROR: 推送货币更新通知给玩家 {playerId}:");
                }
            });
        }

        public Task SendGlobalNotificationAsync(object message)
        {
            // 先返回，后推送
            return Task.Run(async () =>
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                try
                {
                    //Console.WriteLine($"推送货币更新通知给玩家 {playerId}: {currencyType} = {count}");
                    await _hubContext.Clients.All.SendAsync("ReceiveBinary", bytes);
                }
                catch (Exception ex)
                {
                    // 可选：记录日志
                    Console.WriteLine($"ERROR: 推送全局通知:");
                }
            });
        }


        /// <summary>
        /// 血池更新了
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="hpPoolDTO"></param>
        /// <returns></returns>
        public Task NotifyHpPoolUpdate(int playerId, HpPoolDTO hpPoolDTO)
        {
            if (hpPoolDTO == null)
                return Task.CompletedTask;
            // 构造数据更新的Ntf对象
            var hpPoolUpdateNtf = new HpPoolUpdateNtf
            {
                HpPoolDTO = hpPoolDTO
            };
            return SendNotificationAsync(playerId, hpPoolUpdateNtf);
        }

        /// <summary>
        /// 通知布阵信息更新
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task NotifyFormationDeployUpdate(int playerId , List<FormationDeployDTO> dtos)
        {
            // 构造数据更新的Ntf对象
            var formationDeployUpdateNtf = new FormationDeployUpdateNtf
            {
                FormationDeployDTOs = dtos
            };
            await SendNotificationAsync(playerId, formationDeployUpdateNtf);
        }

        /// <summary>
        /// 通知阵型发生变更
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="formationDTO"></param>
        /// <returns></returns>
        public async Task NotifyFormationUpdate(int playerId, FormationDTO formationDTO)
        {
            var formationUpdateNtf = new FormationUpdateNtf
            {
                FormationDTO = formationDTO
            };
            await SendNotificationAsync(playerId, formationUpdateNtf);
        }

        /// <summary>
        /// 通知关卡节点数据更新
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="levelNodeDTO"></param>
        /// <returns></returns>
        public async Task NotifyLevelNodeUpdate(int playerId, LevelNodeDTO levelNodeDTO)
        {
            if (levelNodeDTO == null)
                return ;

            // 构造数据更新的Ntf对象
            var levelNodeUpdateNtf = new LevelNodeUpdateNtf
            {
                LevelNodeDTO = levelNodeDTO
            };
            await SendNotificationAsync(playerId, levelNodeUpdateNtf);

        }

        /// <summary>
        /// 通知货币数据更新
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task NotifyCurrenciesUpdate(int playerId)
        {
            var currentCurrencies = await currencyService.GetAllCurrencies(playerId);
            //currentCurrencies 转成DTO
            var currencyDTOs = currentCurrencies.Select(c => new CurrencyDTO
            {
                CurrencyType = c.CurrencyType,
                Count = c.Count
            }).ToList();
            //构造数据更新的Ntf对象
            await NotifyCurrenciesUpdate(playerId, currencyDTOs);
        }

        public async Task NotifyCurrenciesUpdate(int playerId, List<CurrencyDTO> currencyDTOs)
        {
            var currencyNtf = new CurrencyUpdateNtf
            {
                CurrencyDTOs = currencyDTOs
            };
            await SendNotificationAsync(playerId, currencyNtf);
        }

        public async Task NotifyCurrenciesUpdate(int playerId, CurrencyDTO currencyDTO)
        {
            var currencyNtf = new CurrencyUpdateNtf
            {
                CurrencyDTOs = new List<CurrencyDTO> { currencyDTO }
            };
            await SendNotificationAsync(playerId, currencyNtf);
        }


        /// <summary>
        /// 通知武士数据更新
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task NotifySamuraisUpdate(int playerId)
        {
            // 获取玩家的所有武士
            var samurais = await samuraiService.GetAllSamuraiAsync(playerId);
            // 将武士转换为DTO
            var samuraiDTOs = samurais.Select(s => new SamuraiDTO
            {
                Id = s.Id,         
                Level = s.Level,
                SoldierBusinessId = s.SoldierBusinessId,
                BusinessId = s.BusinessId,
                CurHp = s.CurHp,
                Experience = s.Experience,

            }).ToList();


            var samuraiUpdateNtf = new SamuraiUpdateNtf
            {
                SamuraiDTOs = samuraiDTOs
            };
            await SendNotificationAsync(playerId, samuraiUpdateNtf);
        }

        public async Task NotifyBuildingUpdate(int playerId, BuildingDTO buildingDTO)
        {
            if (buildingDTO == null)
                return;
            var buildingUpdateNtf = new BuildingUpdateNtf
            {
                BuildingDTO = buildingDTO
            };
            await SendNotificationAsync(playerId, buildingUpdateNtf);

        }

        public async Task NotifyGuideStepUpdate(int playerId, string currentGuideStepBusinessId)
        {
            var guideStepUpdateNtf = new CurrentGuideStepUpdateNtf
            {
                CurrentGuideStepBusinessId = currentGuideStepBusinessId
            };
            await SendNotificationAsync(playerId, guideStepUpdateNtf);

        }

        public async Task SendGlobalAnnouncement(string playerName, SamuraiDTO samuraiDTO)
        {
            var rareSamuraiGetNtf = new RareSamuraiGetNtf
            {
                PlayerName = playerName,
                SamuraiDTO = samuraiDTO
            };

            await SendGlobalNotificationAsync(rareSamuraiGetNtf);
        }
    }
}
