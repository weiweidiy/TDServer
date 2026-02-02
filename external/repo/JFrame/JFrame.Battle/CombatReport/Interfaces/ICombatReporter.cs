using System.Collections.Generic;
using static JFramework.CombatReporter;

namespace JFramework
{
    public interface ICombatReporter
    {
        string AddReportData(ReportType reportType, ReportData reportData);

        ICombatReportData GetReportData(string uid);

        List<ICombatReportData> GetAllReportData();
    }
}