using System;
using System.Collections.Generic;
using AdvancePharmacyPOS.Data.Repositories;
using AdvancePharmacyPOS.Models;

namespace AdvancePharmacyPOS.Services
{
    public class BillingResult
    {
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class BillingService
    {
        private readonly SalesRepository _salesRepository;

        public BillingService()
        {
            _salesRepository = new SalesRepository();
        }

        public BillingResult CreateInvoice(int? customerId, int userId, List<SaleItem> items, decimal discountPercent)
        {
            decimal subtotal = 0m;
            decimal taxTotal = 0m;
            foreach (var it in items)
            {
                var lineBeforeTax = (it.UnitPrice * it.Quantity) - it.Discount;
                var lineTax = Math.Round(lineBeforeTax * (it.TaxPercent / 100m), 2);
                it.LineTotal = Math.Round(lineBeforeTax + lineTax, 2);
                subtotal += lineBeforeTax;
                taxTotal += lineTax;
            }
            var discountAmount = Math.Round(subtotal * (discountPercent / 100m), 2);
            subtotal -= discountAmount;
            var grandTotal = Math.Round(subtotal + taxTotal, 2);
            var invoiceNo = GenerateInvoiceNo();
            var sale = new Sale
            {
                CustomerId = customerId,
                InvoiceNo = invoiceNo,
                SaleDate = DateTime.Now,
                Subtotal = subtotal,
                Discount = discountAmount,
                TaxTotal = taxTotal,
                GrandTotal = grandTotal,
                PaidAmount = grandTotal,
                ChangeAmount = 0,
                UserId = userId
            };
            var saleId = _salesRepository.CreateSale(sale, items);
            return new BillingResult { SaleId = saleId, InvoiceNo = invoiceNo, GrandTotal = grandTotal };
        }

        private string GenerateInvoiceNo()
        {
            return $"INV-{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}