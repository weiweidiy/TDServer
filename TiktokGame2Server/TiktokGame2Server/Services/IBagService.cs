
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IBagService
    {
        /// <summary>
        /// 获取玩家的所有背包数据
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<List<BagSlot>> GetAllBagSlotsAsync(int playerId);

        /// <summary>
        /// 解锁一个空的背包格子
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<BagSlot> AddBagSlotAsync(int playerId);

        /// <summary>
        /// 解锁一批空的背包格子
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<BagSlot>> AddBagSlotsAsync(int playerId, int count);

        /// <summary>
        /// 获取没有Item的背包slot
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="bagSlotId"></param>
        /// <returns></returns>
        Task<BagSlot?> GetEmptyBagSlotAsync(int playerId);

        /// <summary>
        /// 获取指定物品 在背包中的所有实例
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="itemBusinessId"></param>
        /// <returns></returns>
        Task<List<BagItem>> GetBagItemsAsync(int playerId, string itemBusinessId);

        /// <summary>
        /// 往背包slot中添加物品
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="itemBusinessId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<BagItem> AddItemToBagSlotAsync(int playerId, string itemBusinessId, int count);

        /// <summary>
        /// 移除指定数量的物品
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<BagSlot> RemoveItemFromBagSlotAsync(int playerId, string itemBusinessId, int count);


        //Task<int> QueryBagItemId(string uid, int playerId);

        //Task<string> QueryBagItemUid(int itemId, int playerId);


    }
}
