using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface ILevelNodesService
    {
        /// <summary>
        /// 获取指定玩家的关卡节点
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<LevelNode?> GetLevelNodeAsync(string levelNodeBusinessId, int playerId);

        /// <summary>
        /// 获取指定玩家的所有关卡节点
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<List<LevelNode>?> GetLevelNodesAsync(int playerId);

        /// <summary>
        /// 指定节点胜利（玩家通过该节点）
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<LevelNode> LevelNodeVictoryAsync(string nodeId, int playerId);
        Task<LevelNode> UpdateLevelNodeProcessAsync(string levelNodeBusinessId, int playerId, int process);
    }
}
