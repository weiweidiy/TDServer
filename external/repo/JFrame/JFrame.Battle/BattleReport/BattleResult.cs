namespace JFramework
{
    /// <summary>
    /// 战斗胜负结果
    /// </summary>
    public class BattleResult
    {
        BattleTeam leftTeam;
        BattleTeam rightTeam;

        public BattleResult(BattleTeam left, BattleTeam right)
        {
            leftTeam = left;
            rightTeam = right;
        }

        public bool IsOver()
        {
            return leftTeam.IsAllDead() || rightTeam.IsAllDead();
        }

        public BattleTeam GetWinner()
        {
            return rightTeam.IsAllDead() ? leftTeam : rightTeam;
        }
    }
}