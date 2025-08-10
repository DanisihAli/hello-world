using System;
using System.Data;
using System.Collections.Generic;
using Dapper;
using AdvancePharmacyPOS.Data;

namespace AdvancePharmacyPOS.Data.Repositories
{
    public class ReportsRepository
    {
        public DataTable GetSalesSummary(DateTime from, DateTime to)
        {
            using (var conn = Database.GetOpenConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT s.InvoiceNo, s.SaleDate, s.GrandTotal, u.Username AS Cashier
FROM Sales s
JOIN Users u ON u.UserId = s.UserId
WHERE s.SaleDate BETWEEN @f AND @t
ORDER BY s.SaleDate DESC";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@f", from));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@t", to));
                using (var reader = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }
    }
}