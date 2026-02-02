using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class GuideService : IGuideService
    {
        private readonly MyDbContext _dbContext;
        public GuideService(MyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<string> CompleteGuideStepAsync(int playerId, string guideBusinessId)
        {
            //完成指定的引导步骤，将guideBusinessId加入到数据库中，并返回当前已完成的最大步骤
            var guideProgress = _dbContext.GuideProgress.FirstOrDefault(g => g.PlayerId == playerId);
            if (guideProgress != null)
            {
                if (guideProgress.GuideBusinessId.Equals(guideBusinessId))
                {
                    //已经完成该步骤，直接返回当前业务ID
                    return Task.FromResult(guideProgress.GuideBusinessId);
                }
                guideProgress.GuideBusinessId = (guideBusinessId);
                _dbContext.GuideProgress.Update(guideProgress);
            }
            else
            {
                guideProgress = new GuideProgress
                {
                    PlayerId = playerId,
                    GuideBusinessId = guideBusinessId
                };
                _dbContext.GuideProgress.Add(guideProgress);
            }

            //保存更改
            _dbContext.SaveChanges();
            return Task.FromResult(guideProgress.GuideBusinessId);
        }

        public Task<string> GetCurrentCompletedGuideStepAsync(int playerId)
        {
            var guideProgress = _dbContext.GuideProgress.FirstOrDefault(g => g.PlayerId == playerId);
            if (guideProgress != null)
            {
                return Task.FromResult(guideProgress.GuideBusinessId);
            }
            return Task.FromResult(string.Empty);   

        }

        public Task<bool> IsGuideStepCompletedAsync(int playerId, string guideBusinessId)
        {
            var guideProgress = _dbContext.GuideProgress.FirstOrDefault(g => g.PlayerId == playerId);
            if (guideProgress != null)
            {
                return Task.FromResult(guideProgress.GuideBusinessId.Equals(guideBusinessId));
            }
            return Task.FromResult(false);

        }
    }
}

