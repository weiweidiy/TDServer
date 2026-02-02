namespace JFramework
{
    /// <summary>
    /// 战斗胜负结果
    /// </summary>
    public class CombatJudge
    {
        CommonCombatTeam leftTeam;
        CommonCombatTeam rightTeam;

        public CombatJudge(CommonCombatTeam left, CommonCombatTeam right)
        {
            leftTeam = left;
            rightTeam = right;
        }

        public bool IsOver()
        {
            return leftTeam.IsAllDead() || rightTeam.IsAllDead();
        }

        public CommonCombatTeam GetWinner()
        {
            return rightTeam.IsAllDead() ? leftTeam : rightTeam;
        }
    }
}