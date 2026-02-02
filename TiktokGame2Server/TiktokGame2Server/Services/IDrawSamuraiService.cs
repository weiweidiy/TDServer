using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IDrawSamuraiService
    {
        Task<Samurai> DrawSamurai(int playerId);

        Task<List<Samurai>> DrawSamurais(int playerId, int count);
    }
}
