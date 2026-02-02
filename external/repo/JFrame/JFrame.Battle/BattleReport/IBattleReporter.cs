using System.Collections.Generic;

namespace JFramework
{
    public interface IBattleReporter
    {
        string AddReportData(string casterUID, ReportType reportType, string targetUID, object[] arg, float timeOffset = 0f);

        IBattleReportData GetReportData(string uid);

        List<IBattleReportData> GetAllReportData();
    }
}