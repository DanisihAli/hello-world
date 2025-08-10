using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using AdvancePharmacyPOS.Data.Repositories;
using AdvancePharmacyPOS.Models;

namespace AdvancePharmacyPOS.UI.Forms
{
    public class ProductsForm : Form
    {
        private readonly ProductsRepository _repo = new ProductsRepository();
        private readonly BindingList<Product> _binding = new BindingList<Product>();
        private readonly DataGridView _grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
        private readonly Button _btnAdd = new Button { Text = "Add" };
        private readonly Button _btnEdit = new Button { Text = "Edit" };

        public ProductsForm()
        {
            Text = "Products";
            Width = 900;
            Height = 600;

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
            panel.Controls.AddRange(new Control[] { _btnAdd, _btnEdit });

            Controls.Add(_grid);
            Controls.Add(panel);

            _grid.DataSource = _binding;

            Load += (s, e) => RefreshData();
            _btnAdd.Click += (s, e) => AddOrEdit(null);
            _btnEdit.Click += (s, e) =>
            {
                if (_grid.CurrentRow?.DataBoundItem is Product p)
                    AddOrEdit(p);
            };
        }

        private void RefreshData()
        {
            _binding.Clear();
            foreach (var p in _repo.GetAll()) _binding.Add(p);
        }

        private void AddOrEdit(Product product)
        {
            var dlg = new ProductEditDialog(product);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.Edited.ProductId == 0)
                {
                    dlg.Edited.ProductId = _repo.Create(dlg.Edited);
                }
                else
                {
                    _repo.Update(dlg.Edited);
                }
                RefreshData();
            }
        }
    }

    internal class ProductEditDialog : Form
    {
        private readonly TextBox txtName = new TextBox();
        private readonly TextBox txtSku = new TextBox();
        private readonly TextBox txtBarcode = new TextBox();
        private readonly NumericUpDown numCategoryId = new NumericUpDown { Minimum = 1, Maximum = 1000000, DecimalPlaces = 0 };
        private readonly NumericUpDown numPurchase = new NumericUpDown { DecimalPlaces = 2, Maximum = 1000000 };
        private readonly NumericUpDown numSale = new NumericUpDown { DecimalPlaces = 2, Maximum = 1000000 };
        private readonly TextBox txtUnit = new TextBox();
        private readonly NumericUpDown numTax = new NumericUpDown { DecimalPlaces = 2, Maximum = 100, Increment = 0.5M };
        private readonly DateTimePicker dtExpiry = new DateTimePicker { Format = DateTimePickerFormat.Short, ShowCheckBox = true };
        private readonly CheckBox chkActive = new CheckBox { Text = "Active", Checked = true };
        private readonly Button btnOk = new Button { Text = "Save" };

        public Product Edited { get; private set; }

        public ProductEditDialog(Product product)
        {
            Text = product == null ? "Add Product" : "Edit Product";
            Width = 400; Height = 420; StartPosition = FormStartPosition.CenterParent;

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(10) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            void addRow(string label, Control c)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layout.Controls.Add(new Label { Text = label, AutoSize = true }, 0, layout.RowCount);
                layout.Controls.Add(c, 1, layout.RowCount);
                layout.RowCount++;
            }

            addRow("Name", txtName);
            addRow("SKU", txtSku);
            addRow("Barcode", txtBarcode);
            addRow("CategoryId", numCategoryId);
            addRow("Purchase Price", numPurchase);
            addRow("Sale Price", numSale);
            addRow("Unit", txtUnit);
            addRow("Tax %", numTax);
            addRow("Expiry Date", dtExpiry);
            addRow("", chkActive);

            layout.Controls.Add(btnOk, 1, layout.RowCount);

            Controls.Add(layout);

            Edited = product != null ? new Product
            {
                ProductId = product.ProductId,
                Name = product.Name,
                SKU = product.SKU,
                Barcode = product.Barcode,
                CategoryId = product.CategoryId,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                Unit = product.Unit,
                TaxPercent = product.TaxPercent,
                ExpiryDate = product.ExpiryDate,
                IsActive = product.IsActive
            } : new Product { IsActive = true };

            txtName.Text = Edited.Name;
            txtSku.Text = Edited.SKU;
            txtBarcode.Text = Edited.Barcode;
            numCategoryId.Value = Edited.CategoryId <= 0 ? 1 : Edited.CategoryId;
            numPurchase.Value = Edited.PurchasePrice;
            numSale.Value = Edited.SalePrice;
            txtUnit.Text = Edited.Unit;
            numTax.Value = Edited.TaxPercent;
            if (Edited.ExpiryDate.HasValue)
            {
                dtExpiry.Checked = true;
                dtExpiry.Value = Edited.ExpiryDate.Value;
            }
            chkActive.Checked = Edited.IsActive;

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Name is required"); return; }
                Edited.Name = txtName.Text.Trim();
                Edited.SKU = txtSku.Text.Trim();
                Edited.Barcode = txtBarcode.Text.Trim();
                Edited.CategoryId = (int)numCategoryId.Value;
                Edited.PurchasePrice = numPurchase.Value;
                Edited.SalePrice = numSale.Value;
                Edited.Unit = txtUnit.Text.Trim();
                Edited.TaxPercent = numTax.Value;
                Edited.ExpiryDate = dtExpiry.Checked ? (System.DateTime?)dtExpiry.Value.Date : null;
                Edited.IsActive = chkActive.Checked;
                DialogResult = DialogResult.OK;
                Close();
            };
        }
    }
}