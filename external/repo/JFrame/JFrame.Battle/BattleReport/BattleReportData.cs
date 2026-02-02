using System;

namespace JFramework
{
    /// <summary>
    /// 动作战报数据，谁什么时间向谁使用了什么动作
    /// </summary>
    public class BattleReportData : IBattleReportData
    {
        public string UID { get; private set; }
        public int Frame { get; private set; }
        public float EscapeTime { get; private set; } //从战斗开始到现在流逝的时间
        public string CasterUID { get; private set; } //行动者UID
        //public string ActionName { get; private set; } //SkillId
        public string TargetUID { get; private set; } //目标UID
        public object[] Arg { get; private set; }   //动作ID

        public ReportType ReportType { get; private set; }

        public BattleReportData(int frame, float escapeTime, string casterUID, ReportType reportType, string targetUID, object[] arg)
        {
            UID = Guid.NewGuid().ToString();
            Frame = frame;
            EscapeTime = escapeTime;
            CasterUID = casterUID;
            ReportType = reportType;
            TargetUID = targetUID;
            Arg = arg;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}