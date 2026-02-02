using JFramework.Common;
using System;

namespace JFramework
{
    /// <summary>
    /// 动作战报数据，谁什么时间向谁使用了什么动作
    /// </summary>
    [Serializable]
    public class CombatReportData : ICombatReportData
    {
        public string UID { get; private set; }
        public int Frame { get; private set; }
        public float EscapeTime { get; private set; } //从战斗开始到现在流逝的时间

        public ReportType ReportType { get; private set; }

        public ReportData ReportData { get; private set; }

        public CombatReportData(int frame, float escapeTime, ReportType reportType, ReportData reportData)
        {
            UID = Guid.NewGuid().ToString();
            Frame = frame;
            EscapeTime = escapeTime;
            ReportType = reportType;
            ReportData = reportData;
        }

        public object Clone()
        {
            var utilty = new Utility();
            return utilty.DeepClone(this);
        }
    }
}