using System;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Forms;
using AdvancePharmacyPOS.Data.Repositories;
using AdvancePharmacyPOS.Services;

namespace AdvancePharmacyPOS.UI.Forms
{
    public class SalesReportForm : Form
    {
        private readonly ReportsRepository _repo = new ReportsRepository();
        private readonly DateTimePicker dtFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today };
        private readonly DateTimePicker dtTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(1).AddSeconds(-1) };
        private readonly Button btnLoad = new Button { Text = "Load" };
        private readonly Button btnExport = new Button { Text = "Export CSV" };
        private readonly Button btnPrint = new Button { Text = "Print" };
        private readonly DataGridView grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
        private DataTable current;

        public SalesReportForm()
        {
            Text = "Sales Report";
            Width = 900; Height = 600;
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
            top.Controls.AddRange(new Control[] { new Label { Text = "From:" }, dtFrom, new Label { Text = "To:" }, dtTo, btnLoad, btnExport, btnPrint });
            Controls.Add(grid);
            Controls.Add(top);
            btnLoad.Click += (s, e) => LoadData();
            btnExport.Click += (s, e) => Export();
            btnPrint.Click += (s, e) => Print();
            Load += (s, e) => LoadData();
        }

        private void LoadData()
        {
            current = _repo.GetSalesSummary(dtFrom.Value.Date, dtTo.Value.Date.AddDays(1).AddSeconds(-1));
            grid.DataSource = current;
        }

        private void Export()
        {
            if (current == null) return;
            using (var sfd = new SaveFileDialog { Filter = "CSV Files|*.csv", FileName = $"Sales_{DateTime.Now:yyyyMMdd}.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportService.ExportToCsv(current, sfd.FileName);
                    MessageBox.Show("Exported.");
                }
            }
        }

        private void Print()
        {
            if (current == null) return;
            var pd = new PrintDocument();
            int rowIndex = 0;
            pd.PrintPage += (s, e) =>
            {
                float y = e.MarginBounds.Top;
                var font = System.Drawing.SystemFonts.DefaultFont;
                e.Graphics.DrawString("Sales Report", new System.Drawing.Font(font, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, e.MarginBounds.Left, y);
                y += 30;
                while (rowIndex < current.Rows.Count && y < e.MarginBounds.Bottom)
                {
                    var r = current.Rows[rowIndex];
                    string line = $"{r["InvoiceNo"]}  {Convert.ToDateTime(r["SaleDate"]).ToString("g")}  {r["Cashier"]}  {Convert.ToDecimal(r["GrandTotal"]).ToString("0.00")}";
                    e.Graphics.DrawString(line, font, System.Drawing.Brushes.Black, e.MarginBounds.Left, y);
                    y += 20;
                    rowIndex++;
                }
                e.HasMorePages = rowIndex < current.Rows.Count;
            };
            var dlg = new PrintPreviewDialog { Document = pd, Width = 900, Height = 700 };
            dlg.ShowDialog();
        }
    }
}