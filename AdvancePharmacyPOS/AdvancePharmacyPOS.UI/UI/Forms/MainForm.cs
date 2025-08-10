using System;
using System.Windows.Forms;
using AdvancePharmacyPOS.Models;

namespace AdvancePharmacyPOS.UI.Forms
{
    public class MainForm : Form
    {
        private readonly User _currentUser;
        private readonly MenuStrip _menu = new MenuStrip();

        public MainForm(User currentUser)
        {
            _currentUser = currentUser;
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            Text = $"Advance Pharmacy POS - Logged in as {_currentUser.Username} ({_currentUser.Role})";
            BuildMenu();
            Controls.Add(_menu);
            MainMenuStrip = _menu;
        }

        private void BuildMenu()
        {
            var mMasters = new ToolStripMenuItem("Masters");
            var mProducts = new ToolStripMenuItem("Products", null, (s, e) => OpenChild(new ProductsForm()));
            mMasters.DropDownItems.Add(mProducts);

            var mTransactions = new ToolStripMenuItem("Transactions");
            var mPos = new ToolStripMenuItem("Billing / POS", null, (s, e) => OpenChild(new POSForm(_currentUser)));
            mTransactions.DropDownItems.Add(mPos);

            var mReports = new ToolStripMenuItem("Reports");
            var mSales = new ToolStripMenuItem("Sales Report", null, (s, e) => OpenChild(new SalesReportForm()));
            mReports.DropDownItems.Add(mSales);

            var mTools = new ToolStripMenuItem("Tools");
            var mBackup = new ToolStripMenuItem("Backup / Restore", null, (s, e) => OpenChild(new BackupRestoreForm()));
            mTools.DropDownItems.Add(mBackup);

            _menu.Items.AddRange(new ToolStripItem[] { mMasters, mTransactions, mReports, mTools });
        }

        private void OpenChild(Form form)
        {
            form.MdiParent = this;
            form.Show();
        }
    }
}