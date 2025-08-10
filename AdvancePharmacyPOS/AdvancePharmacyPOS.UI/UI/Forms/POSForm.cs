using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using AdvancePharmacyPOS.Data.Repositories;
using AdvancePharmacyPOS.Models;
using AdvancePharmacyPOS.Services;

namespace AdvancePharmacyPOS.UI.Forms
{
    public class POSForm : Form
    {
        private readonly User _currentUser;
        private readonly ProductsRepository _productsRepo = new ProductsRepository();
        private readonly BillingService _billing = new BillingService();

        private readonly TextBox txtScan = new TextBox { Dock = DockStyle.Fill };
        private readonly DataGridView grid = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = false };
        private readonly NumericUpDown numDiscount = new NumericUpDown { DecimalPlaces = 2, Maximum = 100, Dock = DockStyle.Right, Width = 100 };
        private readonly Label lblSubtotal = new Label { AutoSize = true };
        private readonly Label lblTax = new Label { AutoSize = true };
        private readonly Label lblTotal = new Label { AutoSize = true, Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold) };
        private readonly Button btnSave = new Button { Text = "Save & Print", Dock = DockStyle.Right, Width = 140 };

        private BindingList<ItemRow> items = new BindingList<ItemRow>();

        public POSForm(User currentUser)
        {
            _currentUser = currentUser;
            Text = "Billing / POS";
            Width = 1000; Height = 700;

            var top = new TableLayoutPanel { Dock = DockStyle.Top, Height = 40, ColumnCount = 4 };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            top.Controls.Add(new Label { Text = "Scan Barcode / Enter SKU", AutoSize = true, Padding = new Padding(8, 10, 8, 8) }, 0, 0);
            top.Controls.Add(txtScan, 1, 0);
            top.SetColumnSpan(txtScan, 3);

            BuildGrid();

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, FlowDirection = FlowDirection.RightToLeft };
            bottom.Controls.Add(btnSave);
            bottom.Controls.Add(new Label { Text = "Discount %:", AutoSize = true, Padding = new Padding(8, 10, 8, 8) });
            bottom.Controls.Add(numDiscount);
            bottom.Controls.Add(new Label { Text = "Total:", AutoSize = true, Padding = new Padding(8, 10, 8, 8) });
            bottom.Controls.Add(lblTotal);
            bottom.Controls.Add(new Label { Text = "Tax:", AutoSize = true, Padding = new Padding(8, 10, 8, 8) });
            bottom.Controls.Add(lblTax);
            bottom.Controls.Add(new Label { Text = "Subtotal:", AutoSize = true, Padding = new Padding(8, 10, 8, 8) });
            bottom.Controls.Add(lblSubtotal);

            Controls.Add(grid);
            Controls.Add(top);
            Controls.Add(bottom);

            grid.DataSource = items;
            txtScan.KeyDown += OnScan;
            grid.CellEndEdit += (s, e) => Recalculate();
            numDiscount.ValueChanged += (s, e) => Recalculate();
            btnSave.Click += OnSave;
        }

        private void BuildGrid()
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ProductId", DataPropertyName = nameof(ItemRow.ProductId), Visible = false });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = nameof(ItemRow.Name), ReadOnly = true, Width = 250 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Price", DataPropertyName = nameof(ItemRow.UnitPrice), Width = 90 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Qty", DataPropertyName = nameof(ItemRow.Quantity), Width = 60 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Discount", DataPropertyName = nameof(ItemRow.Discount), Width = 80 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tax %", DataPropertyName = nameof(ItemRow.TaxPercent), Width = 70 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total", DataPropertyName = nameof(ItemRow.LineTotal), ReadOnly = true, Width = 100 });
        }

        private void OnScan(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var code = txtScan.Text.Trim();
            if (string.IsNullOrEmpty(code)) return;
            var p = _productsRepo.FindByBarcodeOrSku(code);
            if (p == null)
            {
                MessageBox.Show("Product not found");
                return;
            }
            var row = items.FirstOrDefault(x => x.ProductId == p.ProductId);
            if (row == null)
            {
                row = new ItemRow
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    UnitPrice = p.SalePrice,
                    Quantity = 1,
                    Discount = 0,
                    TaxPercent = p.TaxPercent
                };
                items.Add(row);
            }
            else
            {
                row.Quantity += 1;
            }
            txtScan.Clear();
            Recalculate();
        }

        private void Recalculate()
        {
            foreach (var it in items)
            {
                var beforeTax = (it.UnitPrice * it.Quantity) - it.Discount;
                var tax = Math.Round(beforeTax * (it.TaxPercent / 100m), 2);
                it.LineTotal = Math.Round(beforeTax + tax, 2);
            }
            grid.Refresh();

            var subtotal = items.Sum(i => (i.UnitPrice * i.Quantity) - i.Discount);
            var taxTotal = items.Sum(i => Math.Round(((i.UnitPrice * i.Quantity) - i.Discount) * (i.TaxPercent / 100m), 2));
            var discountAmt = Math.Round(subtotal * (numDiscount.Value / 100m), 2);
            subtotal -= discountAmt;
            var grand = Math.Round(subtotal + taxTotal, 2);

            lblSubtotal.Text = subtotal.ToString("0.00");
            lblTax.Text = taxTotal.ToString("0.00");
            lblTotal.Text = grand.ToString("0.00");
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (items.Count == 0) { MessageBox.Show("No items"); return; }
            var saleItems = items.Select(i => new SaleItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TaxPercent = i.TaxPercent
            }).ToList();
            var result = _billing.CreateInvoice(null, _currentUser.UserId, saleItems, numDiscount.Value);
            MessageBox.Show($"Invoice {result.InvoiceNo} saved. SaleId={result.SaleId} Total={result.GrandTotal:0.00}");
            PrintReceipt(result.InvoiceNo);
            items.Clear();
            Recalculate();
        }

        private void PrintReceipt(string invoiceNo)
        {
            var pd = new PrintDocument();
            pd.PrintPage += (s, e) =>
            {
                float y = e.MarginBounds.Top;
                var font = SystemFonts.DefaultFont;
                e.Graphics.DrawString("Advance Pharmacy", new Font(font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, y); y += 20;
                e.Graphics.DrawString($"Invoice: {invoiceNo}", font, Brushes.Black, e.MarginBounds.Left, y); y += 20;
                e.Graphics.DrawString($"Date: {DateTime.Now:g}", font, Brushes.Black, e.MarginBounds.Left, y); y += 20;
                e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, y, e.MarginBounds.Right, y); y += 10;
                foreach (var it in items)
                {
                    string line = $"{it.Name}  x{it.Quantity}  @ {it.UnitPrice:0.00}  = {it.LineTotal:0.00}";
                    e.Graphics.DrawString(line, font, Brushes.Black, e.MarginBounds.Left, y); y += 18;
                }
                y += 10;
                e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, y, e.MarginBounds.Right, y); y += 10;
                e.Graphics.DrawString($"Total: {lblTotal.Text}", new Font(font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, y);
            };
            var preview = new PrintPreviewDialog { Document = pd, Width = 800, Height = 600 };
            preview.ShowDialog();
        }

        private class ItemRow : System.ComponentModel.INotifyPropertyChanged
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Quantity { get; set; }
            public decimal Discount { get; set; }
            public decimal TaxPercent { get; set; }

            private decimal _lineTotal;
            public decimal LineTotal
            {
                get => _lineTotal;
                set { _lineTotal = value; PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(LineTotal))); }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        }
    }
}