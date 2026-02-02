using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// pvp战报对象
    /// </summary>
    public class CombatReporter : ICombatReporter
    {
        public class Comp : IComparer<ICombatReportData>
        {
            public int Compare(ICombatReportData x, ICombatReportData y)
            {
                if (x.EscapeTime == y.EscapeTime) return 0;
                if (x.EscapeTime < y.EscapeTime) return -1;
                return 1;
            }
        }

        //Utility utility = new Utility();

        List<ICombatReportData> reports = new List<ICombatReportData>();

        CombatFrame frame;

        List<CommonCombatTeam> teams;

        Dictionary<string, long> damageStatistics = new Dictionary<string, long>();
        public Dictionary<string, long> DamageStatistics => damageStatistics;

        public CombatReporter(CombatFrame frame, List<CommonCombatTeam> teams)
        {
            this.frame = frame;
            this.teams = teams;
            if (teams != null)
            {
                foreach (var team in this.teams)
                {
                    team.onActionCast += Team_onActionCast;
                    team.onActionStartCD += Team_onActionStartCD;
                    team.onDamage += Team_onDamage;
                    team.onMiss += Team_onMiss;
                    team.onHeal += Team_onHeal;
                    //team.onReborn += Team_onReborn;
                    //team.onDebuffAnti += Team_onDebuffAnti;
                    //team.onMaxHpUp += Team_onMaxHpUp;
                    team.onDead += Team_onDead;
                    team.onBufferAdded += Team_onBufferAdded;
                    team.onBufferRemoved += Team_onBufferRemoved;
                    //team.onBufferCast += Team_onBufferCast;
                    //team.onBufferUpdate += Team_onBufferUpdate;
                    team.onUnitStartMove += Team_onUnitStartMove;
                    team.onUnitSpeedChanged += Team_onUnitSpeedChanged;
                    team.onUnitEndMove += Team_onUnitEndMove;
                    team.onShootTargetChanged += Team_onShootTargetChanged;
                    team.onActionCdChange += Team_onActionCdChange;
                }
            }
        }




        /// <summary>
        /// action释放了
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Team_onActionCast(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.TargetsUid = data.GetTargetsUid();
            reportData.CastDuration = data.CastDuration;

            AddReportData(ReportType.ActionCast, reportData);
        }



        //private void Team_onActionCast(int teamId, ICombatUnit caster, CombatAction action, List<ICombatUnit> targets, float duration)
        //{
        //    List<string> lstUID = new List<string>();
        //    for (int i = 0; i < targets.Count; i++)
        //    {
        //        lstUID.Add(targets[i].Uid);
        //    }
        //    AddReportData(caster.Uid, ReportType.Action, targets[0].Uid, new object[] { action.Id, lstUID, duration });
        //}

        private void Team_onActionStartCD(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.ActionUid = data.Action.Uid;
            reportData.TargetsUid = data.GetTargetsUid();
            reportData.CdDuration = data.CdDuration;

            AddReportData(ReportType.ActionCD, reportData);
        }


        //private void Team_onActionStartCD(PVPBattleManager.Team team, ICombatUnit caster, CombatAction action, float cd)
        //{
        //    AddReportData(caster.Uid, ReportType.ActionCD, action.Uid, new object[] { action.Id, cd });
        //}

        private void Team_onDamage(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.ActionUid = data.Action.Uid;
            reportData.TargetUid = data.Target.Uid;
            reportData.Value = (double)data.Value;
            reportData.TargetHp = (double)data.Target.GetAttributeCurValue(CombatAttribute.CurHp);
            reportData.TargetMaxHp = (double)data.Target.GetAttributeCurValue(CombatAttribute.MaxHP);
            reportData.IsCri = data.IsCri;
            reportData.IsBlock = data.IsBlock;
            AddReportData(ReportType.Damage, reportData);

            //统计伤害
            StatisticsDamage(data.Owner.Uid, data.Target.Uid, (long)reportData.Value);
        }

        private void StatisticsDamage(string caster, string target, long value)
        {
            if(!damageStatistics.ContainsKey(caster))
            {
                damageStatistics.Add(caster, 0);
            }

            damageStatistics[caster] += value;
        }


        //private void Team_onDamage(PVPBattleManager.Team team, ICombatUnit caster, CombatAction action, ICombatUnit target, ExecuteInfo value)
        //{
        //    AddReportData(caster.Uid, ReportType.Damage, target.Uid, new object[] { value.Value, target.HP, target.MaxHP, value.IsCri, value.IsBlock });
        //}

        private void Team_onHeal(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.ActionUid = data.Action.Uid;
            reportData.TargetUid = data.Target.Uid;
            reportData.Value = (double)data.Value;
            reportData.TargetHp = (double)data.Target.GetAttributeCurValue(CombatAttribute.CurHp);
            reportData.TargetMaxHp = (double)data.Target.GetAttributeCurValue(CombatAttribute.MaxHP);
            reportData.IsCri = data.IsCri;
            reportData.IsBlock = data.IsBlock;
            AddReportData(ReportType.Heal, reportData);
        }


        //private void Team_onHeal(PVPBattleManager.Team team, ICombatUnit caster, CombatAction action, ICombatUnit target, int value)
        //{
        //    AddReportData(caster.Uid, ReportType.Heal, target.Uid, new object[] { value, target.HP, target.MaxHP });
        //}

        //private void Team_onMaxHpUp(PVPBattleManager.Team team, ICombatUnit caster, CombatAction action, ICombatUnit target, int value)
        //{
        //    AddReportData(caster.Uid, ReportType.MaxHpUp, target.Uid, new object[] { value, target.HP, target.MaxHP });
        //}

        //private void Team_onReborn(PVPBattleManager.Team team, ICombatUnit caster, CombatAction action, ICombatUnit target, int value)
        //{
        //    AddReportData(caster.Uid, ReportType.Reborn, target.Uid, new object[] { value, target.HP, target.MaxHP });
        //}


        private void Team_onDead(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.ActionUid = data.Action.Uid;
            reportData.TargetUid = data.Target.Uid;
            
            AddReportData(ReportType.Dead, reportData);
        }


        //private void Team_onDead(PVPBattleManager.Team team, ICombatUnit caster, CombatAction action, ICombatUnit target)
        //{
        //    AddReportData(caster.Uid, ReportType.Dead, target.Uid, new object[] { 0 });
        //}


        private void Team_onBufferAdded(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.ActionUid = data.Action.Uid;
            reportData.TargetUid = data.Target.Uid;
            reportData.BufferUid = data.Buffer.Uid;
            reportData.BufferFoldCount = data.Buffer.GetCurFoldCount();
            reportData.BufferId = data.Buffer.Id;

            AddReportData(ReportType.AddBuffer, reportData);
        }


        //private void Team_onBufferAdded(PVPBattleManager.Team team, ICombatUnit target, IBuffer buffer)
        //{
        //    AddReportData(target.Uid, ReportType.AddBuffer, target.Uid, new object[] { buffer.Uid, buffer.Id, buffer.FoldCount });
        //}

        //private void Team_onBufferCast(PVPBattleManager.Team team, ICombatUnit target, IBuffer buffer)
        //{
        //    AddReportData(target.Uid, ReportType.CastBuffer, target.Uid, new object[] { buffer.Uid, buffer.Id });
        //}

        private void Team_onBufferRemoved(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.ActionUid = data.Action.Uid;
            reportData.TargetUid = data.Target.Uid;
            reportData.BufferUid = data.Buffer.Uid;
            reportData.BufferFoldCount = data.Buffer.GetCurFoldCount();
            reportData.BufferId = data.Buffer.Id;
            AddReportData(ReportType.RemoveBuffer, reportData);
        }

        //private void Team_onBufferRemoved(PVPBattleManager.Team team, ICombatUnit target, IBuffer buffer)
        //{
        //    AddReportData(target.Uid, ReportType.RemoveBuffer, target.Uid, new object[] { buffer.Uid, buffer.Id });
        //}

        //private void Team_onBufferUpdate(PVPBattleManager.Team arg1, ICombatUnit target, IBuffer buffer, int foldCount, float[] args)
        //{
        //    AddReportData(target.Uid, ReportType.UpdateBuffer, target.Uid, new object[] { buffer.Uid, buffer.Id, foldCount, args });
        //}

        //private void Team_onDebuffAnti(PVPBattleManager.Team team, ICombatUnit caster, CombatAction action, ICombatUnit target, int debuffId)
        //{
        //    AddReportData(target.Uid, ReportType.DebuffAnti, target.Uid, new object[] { debuffId });
        //}



        private void Team_onUnitStartMove(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.Velocity = data.Velocity;

            AddReportData(ReportType.StartMove, reportData);
        }


        private void Team_onUnitSpeedChanged(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.Velocity = data.Velocity;

            AddReportData(ReportType.SpeedChanged, reportData);
        }


        private void Team_onUnitEndMove(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.Velocity = data.Velocity;

            AddReportData(ReportType.EndMove, reportData);
        }

        /// <summary>
        /// 发射目标改变了
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="data"></param>
        private void Team_onShootTargetChanged(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.TargetUid = data.Target.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.TargetsUid = data.GetTargetsUid();
            reportData.CastDuration = data.CastDuration;
            reportData.ShootCount = data.ShootCount;

            AddReportData(ReportType.ShootChange, reportData);
        }



        /// <summary>
        /// miss消息
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Team_onMiss(int teamId, CombatExtraData data)
        {
            var reportData = new ReportData();

            reportData.CastUnitUid = data.Owner.Uid;
            reportData.ActionId = data.Action.Id;
            reportData.ActionUid = data.Action.Uid;
            reportData.TargetUid = data.Target.Uid;
            reportData.Value = (double)data.Value;
            //reportData.TargetHp = (double)data.Target.GetAttributeCurValue(CombatAttribute.CurHp);
            //reportData.TargetMaxHp = (double)data.Target.GetAttributeCurValue(CombatAttribute.MaxHP);
            reportData.IsCri = data.IsCri;
            reportData.IsBlock = data.IsBlock;
            AddReportData(ReportType.Miss, reportData);
        }

        /// <summary>
        /// cd 發生變更了
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Team_onActionCdChange(int arg1, CombatExtraData arg2)
        {
            //throw new NotImplementedException();
        }


        public List<ICombatReportData> GetAllReportData()
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
        public string AddReportData(ReportType reportType, ReportData reportData)
        {
            //to do: 增加一个流逝时间偏移量，可以延迟播放
            var data = new CombatReportData(frame.CurFrame, frame.GetDeltaTime(frame.CurFrame), reportType, reportData);

            if (ContainsReport(data))
                throw new Exception("已经存在战报" + frame  + " " + reportType);

            reports.Add(data);
            //utility.BinarySearchInsert(reports, data, new Comp());
            return data.UID;
        }

        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        bool ContainsReport(ICombatReportData report)
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
        public ICombatReportData GetReportData(string uid)
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