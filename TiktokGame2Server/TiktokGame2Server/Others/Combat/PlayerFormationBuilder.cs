using Game.Share;
using JFramework;
using JFramework.Game;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class PlayerFormationBuilder : IJCombatFormationBuilder
    {
        List<FormationDeploy> playerFormation;

        IJCombatContext context;

        TiktokConfigManager tiktokConfigService;

        public PlayerFormationBuilder(List<FormationDeploy> playerFormation,  TiktokConfigManager tiktokConfigService, IJCombatContext context)
        {
            this.playerFormation = playerFormation ?? throw new ArgumentNullException(nameof(playerFormation));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }

        public List<JCombatFormationInfo> Build()
        {
            return playerFormation.Select(f =>
            {
                var s = new Samurai()
                {
                    SoldierBusinessId = f.Samurai.SoldierBusinessId,
                    CurHp = f.Samurai.CurHp,
                    BusinessId = f.Samurai.BusinessId,
                    Experience = f.Samurai.Experience,
                    Level = f.Samurai.Level,
                };
                var builder = new PlayerUnitBuilder(new TiktokAttributesBuilder(new PlayerAttributeService(s, tiktokConfigService)), new PlayerActionsBuilder(f.Samurai,tiktokConfigService, context), f.Samurai, tiktokConfigService);
                var info = new JCombatFormationInfo
                {
                    Point = f.FormationPoint,
                    UnitInfo = builder.Build()
                };

                return info;
            }).ToList();
        }
    }


    public class TiktokJCombatUnitInfo : JCombatUnitInfo
    {
        public required string SamuraiBusinessId { get; set; }
        public required string SoldierBusinessId { get; set; }
        public required SoldierType SoldierType { get; set; }
    }
}



//public class FormationInfo
//{
//    public int FormationPoints { get; set; }

//    public required JCombatUnitInfo UnitInfo { get; set; }
//}

///// <summary>
///// 获取玩家武士在阵型中的坐标点位
///// </summary>
///// <returns></returns>
//Func<string, int> CreateFormationPointDelegate(int formationType)
//{
//    // 从formation中获取武士的点位
//    return (unitUid) => // to do: 需要所有的战斗单位（包括NPC）
//    {
//        //从atkFormations中获取对应的阵型点位
//        if (lstFormationQuery == null || lstFormationQuery.Count == 0)
//        {
//            throw new Exception("没有可用的阵型");
//        }
//        var formation = lstFormationQuery.FirstOrDefault(f =>  f.UnitInfo.Uid == unitUid);
//        return formation?.Point ?? -1; // 如果没有找到对应的阵型点位，返回-1
//    };
//}
