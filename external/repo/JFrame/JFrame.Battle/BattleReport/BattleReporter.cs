
//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace JFramework
{
    public enum ReportType
    {
        ActionCast, //英雄动作
        Damage, //受伤
        Miss, //
        Heal,   //治疗回血
        Dead,   //死亡
        Reborn, //复活
        MaxHpUp, //最大生命提升
        AddBuffer, //buff添加了
        RemoveBuffer, //buff移除了
        CastBuffer, //buff触发了
        UpdateBuffer,//更新
        DebuffAnti, //状态抵抗
        ActionCD, //动作CD
        StartMove, //开始移动
        SpeedChanged, //速度改变
        EndMove, //停止移动
        ShootChange, //射击目标改变
    }

    /// <summary>
    /// pvp战报对象
    /// </summary>
    public class BattleReporter : IBattleReporter
    {
        public class Comp : IComparer<IBattleReportData>
        {
            public int Compare(IBattleReportData x, IBattleReportData y)
            {
                if (x.EscapeTime == y.EscapeTime) return 0;
                if (x.EscapeTime < y.EscapeTime) return -1;
                return 1;
            }
        }

        //Utility utility = new Utility();

        List<IBattleReportData> reports = new List<IBattleReportData>();

        CombatFrame frame;

        Dictionary<PVPBattleManager.Team, BattleTeam> teams;

        public BattleReporter(CombatFrame frame, Dictionary<PVPBattleManager.Team, BattleTeam> teams) {
            this.frame = frame;
            this.teams = teams;
            if(teams != null)
            {
                foreach (var team in this.teams.Values)
                {
                    team.onActionCast += Team_onActionCast;
                    team.onActionStartCD += Team_onActionStartCD;
                    team.onDamage += Team_onDamage;
                    team.onHeal += Team_onHeal;
                    team.onReborn += Team_onReborn;
                    team.onDebuffAnti += Team_onDebuffAnti;
                    team.onMaxHpUp += Team_onMaxHpUp;
                    team.onDead += Team_onDead;
                    team.onBufferAdded += Team_onBufferAdded;
                    team.onBufferRemoved += Team_onBufferRemoved;
                    team.onBufferCast += Team_onBufferCast;
                    team.onBufferUpdate += Team_onBufferUpdate;
                }
            }
        }



        private void Team_onActionCast(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, float duration)
        {
            List<string> lstUID = new List<string>();
            for (int i = 0; i < targets.Count; i ++)
            {
                lstUID.Add(targets[i].Uid);
            }
            AddReportData(caster.Uid, ReportType.ActionCast, targets[0].Uid, new object[] { action.Id, lstUID , duration });
        }

        private void Team_onActionStartCD(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, float cd)
        {
            AddReportData(caster.Uid, ReportType.ActionCD, action.Uid, new object[] { action.Id, cd});
        }

        private void Team_onDamage(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, IBattleUnit target, ExecuteInfo value)
        {
            AddReportData(caster.Uid, ReportType.Damage, target.Uid, new object[] { value.Value, target.HP, target.MaxHP , value.IsCri, value.IsBlock});
        }


        private void Team_onHeal(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, IBattleUnit target, int value)
        {
            AddReportData(caster.Uid, ReportType.Heal, target.Uid, new object[] { value, target.HP, target.MaxHP });
        }

        private void Team_onMaxHpUp(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, IBattleUnit target, int value)
        {
            AddReportData(caster.Uid, ReportType.MaxHpUp, target.Uid, new object[] { value, target.HP, target.MaxHP });
        }

        private void Team_onReborn(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, IBattleUnit target, int value)
        {
            AddReportData(caster.Uid, ReportType.Reborn, target.Uid, new object[] { value, target.HP, target.MaxHP });
        }
        private void Team_onDead(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            AddReportData(caster.Uid, ReportType.Dead, target.Uid, new object[] {0});
        }

        private void Team_onBufferAdded(PVPBattleManager.Team team, IBattleUnit target, IBuffer buffer)
        {
            AddReportData(target.Uid, ReportType.AddBuffer, target.Uid, new object[] {buffer.Uid,  buffer.Id, buffer.FoldCount });
        }

        private void Team_onBufferCast(PVPBattleManager.Team team, IBattleUnit target, IBuffer buffer)
        {
            AddReportData(target.Uid, ReportType.CastBuffer, target.Uid, new object[] { buffer.Uid, buffer.Id });
        }

        private void Team_onBufferRemoved(PVPBattleManager.Team team, IBattleUnit target, IBuffer buffer)
        {
            AddReportData(target.Uid, ReportType.RemoveBuffer, target.Uid, new object[] { buffer.Uid, buffer.Id });
        }

        private void Team_onBufferUpdate(PVPBattleManager.Team arg1, IBattleUnit target, IBuffer buffer, int foldCount, float[] args)
        {
            AddReportData(target.Uid, ReportType.UpdateBuffer, target.Uid, new object[] { buffer.Uid, buffer.Id , foldCount, args});
        }

        private void Team_onDebuffAnti(PVPBattleManager.Team team, IBattleUnit caster, IBattleAction action, IBattleUnit target, int debuffId)
        {
            AddReportData(target.Uid, ReportType.DebuffAnti, target.Uid, new object[] { debuffId });
        }


        public List<IBattleReportData> GetAllReportData()
        {
            return reports;
        }

        /// <summary>
        /// 添加一个动作类型战报，并返回唯一id
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="escapeTime"></param>
        /// <param name="casterUID"></param>
        /// <param name="reportType"></param>
        /// <param name="targetUID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string AddReportData(string casterUID, ReportType reportType, string targetUID, object[] arg , float timeOffset = 0f)
        {
            //to do: 增加一个流逝时间偏移量，可以延迟播放
            var data = new BattleReportData(frame.CurFrame,frame.GetDeltaTime(frame.CurFrame) + timeOffset, casterUID, reportType, targetUID, arg);

            if (ContainsReport(data))
                throw new Exception("已经存在战报" + frame + " " + casterUID + " " + reportType);

            reports.Add(data);
            //utility.BinarySearchInsert(reports, data, new Comp());
            return data.UID;
        }

        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        bool ContainsReport(IBattleReportData report)
        {
            foreach (var data in reports)
            {
                if (data.UID.Equals(report.UID))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定战报
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="hostUID"></param>
        /// <returns></returns>
        public IBattleReportData GetReportData(string uid)
        {
            foreach (var report in reports)
            {
                if (report.UID.Equals(uid))
                    return report;
            }
            return null;
        }

    }
}