using System;
using System.Drawing;
using System.Windows.Forms;
using AdvancePharmacyPOS.Models;
using AdvancePharmacyPOS.Services;

namespace AdvancePharmacyPOS.UI.Forms
{
    public class LoginForm : Form
    {
        private readonly TextBox _txtUsername = new TextBox { Width = 200 };
        private readonly TextBox _txtPassword = new TextBox { Width = 200, UseSystemPasswordChar = true };
        private readonly Button _btnLogin = new Button { Text = "Login", Width = 100 };
        private readonly Label _lblMessage = new Label { ForeColor = Color.Red, AutoSize = true };
        private readonly AuthService _auth = new AuthService();

        public User LoggedInUser { get; private set; }

        public LoginForm()
        {
            Text = "Login - Advance Pharmacy POS";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(360, 200);

            var lblU = new Label { Text = "Username", AutoSize = true };
            var lblP = new Label { Text = "Password", AutoSize = true };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(16)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(lblU, 0, 0);
            layout.Controls.Add(_txtUsername, 1, 0);
            layout.Controls.Add(lblP, 0, 1);
            layout.Controls.Add(_txtPassword, 1, 1);
            layout.Controls.Add(_lblMessage, 1, 2);
            layout.Controls.Add(_btnLogin, 1, 3);

            _btnLogin.Click += OnLoginClicked;
            _txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) OnLoginClicked(s, e); };

            Controls.Add(layout);
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var user = _auth.ValidateLogin(_txtUsername.Text.Trim(), _txtPassword.Text);
                if (user == null)
                {
                    _lblMessage.Text = "Invalid username or password";
                    return;
                }
                LoggedInUser = user;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}