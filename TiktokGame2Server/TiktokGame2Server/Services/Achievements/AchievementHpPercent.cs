
using Game.Share;

namespace TiktokGame2Server.Others
{
    public class AchievementHpPercent : AchievementBase
    {
        public AchievementHpPercent(float[] args) : base(args)
        {
            if (args.Length < 1)
                throw new ArgumentException("参数数量不正确 " + typeof(AchievementHpPercent));
        }

        float GetHpPercent()
        {
            return GetArg(0);
        }

        public override bool IsCompleted(string playerUid, TiktokJCombatTurnBasedReportData reportData)
        {
            var formation = reportData.FormationData[playerUid];
            float allCurHp = 0;
            float allMaxHp = 0;
            foreach(var unit in formation)
            {
                allCurHp += unit.CurHp;
                allMaxHp += unit.MaxHp;
            }

            return allCurHp / allMaxHp >= GetHpPercent();
        }
    }
}
