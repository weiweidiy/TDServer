using Game.Share;
using JFramework;
using JFramework.Game;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics.Eventing.Reader;
using TiktokGame2Server.Entities;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    /// <summary>
    /// 副本节点的战斗服务
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="formationService"></param>
    /// <param name="tiktokConfigService"></param>
    public partial class LevelNodeCombatService(MyDbContext dbContext
        , IFormationDeployService formationService
        , TiktokConfigManager tiktokConfigService
        , IEvaluationService evaluationService) : ILevelNodeCombatService
    {
        private readonly MyDbContext _dbContext = dbContext;
        private readonly IFormationDeployService formationService = formationService;
        private readonly TiktokConfigManager tiktokConfigService = tiktokConfigService;
        private readonly IEvaluationService evaluationService = evaluationService;

        ///// <summary>
        ///// 阵型查询列表
        ///// </summary>
        //List<JCombatFormationInfo> lstFormationQuery = null;
        public async Task<TiktokJCombatTurnBasedReportData> GetReport(int playerId, string levelNodeBusinessId)
        {
            //从Player表获取玩家信息
            var player = await _dbContext.Players.FindAsync(playerId);
            var playerUid = player?.Uid;

            //创建阵型查询列表（包含玩家阵型和NPC阵型）
            var playerFormations = await formationService.GetFormationDeployAsync(tiktokConfigService.GetAtkFormationType(), playerId);
            if (playerFormations == null || playerFormations.Count == 0)
            {
                throw new Exception("玩家没有可用的阵型");
            }

            JCombatTurnBasedFrameRecorder frameRecorder;
            JCombatTurnBasedEventRecorder eventRecorder;
            //创建帧记录器
            frameRecorder = new JCombatTurnBasedFrameRecorder(19); //从0开始，共20回合
            eventRecorder = new JCombatTurnBasedEventRecorder(frameRecorder);

            var logger = new ConsoleLogger();

            IJCombatContext context = new TiktokJCombatContext(eventRecorder, logger);

            //构建双方阵型信息
            var playerFormationBuilder = new PlayerFormationBuilder(playerFormations,tiktokConfigService, context);
            var levelNodeFormationBuilder = new LevelNodeFormationBuilder(levelNodeBusinessId,tiktokConfigService, context);
            var playerFormationInfos = playerFormationBuilder.Build();
            var levelodeFormationInfos = levelNodeFormationBuilder.Build();

            //创建阵型点位查询构建器
            var lst = new List<JCombatFormationInfo>();
            lst.AddRange(playerFormationInfos);
            lst.AddRange(levelodeFormationInfos);
            var formationBuilder = new JCombatSeatFuncBuilder(lst);

            JCombatTurnBased turnbasedCombat;

            JCombatSeatBasedQuery jcombatQuery;
            JCombatTeam team1;
            JCombatTeam team2;

            JCombatTurnBasedActionSelector actionSelector;
            JCombatTurnBasedRunner combatRunner;
            IJCombatTurnBasedAttrNameQuery attrNameQuery;
            JCombatTurnBasedReportBuilder report;


            attrNameQuery = new TiktokAttrNameQuery();
            //查询工具，funcSeat 可以替换成 formationBuilder
            jcombatQuery = new JCombatSeatBasedQuery(formationBuilder, frameRecorder, attrNameQuery);
            jcombatQuery.SetLogger(logger);


            report = new TiktokJCombatTurnBasedReportBuilder(jcombatQuery);

            //创建玩家unit对象
            var playerUnits = new List<IJCombatUnit>();
            foreach (var formationInfo in playerFormationInfos)
            {
                //队伍1
                var unit1 = new JCombatTurnBasedUnit(formationInfo.UnitInfo, attrNameQuery, context);
                playerUnits.Add(unit1);
            }
            team1 = new JCombatTeam(playerUid, playerUnits);

            //创建levelnode unit对象
            var levelNodeUnits = new List<IJCombatUnit>();
            foreach (var formationInfo in levelodeFormationInfos)
            {
                //队伍2 从配置表获取武士点位
                var unit2 = new JCombatTurnBasedUnit(formationInfo.UnitInfo, attrNameQuery, context);
                levelNodeUnits.Add(unit2);
            }
            team2 = new JCombatTeam(levelNodeBusinessId, levelNodeUnits);

            var lstTeams = new List<IJCombatTeam>();
            lstTeams.Add(team1);
            lstTeams.Add(team2);

            //设置双方队伍
            jcombatQuery.SetTeams(lstTeams);

            actionSelector = new JCombatTurnBasedActionSelector(jcombatQuery.GetUnits().OfType<IJCombatTurnBasedUnit>().ToList());

            var runables = new List<IRunable>();
            runables.Add(team1);
            runables.Add(team2);

            turnbasedCombat = new JCombatTurnBased(actionSelector, frameRecorder, jcombatQuery, runables);
            combatRunner = new JCombatTurnBasedRunner(turnbasedCombat, jcombatQuery, eventRecorder, report);

            await combatRunner.Run();

            var result = combatRunner.GetReport();

            var reportData = report.GetCombatReportData<TiktokJCombatUnitData>() as TiktokJCombatTurnBasedReportData;
            reportData.CombatSceneBusinessId = tiktokConfigService.GetLevelNodeCombatSceneBusinessId(levelNodeBusinessId);
            reportData.Evaluation = evaluationService.GetEvaluation(playerUid, reportData);
            //将reportData中的events按sortIndex升序排序
            reportData.events = reportData.events.OrderBy(e => e.SortIndex).ToList();

            return reportData;
        }

    }

    


}


