//using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFramework
{
    public class SingleCombatManager : CombatManager
    {
        protected Dictionary<int, CommonCombatTeam> teams;

        protected Dictionary<int, KeyValuePair<CombatTeamType, List<CombatUnitInfo>>> teamsData;

        public SingleCombatManager() : base() { }
        public SingleCombatManager(float limitTime, float deltaTime,ILogger logger = null) : base(limitTime, deltaTime, logger)
        {
        }

        public void Initialize(KeyValuePair<CombatTeamType, List<CombatUnitInfo>> dicTeam1Data, KeyValuePair<CombatTeamType, List<CombatUnitInfo>> dicTeam2Data, List<CombatBufferInfo> buffers, float timeLimit, CombatUnitInfo god = null)
        {
            teams = new Dictionary<int, CommonCombatTeam>();
            teamsData = new Dictionary<int, KeyValuePair<CombatTeamType, List<CombatUnitInfo>>>();
            teamsData.Add(0, dicTeam1Data);
            teamsData.Add(1, dicTeam2Data);

            var context = new CombatContext();
            context.CombatManager = this;
            context.CombatBufferFactory = bufferFactory;

            var team1Type = dicTeam1Data.Key;
            var team1Data = dicTeam1Data.Value;

            var team2Type = dicTeam2Data.Key;
            var team2Data = dicTeam2Data.Value;

            if (team1Data == null || team2Data == null)
                throw new ArgumentNullException("teamdata 不能為null");

            CommonCombatTeam team1 = team1Type == CombatTeamType.Combine ? new SpecialCombatTeam() : new CommonCombatTeam();
            team1.Initialize(0, context, team1Data);
            if (team1Data.Count > 0)
                AddTeam(0, team1); //1 = 隊伍id

            CommonCombatTeam team2 = team2Type == CombatTeamType.Combine ? new SpecialCombatTeam() : new CommonCombatTeam();
            team2.Initialize(1, context, team2Data);
            if (team2Data.Count > 0)
                AddTeam(1, team2);
          

            //预加载所有buffers
            bufferFactory.PreloadBuffers(buffers, context);
        }



        #region 队伍接口
        /// <summary>
        /// 添加隊伍
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="teamObj"></param>
        /// <exception cref="Exception"></exception>
        public override void AddTeam(int teamId, CommonCombatTeam teamObj)
        {
            if (teams == null)
                throw new Exception("team list is not init , please call the Initialize method ");

            teams.Add(teamId, teamObj);
        }

        /// <summary>
        /// 獲取隊伍對象
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public override CommonCombatTeam GetTeam(int teamId)
        {
            if (teams.ContainsKey(teamId))
                return teams[teamId];
            return null;
        }

        /// <summary>
        /// 獲取所有隊伍
        /// </summary>
        /// <returns></returns>
        public override List<CommonCombatTeam> GetTeams()
        {
            return teams.Values.ToList();
        }
        #endregion

        #region unit接口
        public override void AddUnit(int teamId, CombatUnit unit)
        {
            throw new System.NotImplementedException();
        }
        public override void RemoveUnit(int teamId, CombatUnit unit)
        {
            throw new System.NotImplementedException();
        }

        public override int GetAllUnitCount()
        {
            throw new NotImplementedException();
        }
        #endregion



        public override object GetExtraData()
        {
            throw new System.NotImplementedException();
        }


        public override void ClearResult()
        {

        }

        public override bool IsBuffer(int buffId)
        {
            throw new System.NotImplementedException();
        }

        public override KeyValuePair<CombatTeamType, List<CombatUnitInfo>> GetTeamData(int teamId)
        {
            return teamsData[teamId];
        }

        public override Task<CombatReport> GetResult()
        {
            isCombatOver = true;
            return base.GetResult();
        }
    }
}
