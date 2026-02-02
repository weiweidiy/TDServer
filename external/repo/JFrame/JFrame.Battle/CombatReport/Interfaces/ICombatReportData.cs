using System;

namespace JFramework
{
    public interface ICombatReportData : ICloneable
    {
        string UID { get; }
        int Frame { get; }
        float EscapeTime { get; }
        ReportData ReportData { get; }
        ReportType ReportType { get; }

        ///// <summary>
        ///// 对应id
        ///// </summary>
        //object[] Arg { get; } //如果reportType = action , arg = action id, 如果是damage , Arg = damage

    }
}
