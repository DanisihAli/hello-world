using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace AdvancePharmacyPOS.Services
{
    public static class ExportService
    {
        public static void ExportToCsv(DataTable table, string path)
        {
            var sb = new StringBuilder();
            var cols = table.Columns.Cast<DataColumn>().Select(c => Escape(c.ColumnName));
            sb.AppendLine(string.Join(",", cols));
            foreach (DataRow row in table.Rows)
            {
                var vals = row.ItemArray.Select(v => Escape(v?.ToString() ?? string.Empty));
                sb.AppendLine(string.Join(",", vals));
            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private static string Escape(string s)
        {
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
            {
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            }
            return s;
        }
    }
}