using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class LevelNodeFormationBuilder : IJCombatFormationBuilder
    {
        TiktokConfigManager tiktokConfigService;
        string levelNodeBusinessId;
        IJCombatContext context;
        public LevelNodeFormationBuilder(string levelNodeBusinessId,  TiktokConfigManager tiktokConfigService, IJCombatContext context) 
        {
            this.tiktokConfigService = tiktokConfigService;
            this.levelNodeBusinessId = levelNodeBusinessId;
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public List<JCombatFormationInfo> Build()
        {
            //fake data
            var fakeResult = new List<JCombatFormationInfo>();

            var formationCfg = tiktokConfigService.GetLevelNodeFormation(levelNodeBusinessId);
            for(int i = 0; i < formationCfg.Length; i++)
            {
                var formationUnitBusinessId = formationCfg[i];
                if (formationUnitBusinessId == "0")
                    continue;

                var unitBuilder = new LevelNodeUnitBuilder(new TiktokAttributesBuilder(new FormationUnitAttributeService(formationUnitBusinessId, tiktokConfigService))
                        , new LevelNodeActionsBuilder(formationUnitBusinessId,tiktokConfigService, context)
                        , formationUnitBusinessId, tiktokConfigService);

                var formation = new JCombatFormationInfo();
                formation.Point = i ;
                formation.UnitInfo = unitBuilder.Build(); // unitBuilder.Build(int.Parse(formationCfg[i])); //这里是配置表中的id
                fakeResult.Add(formation);
            }

            return fakeResult;
        }
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
