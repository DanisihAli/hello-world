using System;
using System.Windows.Forms;

namespace AdvancePharmacyPOS
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var login = new UI.Forms.LoginForm())
            {
                var result = login.ShowDialog();
                if (result == DialogResult.OK && login.LoggedInUser != null)
                {
                    Application.Run(new UI.Forms.MainForm(login.LoggedInUser));
                }
            }
        }
    }
}