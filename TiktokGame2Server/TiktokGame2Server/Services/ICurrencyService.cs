
using Game.Share;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetAllCurrencies(int playerId);
        Task<Currency> GetCurrency(int playerId, CurrencyType currencyType);
        Task<Currency> AddCurrency(int playerId, CurrencyType currencyType, int amount);
        Task<Currency> SpendCurrency(int playerId, CurrencyType currencyType, int amount);

        //接口：判断玩家是否有足够的货币
        Task<bool> HasEnoughCurrency(int playerId, CurrencyType currencyType, int amount);
    }
}
