namespace TiktokGame2Server.Others
{
    public interface IGuideService
    {
        /// <summary>
        /// 获取当前玩家已完成的引导步骤
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<string> GetCurrentCompletedGuideStepAsync(int playerId);

        /// <summary>
        /// 完成指定的引导步骤
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="guideBusinessId"></param>
        /// <returns></returns>
        Task<string> CompleteGuideStepAsync(int playerId, string guideBusinessId);

        /// <summary>
        /// 是否已经完成指定的引导步骤
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="guideBusinessId"></param>
        /// <returns></returns>
        Task<bool> IsGuideStepCompletedAsync(int playerId, string guideBusinessId);
    }
}

