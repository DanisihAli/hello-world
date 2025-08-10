using System.Collections.Generic;
using System.Linq;
using Dapper;
using AdvancePharmacyPOS.Data;
using AdvancePharmacyPOS.Models;

namespace AdvancePharmacyPOS.Data.Repositories
{
    public class ProductsRepository
    {
        public IEnumerable<Product> GetAll()
        {
            using (var conn = Database.GetOpenConnection())
            {
                return conn.Query<Product>("SELECT * FROM Products WHERE IsActive=1 ORDER BY Name");
            }
        }

        public int Create(Product product)
        {
            const string sql = @"INSERT INTO Products (Name, SKU, Barcode, CategoryId, PurchasePrice, SalePrice, Unit, TaxPercent, ExpiryDate, IsActive)
VALUES (@Name, @SKU, @Barcode, @CategoryId, @PurchasePrice, @SalePrice, @Unit, @TaxPercent, @ExpiryDate, @IsActive);
SELECT CAST(SCOPE_IDENTITY() as int);";
            using (var conn = Database.GetOpenConnection())
            {
                return conn.ExecuteScalar<int>(sql, product);
            }
        }

        public void Update(Product product)
        {
            const string sql = @"UPDATE Products SET Name=@Name, SKU=@SKU, Barcode=@Barcode, CategoryId=@CategoryId, PurchasePrice=@PurchasePrice, SalePrice=@SalePrice, Unit=@Unit, TaxPercent=@TaxPercent, ExpiryDate=@ExpiryDate, IsActive=@IsActive WHERE ProductId=@ProductId";
            using (var conn = Database.GetOpenConnection())
            {
                conn.Execute(sql, product);
            }
        }

        public Product FindByBarcodeOrSku(string code)
        {
            using (var conn = Database.GetOpenConnection())
            {
                return conn.Query<Product>("SELECT TOP 1 * FROM Products WHERE IsActive=1 AND (Barcode=@c OR SKU=@c)", new { c = code }).FirstOrDefault();
            }
        }
    }
}