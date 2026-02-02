using Game.Share;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Hubs;

namespace TiktokGame2Server.Others
{
    public class SamuraiService : ISamuraiService
    {
        private readonly MyDbContext _dbContext;
        private readonly TiktokConfigManager tiktokConfigService;
        //private readonly IHubContext<GameHub> _hubContext;
        public SamuraiService(MyDbContext dbContext, TiktokConfigManager tiktokConfigService/*, IHubContext<GameHub> hubContext*/)
        {
            _dbContext = dbContext;
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
            //_hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }


        /// <summary>
        /// 获取玩家的所有武士
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<Samurai>> GetAllSamuraiAsync(int playerId)
        {
            return await _dbContext.Samurais.Where(s => s.PlayerId == playerId).ToListAsync();
        }

        /// <summary>
        /// 获取指定武士
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public Task<Samurai?> GetSamuraiAsync(int samuraiId)
        {
            return _dbContext.Samurais.FirstOrDefaultAsync(s => s.Id == samuraiId);

        }

        public Task<List<Samurai>> GetSamuraisAsync(List<int> samuraiIds)
        {
            return _dbContext.Samurais
                .Where(s => samuraiIds.Contains(s.Id))
                .ToListAsync();

        }

        /// <summary>
        /// 删除指定武士
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public Task<bool> DeleteSamuraiAsync(int samuraiId)
        {
            var samurai = _dbContext.Samurais.Find(samuraiId);
            if (samurai == null)
                return Task.FromResult(false);
            _dbContext.Samurais.Remove(samurai);




            return _dbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);

        }


        public Task DeleteSamuraisAsync(List<int> expSamuraiIds)
        {
            var samurais = _dbContext.Samurais.Where(s => expSamuraiIds.Contains(s.Id));
            _dbContext.Samurais.RemoveRange(samurais);
            return _dbContext.SaveChangesAsync();

        }

        /// <summary>
        /// 新添加一个武士
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Samurai> AddSamuraiAsync(string samuraiBusinessId, string soldierUid, int playerId)
        {
            if (!CheckUid(samuraiBusinessId))
                throw new ArgumentException($"武士 {samuraiBusinessId} 配置数据不存在或无效。");

            var samurai = new Samurai
            {
                BusinessId = samuraiBusinessId,
                PlayerId = playerId,
                SoldierBusinessId = soldierUid,
                CurHp = tiktokConfigService.FormulaMaxHpByLevel(1),//默认1级
            };
            _dbContext.Samurais.Add(samurai);
            await _dbContext.SaveChangesAsync();

            //PushNewSamuraiNtf(samurai.Id, samurai.BusinessId, samurai.Level, samurai.Experience, samurai.CurHp, samurai.SoldierBusinessId, samurai.PlayerId);

            return samurai;
        }

        public async Task<List<Samurai>> AddSamuraisAsync(List<string> samuraiBusinessIds, List<string> soldierUids, int playerId)
        {
            if (samuraiBusinessIds.Count != soldierUids.Count)
                throw new ArgumentException("武士的业务ID和士兵UID数量不匹配。");

            var samurais = new List<Samurai>();
            for (int i = 0; i < samuraiBusinessIds.Count; i++)
            {
                var samurai = await AddSamuraiAsync(samuraiBusinessIds[i], soldierUids[i], playerId);
                samurais.Add(samurai);
            }
            return samurais;
        }

        /// <summary>
        /// 更新武士的血量
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <param name="curHp"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Samurai> UpdateSamuraiCurHp(int samuraiId, int curHp)
        {
            var samurai = await _dbContext.Samurais.FindAsync(samuraiId);
            if (samurai == null)
                throw new ArgumentException($"武士 {samuraiId} 不存在。");
            samurai.CurHp = curHp;
            _dbContext.Samurais.Update(samurai);
            await _dbContext.SaveChangesAsync();
            return samurai;
        }

        /// <summary>
        /// 增加武士的经验值
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Samurai> AddSamuraiExperience(int samuraiId, int experience)
        {
            var samurai = await _dbContext.Samurais.FindAsync(samuraiId);
            if (samurai == null)
                throw new ArgumentException($"武士 {samuraiId} 不存在。");
            samurai.Experience += experience;
            //重新计算等级
            var level = tiktokConfigService.GetFormulaLevel(samurai.Experience);
            samurai.Level = level;

            _dbContext.Samurais.Update(samurai);
            await _dbContext.SaveChangesAsync();


            

            return samurai;
        }


        bool CheckUid(string samuraiBusinessId)
        {
            // 检查 samuraiBusinessId 是否符合预期格式
            // 这里可以根据实际情况进行验证
            return !string.IsNullOrEmpty(samuraiBusinessId) && tiktokConfigService.IsValidSamurai(samuraiBusinessId);
        }

        public Task<Samurai> UpdateSamuraiHpAsync(int samuraiId, int curHp)
        {
            var samurai = _dbContext.Samurais.Find(samuraiId);
            if (samurai == null)
                throw new ArgumentException($"武士 {samuraiId} 不存在。");
            samurai.CurHp = curHp;
            _dbContext.Samurais.Update(samurai);
            return _dbContext.SaveChangesAsync().ContinueWith(t => samurai);

        }

        public Task<bool> CheckSamurais(List<int> samuraisIds, int playerId)
        {
            return _dbContext.Samurais
                .Where(s => samuraisIds.Contains(s.Id) && s.PlayerId == playerId)
                .CountAsync()
                .ContinueWith(t => t.Result == samuraisIds.Count);


        }

        //void PushNewSamuraiNtf(int id, string businessId, int level, int exp, int curHp, string soldierBusinessId, int playerId)
        //{
        //    // 先返回，后推送
        //    _ = Task.Run(async () =>
        //    {

        //        var ntf = new SamuraiUpdateNtf()
        //        {
        //            Uid = Guid.NewGuid().ToString(),
        //            TypeId = (int)ProtocolType.SamuraiUpdateNtf,
        //            NewSamuraiDTOs = new List<SamuraiDTO>
        //            {
        //                new SamuraiDTO
        //                {
        //                    Id = id,
        //                    BusinessId = businessId,
        //                    Level = level,
        //                    Experience = exp,
        //                    CurHp = curHp,
        //                    SoldierBusinessId = soldierBusinessId,
        //                }
        //            }
        //        };
        //        var bytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ntf));
        //        try
        //        {
        //            await _hubContext.Clients.User(playerId.ToString()).SendAsync("ReceiveBinary", bytes);
        //        }
        //        catch (Exception ex)
        //        {
        //            // 可选：记录日志
        //            Console.WriteLine("");
        //        }
        //    });
        //}


    }
}
