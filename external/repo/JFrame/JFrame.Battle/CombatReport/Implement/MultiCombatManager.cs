//using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static JFramework.PVPBattleManager;

namespace JFramework
{
    /// <summary>
    /// 多波战斗
    /// </summary>
    public class MultiCombatManager : CombatManager
    {
        Dictionary<int, List<CommonCombatTeam>> dicTeams = new Dictionary<int, List<CommonCombatTeam>>();

        Dictionary<int, List<KeyValuePair<CombatTeamType, List<CombatUnitInfo>>>> dicTeamsData = new Dictionary<int, List<KeyValuePair<CombatTeamType, List<CombatUnitInfo>>>>();

        int curLeftTeamIndex = 0;
        int curRightTeamIndex = 0;

        CombatContext context;

        public MultiCombatManager(float limitTime, float deltaTime , ILogger logger = null) : base(limitTime, deltaTime, logger)
        {
        }

        /// <summary>
        /// 初始化战斗数据
        /// </summary>
        /// <param name="dicTeam1Data"></param>
        /// <param name="dicTeam2Data"></param>
        /// <param name="bufferInfos"></param>
        /// <param name="god"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Initialize(List<KeyValuePair<CombatTeamType, List<CombatUnitInfo>>> dicTeam1Data, List<KeyValuePair<CombatTeamType, List<CombatUnitInfo>>> dicTeam2Data, List<CombatBufferInfo> bufferInfos,  CombatUnitInfo god = null)
        {
            dicTeams = new Dictionary<int, List<CommonCombatTeam>>();
            dicTeamsData = new Dictionary<int, List<KeyValuePair<CombatTeamType, List<CombatUnitInfo>>>>();
            dicTeamsData.Add(0, dicTeam1Data);
            dicTeamsData.Add(1, dicTeam2Data);

            context = new CombatContext();
            context.CombatManager = this;
            context.CombatBufferFactory = bufferFactory;
            context.Logger = logger;


            for(int i = 0; i < dicTeam1Data.Count; i++)
            {
                var groupType = dicTeam1Data[i].Key;
                var groupData = dicTeam1Data[i].Value;

                if (groupData == null)
                    throw new ArgumentNullException("teamdata 不能為null");

                CommonCombatTeam group = CreateTeam(groupType);
                group.Initialize(0, context, groupData);
                if (groupData.Count > 0)
                    AddTeam(0, group); //1 = 隊伍id
            }


            for (int i = 0; i < dicTeam2Data.Count; i++)
            {
                var groupType = dicTeam2Data[i].Key;
                var groupData = dicTeam2Data[i].Value;
                if (groupData == null)
                    throw new ArgumentNullException("teamdata 不能為null");

                CommonCombatTeam group = CreateTeam(groupType);
                group.Initialize(1, context, groupData);
                if (groupData.Count > 0)
                    AddTeam(1, group);
            }

            //预加载所有buffers
            bufferFactory.PreloadBuffers(bufferInfos, context);
        }

        CommonCombatTeam CreateTeam(CombatTeamType teamType)
        {
            switch(teamType)
            {
                case CombatTeamType.Single:
                    return new CommonCombatTeam();
                case CombatTeamType.Combine:
                    return new SpecialCombatTeam();
                case CombatTeamType.Remix:
                    return new RemixCombatTeam();
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 查找单位
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<CombatUnitInfo> FindUnit(int teamId,  Func<CombatUnitInfo, bool> predicate)
        {
            var result = new List<CombatUnitInfo>();
            var teamData = GetTeamData(teamId);
            var lst = teamData.Value;

            foreach(var info in  lst)
            {
                if(predicate(info))
                    result.Add(info);
            }
            return result;
        }

        /// <summary>
        /// 获取战报
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override async Task<CombatReport> GetResult()
        {
            var report = await base.GetResult();
            var result = NextGroup(report.winner);
            isCombatOver = !result;
            return report;

        }
        
        /// <summary>
        /// 获取小组数量
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        int GetGroupCount(int teamId)
        {
            return dicTeamsData[teamId].Count;
        }

        /// <summary>
        /// 当前是否最后一组
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        bool IsLastGroup(int teamId)
        {
            var curIndex = GetCurGroupIndex(teamId);
            var groupCount = GetGroupCount(teamId);
            return curIndex == groupCount - 1;
        }

        /// <summary>
        /// 进入下一组
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        bool NextGroup(int teamId)
        {
            var curIndex = GetCurGroupIndex(teamId);
            var groupCount = GetGroupCount(teamId);

            if(curIndex + 1 < groupCount)
            {
                SetCurGroupIndex(teamId, curIndex + 1);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 获取当前的队伍的group索引
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public int GetCurGroupIndex(int teamId)
        {
            return teamId == 0 ? curLeftTeamIndex : curRightTeamIndex;
        }

        /// <summary>
        /// 设置当前索引
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="curIndex"></param>
        void SetCurGroupIndex(int teamId , int curIndex)
        {
            if (teamId == 0)
            {
                curLeftTeamIndex = curIndex;
            }
            else
            {
                curRightTeamIndex = curIndex;
            }
        }

        /// <summary>
        /// 获取指定队伍的队伍原始数据
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override KeyValuePair<CombatTeamType, List<CombatUnitInfo>> GetTeamData(int teamId)
        {
            var curIndex = GetCurGroupIndex(teamId);
            return GetTeamData(teamId, curIndex);
        }

        /// <summary>
        /// 获取指定波次的队伍原始数据
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="wave"></param>
        /// <returns></returns>
        public KeyValuePair<CombatTeamType, List<CombatUnitInfo>> GetTeamData(int teamId, int wave)
        {
            var allData = dicTeamsData[teamId];
            if (wave >= allData.Count)
                throw new Exception($"获取队伍信息时索引越界 参数{wave} 实际长度：{allData.Count}");

            return allData[wave];
        }

        /// <summary>
        /// 更新替换指定索引波次的队伍信息
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="wave"></param>
        /// <param name="teamData"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateTeamData(int teamId, int wave, KeyValuePair<CombatTeamType, List<CombatUnitInfo>> teamData)
        {
            var allData = dicTeamsData[teamId];
            if (wave >= allData.Count)
                throw new Exception($"获取队伍信息时索引越界 参数{wave} 实际长度：{allData.Count}");

            allData[wave] = teamData;

            //创建新的队伍对象
            CommonCombatTeam team = teamData.Key == CombatTeamType.Combine ? new SpecialCombatTeam() : new CommonCombatTeam();
            team.Initialize(teamId, context, teamData.Value);
            if (teamData.Value.Count > 0)
                UpdateTeam(teamId, wave, team);
                //AddTeam(0, group); //1 = 隊伍id
        }

        /// <summary>
        /// 添加到队伍里
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="team"></param>
        public override void AddTeam(int teamId, CommonCombatTeam team)
        {
            if (!dicTeams.ContainsKey(teamId))
                dicTeams.Add(teamId, new List<CommonCombatTeam>());

            var allGroup = dicTeams[teamId];
            allGroup.Add(team);
        }

        /// <summary>
        /// 更新队伍对象
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="wave"></param>
        /// <param name="team"></param>
        public void UpdateTeam(int teamId, int wave, CommonCombatTeam team)
        {
            if (!dicTeams.ContainsKey(teamId))
                dicTeams.Add(teamId, new List<CommonCombatTeam>());

            var allGroup = dicTeams[teamId];
            allGroup[wave] = team;
        }

        /// <summary>
        /// 获取当前的队伍
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public override CommonCombatTeam GetTeam(int teamId)
        {
            var curIndex = GetCurGroupIndex(teamId);
            var allGroup = dicTeams[teamId];
            return allGroup[curIndex];
        }

        /// <summary>
        /// 获取当前波次双方队伍
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override List<CommonCombatTeam> GetTeams()
        {
            var result = new List<CommonCombatTeam>();
            result.Add(GetTeam(0));
            result.Add(GetTeam(1));
            return result;
        }


        public override void AddUnit(int teamId, CombatUnit unit)
        {
            throw new System.NotImplementedException();
        }

        public override void ClearResult()
        {
            throw new System.NotImplementedException();
        }

        public override int GetAllUnitCount()
        {
            throw new System.NotImplementedException();
        }


        public override object GetExtraData()
        {
            throw new System.NotImplementedException();
        }



        protected override void Update(CombatFrame frame)
        {
            base.Update(frame);

            context.combatBulletManager.Update(frame);
        }



        public override bool IsBuffer(int buffId)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUnit(int teamId, CombatUnit unit)
        {
            throw new System.NotImplementedException();
        }


    }
}
