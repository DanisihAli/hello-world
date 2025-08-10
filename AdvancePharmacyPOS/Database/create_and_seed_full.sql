/* Create and seed full schema for Advance Pharmacy POS */
IF DB_ID('POSPharmacy') IS NULL
BEGIN
    CREATE DATABASE POSPharmacy;
END
GO

USE POSPharmacy;
GO

/* Drop existing tables for a clean seed (order respects FKs) */
IF OBJECT_ID('dbo.SalesItems','U') IS NOT NULL DROP TABLE dbo.SalesItems;
IF OBJECT_ID('dbo.Sales','U') IS NOT NULL DROP TABLE dbo.Sales;
IF OBJECT_ID('dbo.PurchaseItems','U') IS NOT NULL DROP TABLE dbo.PurchaseItems;
IF OBJECT_ID('dbo.Purchases','U') IS NOT NULL DROP TABLE dbo.Purchases;
IF OBJECT_ID('dbo.StockMovements','U') IS NOT NULL DROP TABLE dbo.StockMovements;
IF OBJECT_ID('dbo.Products','U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Categories','U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID('dbo.Customers','U') IS NOT NULL DROP TABLE dbo.Customers;
IF OBJECT_ID('dbo.Suppliers','U') IS NOT NULL DROP TABLE dbo.Suppliers;
IF OBJECT_ID('dbo.Payments','U') IS NOT NULL DROP TABLE dbo.Payments;
IF OBJECT_ID('dbo.CompanyInfo','U') IS NOT NULL DROP TABLE dbo.CompanyInfo;
IF OBJECT_ID('dbo.Currencies','U') IS NOT NULL DROP TABLE dbo.Currencies;
IF OBJECT_ID('dbo.SmsSettings','U') IS NOT NULL DROP TABLE dbo.SmsSettings;
IF OBJECT_ID('dbo.Users','U') IS NOT NULL DROP TABLE dbo.Users;
GO

/* Masters */
CREATE TABLE dbo.CompanyInfo (
    CompanyId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(200) NOT NULL,
    Address NVARCHAR(400) NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(100) NULL,
    TaxNumber NVARCHAR(100) NULL
);

CREATE TABLE dbo.Currencies (
    CurrencyId INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(10) NOT NULL,
    Symbol NVARCHAR(10) NULL,
    Name NVARCHAR(50) NOT NULL,
    IsDefault BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.SmsSettings (
    SmsSettingsId INT IDENTITY(1,1) PRIMARY KEY,
    ProviderName NVARCHAR(100) NULL,
    ApiKey NVARCHAR(200) NULL,
    SenderId NVARCHAR(50) NULL,
    IsEnabled BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(128) NOT NULL,
    PasswordSalt NVARCHAR(64) NOT NULL,
    FullName NVARCHAR(100) NULL,
    Role INT NOT NULL, -- 1=Admin,2=Cashier
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE dbo.Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE dbo.Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    SKU NVARCHAR(50) NULL,
    Barcode NVARCHAR(50) NULL,
    CategoryId INT NOT NULL,
    PurchasePrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    SalePrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    Unit NVARCHAR(50) NULL,
    TaxPercent DECIMAL(9,2) NOT NULL DEFAULT 0,
    ExpiryDate DATE NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE dbo.Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(300) NULL,
    OpeningBalance DECIMAL(18,2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE dbo.Suppliers (
    SupplierId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(300) NULL,
    OpeningBalance DECIMAL(18,2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE dbo.StockMovements (
    StockMovementId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    MovementType NVARCHAR(10) NOT NULL, -- IN/OUT
    Reference NVARCHAR(100) NULL,
    MovementDate DATETIME NOT NULL,
    Note NVARCHAR(200) NULL
);

/* Purchases */
CREATE TABLE dbo.Purchases (
    PurchaseId INT IDENTITY(1,1) PRIMARY KEY,
    SupplierId INT NOT NULL,
    InvoiceNo NVARCHAR(50) NOT NULL,
    PurchaseDate DATETIME NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) NOT NULL,
    TaxTotal DECIMAL(18,2) NOT NULL,
    GrandTotal DECIMAL(18,2) NOT NULL,
    UserId INT NOT NULL
);

CREATE TABLE dbo.PurchaseItems (
    PurchaseItemId INT IDENTITY(1,1) PRIMARY KEY,
    PurchaseId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    UnitCost DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) NOT NULL,
    TaxPercent DECIMAL(9,2) NOT NULL,
    LineTotal DECIMAL(18,2) NOT NULL
);

/* Sales */
CREATE TABLE dbo.Sales (
    SaleId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NULL,
    InvoiceNo NVARCHAR(50) NOT NULL,
    SaleDate DATETIME NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) NOT NULL,
    TaxTotal DECIMAL(18,2) NOT NULL,
    GrandTotal DECIMAL(18,2) NOT NULL,
    PaidAmount DECIMAL(18,2) NOT NULL,
    ChangeAmount DECIMAL(18,2) NOT NULL,
    UserId INT NOT NULL
);

CREATE TABLE dbo.SalesItems (
    SaleItemId INT IDENTITY(1,1) PRIMARY KEY,
    SaleId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) NOT NULL,
    TaxPercent DECIMAL(9,2) NOT NULL,
    LineTotal DECIMAL(18,2) NOT NULL
);

/* Payments */
CREATE TABLE dbo.Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    PaymentDate DATETIME NOT NULL,
    PartyType NVARCHAR(20) NOT NULL, -- Customer/Supplier
    CustomerId INT NULL,
    SupplierId INT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Note NVARCHAR(200) NULL
);

/* Sample Data */
INSERT INTO dbo.CompanyInfo (CompanyName, Address, Phone, Email, TaxNumber)
VALUES ('Advance Pharmacy', '100 Health St, City', '555-0000', 'info@advancepharmacy.com', 'TAX-12345');

INSERT INTO dbo.Currencies (Code, Symbol, Name, IsDefault) VALUES ('USD', '$', 'US Dollar', 1);
INSERT INTO dbo.SmsSettings (ProviderName, ApiKey, SenderId, IsEnabled) VALUES ('', '', '', 0);

-- Users
DECLARE @salt1 NVARCHAR(64) = 'a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6';
DECLARE @salt2 NVARCHAR(64) = '1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d';
DECLARE @adminHash NVARCHAR(128) = LOWER(CONVERT(VARCHAR(128), HASHBYTES('SHA2_256', 'admin123'+@salt1), 2));
DECLARE @cashierHash NVARCHAR(128) = LOWER(CONVERT(VARCHAR(128), HASHBYTES('SHA2_256', 'cashier123'+@salt2), 2));
INSERT INTO dbo.Users (Username, PasswordHash, PasswordSalt, FullName, Role, IsActive)
VALUES ('admin', @adminHash, @salt1, 'Administrator', 1, 1),
       ('cashier', @cashierHash, @salt2, 'Cashier User', 2, 1);

-- Categories
INSERT INTO dbo.Categories (Name) VALUES ('Analgesics'), ('Antibiotics'), ('Vitamins');

-- Products (5 sample)
INSERT INTO dbo.Products (Name, SKU, Barcode, CategoryId, PurchasePrice, SalePrice, Unit, TaxPercent, ExpiryDate, IsActive)
VALUES
('Paracetamol 500mg', 'SKU001', 'BAR001', 1, 1.50, 2.50, 'Tablet', 5, '2026-12-31', 1),
('Amoxicillin 250mg', 'SKU002', 'BAR002', 2, 3.00, 5.00, 'Capsule', 5, '2026-09-30', 1),
('Vitamin C 1000mg', 'SKU003', 'BAR003', 3, 4.00, 6.50, 'Tablet', 5, '2027-03-31', 1),
('Cough Syrup 100ml', 'SKU004', 'BAR004', 1, 2.00, 3.50, 'Bottle', 5, '2026-05-31', 1),
('Ibuprofen 200mg', 'SKU005', 'BAR005', 1, 1.80, 3.00, 'Tablet', 5, '2026-11-30', 1);

-- Customers (3)
INSERT INTO dbo.Customers (Name, Phone, Email, Address)
VALUES ('John Doe', '555-1001', 'john@example.com', '123 Main St'),
       ('Mary Smith', '555-1002', 'mary@example.com', '456 Oak Ave'),
       ('Acme Clinic', '555-1003', 'procurement@acmeclinic.com', '789 Pine Rd');

-- Suppliers (2)
INSERT INTO dbo.Suppliers (Name, Phone, Email, Address)
VALUES ('MedSupply Co', '555-2001', 'sales@medsupply.com', '10 Pharma Park'),
       ('HealthWholesale', '555-2002', 'info@healthwholesale.com', '22 Wellness Way');

-- Initial stock IN via StockMovements
INSERT INTO dbo.StockMovements (ProductId, Quantity, MovementType, Reference, MovementDate, Note)
SELECT ProductId, 100, 'IN', 'SEED', GETDATE(), 'Initial stock' FROM dbo.Products;

-- Sample purchase (Purchase Daybook)
DECLARE @pNow DATETIME = DATEADD(DAY, -2, GETDATE());
INSERT INTO dbo.Purchases (SupplierId, InvoiceNo, PurchaseDate, Subtotal, Discount, TaxTotal, GrandTotal, UserId)
VALUES (1, 'PINV-20250101080000', @pNow, 100.00, 5.00, 4.75, 99.75, 1);
DECLARE @pid INT = SCOPE_IDENTITY();
INSERT INTO dbo.PurchaseItems (PurchaseId, ProductId, Quantity, UnitCost, Discount, TaxPercent, LineTotal)
VALUES (@pid, 1, 50, 1.50, 0.00, 5, 78.75);
-- Stock IN for purchase
INSERT INTO dbo.StockMovements (ProductId, Quantity, MovementType, Reference, MovementDate, Note)
VALUES (1, 50, 'IN', 'PINV-20250101080000', @pNow, 'Purchase');

-- Sample sales
DECLARE @now DATETIME = GETDATE();
INSERT INTO dbo.Sales (CustomerId, InvoiceNo, SaleDate, Subtotal, Discount, TaxTotal, GrandTotal, PaidAmount, ChangeAmount, UserId)
VALUES (1, 'INV-20250101090000', DATEADD(DAY,-1,@now), 10.00, 0.50, 0.48, 9.98, 10.00, 0.02, 2),
       (2, 'INV-20250102103000', @now, 15.00, 0.00, 0.75, 15.75, 15.75, 0.00, 2);
INSERT INTO dbo.SalesItems (SaleId, ProductId, Quantity, UnitPrice, Discount, TaxPercent, LineTotal)
VALUES (1, 1, 2, 2.50, 0.50, 5, 4.75),
       (2, 2, 3, 5.00, 0.00, 5, 15.75);
-- Stock OUT for sample sales
INSERT INTO dbo.StockMovements (ProductId, Quantity, MovementType, Reference, MovementDate, Note)
VALUES (1, 2, 'OUT', 'INV-20250101090000', DATEADD(DAY,-1,@now), 'Sample sale'),
       (2, 3, 'OUT', 'INV-20250102103000', @now, 'Sample sale');

-- Sample payments
INSERT INTO dbo.Payments (PaymentDate, PartyType, CustomerId, SupplierId, Amount, Note)
VALUES (GETDATE(), 'Customer', 1, NULL, 20.00, 'Customer payment'),
       (GETDATE(), 'Supplier', NULL, 1, 50.00, 'Supplier payment');

/* Indexes */
CREATE INDEX IX_Products_Barcode ON dbo.Products(Barcode);
CREATE INDEX IX_Products_SKU ON dbo.Products(SKU);