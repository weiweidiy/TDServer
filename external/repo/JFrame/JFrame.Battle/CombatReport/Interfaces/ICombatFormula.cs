namespace JFramework
{
    public interface ICombatFormula
    {
        double GetHitValue(CombatExtraData extraData);

        bool IsHit(CombatExtraData extraData);
    }
}