using System.Collections.Generic;

namespace JFramework
{
    public class PVPBattleReportParser
    {
        List<IBattleReportData> report = new List<IBattleReportData>();
        public PVPBattleReportParser(List<IBattleReportData> report)
        {
            foreach (var data in report)
            {
                this.report.Add(data.Clone() as IBattleReportData);
            }
            //this.report = report;
            this.report.Reverse();
        }

        public List<IBattleReportData> GetData(float escapeTime)
        {
            var result = new List<IBattleReportData>();
            for (int i = report.Count - 1; i >= 0 ; i--)
            {
                var data = report[i];

                if (data.EscapeTime <= escapeTime)
                {
                    result.Add(data);
                    report.RemoveAt(i);
                }
                else
                {
                    break;
                }

            }
            return result;
        }

        public int Count()
        {
            return this.report.Count;
        }
    }
}