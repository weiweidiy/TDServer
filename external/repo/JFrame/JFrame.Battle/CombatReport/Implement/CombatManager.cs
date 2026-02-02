//using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static JFramework.PVPBattleManager;

namespace JFramework
{
    public abstract class CombatManager : ICombatManager<CombatReport, CommonCombatTeam, CombatUnit>
    {
        protected CombatFrame frame ;
        public CombatFrame Frame { get => frame; }

        protected CombatJudge combatJudge;

        protected CombatReport report;

        public CombatReporter Reporter { get; protected set; }

        protected CombatBufferFactory bufferFactory = new CombatBufferFactory();

        protected bool isCombatOver = false;

        public ILogger logger = null;

        public CombatManager(float limitTime, float deltaTime, ILogger logger = null)
        {
            frame = new CombatFrame(limitTime, deltaTime);
            report = new CombatReport();
            this.logger = logger;
        }

        public CombatManager() : this(90f, 0.25f) { }

        public void Update()
        {
            Update(frame);
   
        }

        public void Start()
        {
            foreach (var team in GetTeams())
            {
                team.Start();
            }
        }
        public void Stop()
        {
            foreach (var team in GetTeams())
            {
                team.Stop();
            }
        }
        public virtual async Task<CombatReport> GetResult()
        {
            CombatReport r = await Task.Run(() =>
            {
                // 模拟一个耗时操作
                var attackers = GetTeamData(0);
                var defencers = GetTeamData(1);

                frame.ResetFrame();

                combatJudge = new CombatJudge(GetTeam(0), GetTeam(1));

                Reporter = new CombatReporter(frame, GetTeams());

                //开始战斗
                Start();

                //更新战斗 如果战斗没有决出胜负，则继续战斗
                while (!combatJudge.IsOver() && !frame.IsMaxFrame())
                {
                    Update();

                    frame.NextFrame();
                }

                var winner = combatJudge.GetWinner().TeamId == 0 ? 1 : 0; //1:挑战成功 0：挑战失败

                report.report = Reporter.GetAllReportData();
                report.winner = winner;
                report.deltaTime = frame.DeltaTime;
                report.attacker = attackers;
                report.defence = defencers;
                report.damageStatistics = Reporter.DamageStatistics;

                Stop();

                return report;
            });



            //Debug.Log("战斗结束 " + frame.FrameCount);
            //await UniTask.Delay(5000);
            return r;
        }

        public abstract void ClearResult();

        public abstract KeyValuePair<CombatTeamType, List<CombatUnitInfo>> GetTeamData(int teamId);

        public abstract void AddTeam(int teamId, CommonCombatTeam teamObj);
        public abstract CommonCombatTeam GetTeam(int teamId);
        public abstract List<CommonCombatTeam> GetTeams();
        public int GetOppoTeamId(int teamId)
        {
            return teamId == 0 ? 1 : 0;
        }

        public virtual int GetOppoTeamId(CombatUnit unit)
        {
            var team0 = GetTeam(0);
            foreach (var item in team0.GetUnits())
            {
                if (item.Uid == unit.Uid)
                    return 1;
            }

            return 0;
        }

        public virtual int GetFriendTeamId(CombatUnit unit)
        {
            var teams = GetTeams();
            foreach (var team in teams)
            {
                var units = team.GetUnits();
                foreach (var item in units)
                {
                    if (item.Uid == unit.Uid)
                        return team.TeamId;
                }
            }

            throw new Exception("沒有找對對方的隊伍id");
        }

        public float GetCombatTimeLimit()
        {
            return frame.AllTime;
        }
        public abstract bool IsBuffer(int buffId);
        public abstract object GetExtraData();
        public abstract void AddUnit(int teamId, CombatUnit unit);
        public abstract void RemoveUnit(int teamId, CombatUnit unit);
        public int GetUnitCount(int teamId)
        {
            return GetTeam(teamId).Count();
        }
        public abstract int GetAllUnitCount();
        public CombatUnit GetUnit(string uid)
        {
            foreach (var team in GetTeams())
            {
                var unit = team.GetUnit(uid);
                if (unit != null)
                    return unit;
            }
            return null;
        }

        public int GetUnitTeamId(string uid)
        {
            var teams = GetTeams();
            for(int i = 0; i < teams.Count; i++)
            {
                var team = teams[i];
                if( team.Get(uid) != null ) return team.TeamId;
            }

            return -1;
        }

        public virtual List<CombatUnit> GetUnits(int teamId, bool findMode)
        {
            var team = GetTeam(teamId);
            return team.GetUnits(findMode);
        }
        public virtual List<CombatUnit> GetUnits(bool findMode)
        {
            var result = new List<CombatUnit>();

            foreach (var team in GetTeams())
            {
                result.AddRange(team.GetUnits(findMode));
            }

            return result;
        }
        public virtual List<CombatUnit> GetUnitsInRange(CombatUnit unit, int teamId, float range, bool alive = true, bool findMode = false)
        {
            var units = GetUnits(teamId, findMode);
            if (range == -1)
                return units;

            var result = new List<CombatUnit>();

            foreach (var item in units)
            {
                if (item.IsAlive() != alive)
                    continue;

                var myX = (unit as ICombatMovable).GetPosition().x;
                var x = (item as ICombatMovable).GetPosition().x;
                if (Math.Abs(myX - x) <= range)
                    result.Add(item);
            }

            return result;
        }

        protected virtual void Update(CombatFrame frame)
        {
            foreach (var team in GetTeams())
            {
                team.UpdatePosition(frame);
            }

            foreach (var team in GetTeams())
            {
                team.Update(frame);
            }
        }

        public bool IsCombatOver() => isCombatOver;


    }
}
