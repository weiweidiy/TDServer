using System.Runtime.CompilerServices;

namespace TiktokGame2Server.Others
{
    public interface IAttributeService
    {
        int GetPower();
        int GetDef();
        int GetIntel();
        int GetSpeed();
        int GetLevel();
        int GetSex();

        int GetHp();
        int GetMaxHp();
        int GetAttack();
        int GetDefence();
    }
}
