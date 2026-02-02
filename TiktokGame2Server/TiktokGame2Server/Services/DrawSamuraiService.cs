using JFramework;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class DrawSamuraiService : IDrawSamuraiService
    {
        private readonly ISamuraiService samuraiService;
        TiktokConfigManager tiktokConfigService;
        //Utility utility = new Utility();
        public DrawSamuraiService(ISamuraiService samuraiService, TiktokConfigManager tiktokConfigService)
        {
            this.samuraiService = samuraiService ?? throw new ArgumentNullException(nameof(samuraiService));
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }
        public async Task<Samurai> DrawSamurai(int playerId)
        {
            var samuraiBusinessId = GetRandomSamuraiBusinessIdFromSamuraiDrawPool(); // This should be replaced with the actual business ID logic
            var soldierBusinessId = tiktokConfigService.GetDefaultSoldierBusinessId(samuraiBusinessId);

            var samurai = await samuraiService.AddSamuraiAsync(samuraiBusinessId, soldierBusinessId, playerId);

            // 广播消息到所有客户端
            //await hubContext.Clients.All.SendAsync("ReceiveDrawSamurai", new
            //{
            //    PlayerId = playerId,
            //    
            //    SamuraiBusinessId = samuraiBusinessId,
            //    
            //});
            return samurai;
        }

        private string GetRandomSamuraiBusinessIdFromSamuraiDrawPool()
        {
            var allSamuraiBusinessIds = tiktokConfigService.GetSamuraiDrawPool();
            return allSamuraiBusinessIds.ToList().GetRandomItems(1)[0];
        }

        public Task<List<Samurai>> DrawSamurais(int playerId, int count)
        {
            var businessIds = new List<string> { "1", "1", "2" }; 
            var soldierBusinessIds = new List<string> { "1", "1", "1" };
            return samuraiService.AddSamuraisAsync(businessIds, soldierBusinessIds, playerId);

        }
    }
}
