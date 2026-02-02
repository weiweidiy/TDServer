using System.Collections.Generic;
using System.Threading.Tasks;


namespace JFramework
{
    public class CombatReprotParser
    {
        List<ICombatReportData> report = new List<ICombatReportData>();
        public CombatReprotParser(List<ICombatReportData> report)
        {
            //foreach (var data in report)
            //{
            //    this.report.Add(data.Clone() as ICombatReportData);
            //}

            //this.report.Reverse();
        }

        public Task LoadData(List<ICombatReportData> report)
        {
            foreach (var data in report)
            {
                this.report.Add(data.Clone() as ICombatReportData);
            }

            this.report.Reverse();

            return Task.CompletedTask;
        }

        public List<ICombatReportData> GetData(float escapeTime)
        {
            var result = new List<ICombatReportData>();
            for (int i = report.Count - 1; i >= 0; i--)
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
