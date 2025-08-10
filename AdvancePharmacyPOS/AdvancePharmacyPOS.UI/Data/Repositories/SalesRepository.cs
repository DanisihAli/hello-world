using System.Collections.Generic;
using System.Data;
using Dapper;
using AdvancePharmacyPOS.Data;
using AdvancePharmacyPOS.Models;

namespace AdvancePharmacyPOS.Data.Repositories
{
    public class SalesRepository
    {
        public int CreateSale(Sale sale, IEnumerable<SaleItem> items)
        {
            using (var conn = Database.GetOpenConnection())
            using (var tx = conn.BeginTransaction())
            {
                var saleId = conn.ExecuteScalar<int>(@"INSERT INTO Sales (CustomerId, InvoiceNo, SaleDate, Subtotal, Discount, TaxTotal, GrandTotal, PaidAmount, ChangeAmount, UserId)
VALUES (@CustomerId, @InvoiceNo, @SaleDate, @Subtotal, @Discount, @TaxTotal, @GrandTotal, @PaidAmount, @ChangeAmount, @UserId);
SELECT CAST(SCOPE_IDENTITY() as int);", sale, tx);
                foreach (var it in items)
                {
                    it.SaleId = saleId;
                    conn.Execute(@"INSERT INTO SalesItems (SaleId, ProductId, Quantity, UnitPrice, Discount, TaxPercent, LineTotal)
VALUES(@SaleId, @ProductId, @Quantity, @UnitPrice, @Discount, @TaxPercent, @LineTotal);", it, tx);

                    conn.Execute(@"INSERT INTO StockMovements (ProductId, Quantity, MovementType, Reference, MovementDate, Note)
VALUES (@ProductId, @Qty, 'OUT', @Ref, @Dt, 'Sale');",
                        new { ProductId = it.ProductId, Qty = it.Quantity, Ref = sale.InvoiceNo, Dt = sale.SaleDate }, tx);
                }
                tx.Commit();
                return saleId;
            }
        }
    }
}