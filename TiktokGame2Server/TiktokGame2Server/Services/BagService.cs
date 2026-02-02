
using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class BagService : IBagService
    {
        private readonly MyDbContext _dbContext;
        private readonly TiktokConfigManager tiktokConfigService;
        public BagService(MyDbContext dbContext, TiktokConfigManager tiktokConfigService)
        {
            _dbContext = dbContext;
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }

        /// <summary>
        /// 获取玩家的所有背包格子
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Task<List<BagSlot>> GetAllBagSlotsAsync(int playerId)
        {
            // Fetch all bags for the player from the database
            return _dbContext.BagSlots
                .Where(b => b.PlayerId == playerId)
                .Include(b => b.BagItem) // Include related Item entity
                .ToListAsync();
        }

        /// <summary>
        /// 添加一个空的背包格子
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Task<BagSlot> AddBagSlotAsync(int playerId)
        {
            // Create a new bag for the player and save it to the database
            var newBag = new BagSlot
            {
                PlayerId = playerId,
            };
            _dbContext.BagSlots.Add(newBag);
            return _dbContext.SaveChangesAsync().ContinueWith(_ => newBag);

        }

        /// <summary>
        /// 添加一批空的背包格子
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Task<List<BagSlot>> AddBagSlotsAsync(int playerId, int count)
        {
            // Create multiple new bags for the player and save them to the database
            var newBags = new List<BagSlot>();
            for (int i = 0; i < count; i++)
            {
                var newBag = new BagSlot
                {
                    PlayerId = playerId,
                };
                newBags.Add(newBag);
            }
            _dbContext.BagSlots.AddRange(newBags);
            return _dbContext.SaveChangesAsync().ContinueWith(_ => newBags);

        }


        /// <summary>
        /// 获取没有Item的背包slot
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Task<BagSlot?> GetEmptyBagSlotAsync(int playerId)
        {
            // Fetch an empty bag slot for the player from the database
            return _dbContext.BagSlots
                .Where(b => b.PlayerId == playerId && b.ItemId == null)
                .FirstOrDefaultAsync();

        }

        /// <summary>
        /// 获取指定物品 在背包中的所有实例
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="itemBusinessId"></param>
        /// <returns></returns>
        public Task<List<BagItem>> GetBagItemsAsync(int playerId, string itemBusinessId)
        {
            // Fetch all bag items for the player with the specified item business ID
            return _dbContext.BagItems
                .Where(bi => bi.PlayerId == playerId && bi.ItemBusinessId == itemBusinessId)
                .ToListAsync();
        }

        /// <summary>
        /// 添加物品到背包slot中
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="itemBusinessId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BagItem> AddItemToBagSlotAsync(int playerId, string itemBusinessId, int count)
        {
            //如果找到相同itemBusinessId的道具，且没有满，则该道具数量+count，如果已经满了，则查看空的格子，如果存在则放入空的BagSlot
            var items = await GetBagItemsAsync(playerId, itemBusinessId);
            if(items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item.Count + count < tiktokConfigService.GetItemMaxCount(item.ItemBusinessId))
                    {
                        item.Count += count;
                        //更新道具
                        _dbContext.BagItems.Update(item);
                        await _dbContext.SaveChangesAsync();
                        return item;
                    }
                }
            }

            //如果没有找到相同itemBusinessId的道具，或者所有的道具都满了，则查看空的BagSlot
            var emptyBagSlot = await GetEmptyBagSlotAsync(playerId);
            if (emptyBagSlot != null)
            {
                //如果存在空的BagSlot，则创建新的BagItem并放入空的BagSlot
                var newItem = new BagItem
                {
                    PlayerId = playerId,
                    ItemBusinessId = itemBusinessId,
                    Count = count,
                    BagSlotId = emptyBagSlot.Id
                };
                //将新道具添加到数据库
                _dbContext.BagItems.Add(newItem);
                emptyBagSlot.ItemId = newItem.Id; // 更新BagSlot的ItemId
                _dbContext.BagSlots.Update(emptyBagSlot); // 更新BagSlot
                await _dbContext.SaveChangesAsync();
                return newItem;
            }

            //如果没有空的BagSlot，则返回背包已满的错误
            throw new InvalidOperationException("Bag is full, cannot add new item.");
        }

        /// <summary>
        /// 删除指定数量的物品
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BagSlot> RemoveItemFromBagSlotAsync(int playerId, string itemBusinessId, int count)
        {
            // 获取玩家背包中所有该物品的实例，按数量升序（优先消耗数量少的）
            var items = await GetBagItemsAsync(playerId, itemBusinessId);
            if (items == null || items.Count == 0)
                throw new InvalidOperationException("背包中没有该物品");

            int remaining = count;
            BagSlot? lastUpdatedSlot = null;

            foreach (var item in items.OrderBy(i => i.Count))
            {
                if (remaining <= 0)
                    break;

                if (item.Count > remaining)
                {
                    // 只减少部分数量
                    item.Count -= remaining;
                    _dbContext.BagItems.Update(item);
                    await _dbContext.SaveChangesAsync();
                    lastUpdatedSlot = await _dbContext.BagSlots.FindAsync(item.BagSlotId);
                    break;
                }
                else
                {
                    // 移除整个BagItem
                    remaining -= item.Count;
                    var bagSlot = await _dbContext.BagSlots.FindAsync(item.BagSlotId);
                    if (bagSlot != null)
                    {
                        bagSlot.ItemId = null;
                        _dbContext.BagSlots.Update(bagSlot);
                        lastUpdatedSlot = bagSlot;
                    }
                    _dbContext.BagItems.Remove(item);
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (remaining > 0)
                throw new InvalidOperationException("物品数量不足，无法移除指定数量");

            // 返回最后一个被更新的BagSlot（可根据实际需求调整返回值）
            if (lastUpdatedSlot == null)
                throw new InvalidOperationException("未找到被更新的背包格子");

            return lastUpdatedSlot;

        }
    }
}
