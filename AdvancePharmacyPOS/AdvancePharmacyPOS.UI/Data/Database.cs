using System.Configuration;
using System.Data.SqlClient;

namespace AdvancePharmacyPOS.Data
{
    public static class Database
    {
        public static SqlConnection GetOpenConnection()
        {
            var connStr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            var connection = new SqlConnection(connStr);
            connection.Open();
            return connection;
        }
    }
}