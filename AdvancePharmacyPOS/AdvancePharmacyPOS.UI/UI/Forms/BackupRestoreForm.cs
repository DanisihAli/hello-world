using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using AdvancePharmacyPOS.Data;

namespace AdvancePharmacyPOS.UI.Forms
{
    public class BackupRestoreForm : Form
    {
        private readonly TextBox txtBackupPath = new TextBox { Width = 400 };
        private readonly Button btnBrowseBackup = new Button { Text = "Browse..." };
        private readonly Button btnBackup = new Button { Text = "Backup Database" };

        private readonly TextBox txtRestorePath = new TextBox { Width = 400 };
        private readonly Button btnBrowseRestore = new Button { Text = "Browse..." };
        private readonly Button btnRestore = new Button { Text = "Restore Database" };

        public BackupRestoreForm()
        {
            Text = "Backup / Restore Database";
            Width = 650; Height = 220; StartPosition = FormStartPosition.CenterParent;

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, Padding = new Padding(10) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new Label { Text = "Backup to (.bak):", AutoSize = true }, 0, 0);
            layout.Controls.Add(txtBackupPath, 1, 0);
            layout.Controls.Add(btnBrowseBackup, 2, 0);
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(btnBackup, 2, 1);

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new Label { Text = "Restore from (.bak):", AutoSize = true }, 0, 2);
            layout.Controls.Add(txtRestorePath, 1, 2);
            layout.Controls.Add(btnBrowseRestore, 2, 2);
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(btnRestore, 2, 3);

            Controls.Add(layout);

            btnBrowseBackup.Click += (s, e) =>
            {
                using (var sfd = new SaveFileDialog { Filter = "SQL Backup|*.bak", FileName = "POSPharmacy.bak" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        txtBackupPath.Text = sfd.FileName;
                    }
                }
            };

            btnBrowseRestore.Click += (s, e) =>
            {
                using (var ofd = new OpenFileDialog { Filter = "SQL Backup|*.bak" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtRestorePath.Text = ofd.FileName;
                    }
                }
            };

            btnBackup.Click += async (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBackupPath.Text)) { MessageBox.Show("Select backup path"); return; }
                try
                {
                    using (var conn = Database.GetOpenConnection())
                    using (var cmd = conn.CreateCommand())
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(txtBackupPath.Text));
                        cmd.CommandText = "BACKUP DATABASE POSPharmacy TO DISK = @path WITH INIT";
                        cmd.Parameters.Add(new SqlParameter("@path", txtBackupPath.Text));
                        await cmd.ExecuteNonQueryAsync();
                    }
                    MessageBox.Show("Backup completed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Backup failed: " + ex.Message);
                }
            };

            btnRestore.Click += async (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtRestorePath.Text)) { MessageBox.Show("Select .bak file"); return; }
                try
                {
                    using (var conn = Database.GetOpenConnection())
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"ALTER DATABASE POSPharmacy SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE POSPharmacy FROM DISK = @path WITH REPLACE;
ALTER DATABASE POSPharmacy SET MULTI_USER;";
                        cmd.Parameters.Add(new SqlParameter("@path", txtRestorePath.Text));
                        await cmd.ExecuteNonQueryAsync();
                    }
                    MessageBox.Show("Restore completed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Restore failed: " + ex.Message);
                }
            };
        }
    }
}