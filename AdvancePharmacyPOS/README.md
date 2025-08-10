# Advance Pharmacy (POS) Management System

WinForms application targeting .NET Framework 4.8. Offline-capable with SQL Server Express.

## Features (implemented in this starter)
- Login (Admin/Cashier)
- Products management (CRUD)
- Billing / POS (barcode/SKU entry, totals, save invoice, print preview)
- Sales report (date range, export CSV, print)
- Backup/Restore utility (SQL BACKUP/RESTORE)

Additional modules are scaffolded for future expansion.

## Tech stack
- .NET Framework 4.8 (WinForms)
- SQL Server Express
- System.Data.SqlClient + Dapper
- PdfSharp (planned for invoice PDF)
- BarcodeLib (planned for barcode rendering)

## Prerequisites (Windows 10/11)
1. Install SQL Server Express and SQL Server Management Studio (SSMS)
2. Install Visual Studio 2019/2022 with .NET desktop development workload
3. Ensure .NET Framework 4.8 Developer Pack is installed

## Database setup
- Connection string (App.config): `Data Source=.\SQLEXPRESS;Initial Catalog=POSPharmacy;Integrated Security=True;TrustServerCertificate=True`

### Create DB via script
1. Open SSMS and connect to `localhost\SQLEXPRESS`
2. New Query -> open `Database/create_and_seed_full.sql` (preferred) or `Database/create_and_seed.sql`
3. Execute. This creates database `POSPharmacy` and seeds sample data with 5 products, 2 suppliers, 3 customers, 2 users, and example sales/purchases.

### Backup/Restore
- Use the in-app Tools -> Backup / Restore to create or restore a `.bak`, or in SSMS:
  - Back up: right-click `POSPharmacy` -> Tasks -> Back Up...
  - Restore: right-click `Databases` -> Restore Database...

## Build and run
1. Open `AdvancePharmacyPOS.sln` in Visual Studio
2. Restore NuGet packages (VS will prompt, or right-click solution -> Restore NuGet Packages)
3. Set build config to `Release` (or `Debug`) and Build
4. Run. Login with:
   - Admin: `admin` / `admin123`
   - Cashier: `cashier` / `cashier123`

## Create installer (MSI)
1. Install extension: Visual Studio Installer Projects
2. Add new Setup Project to the solution
3. Add Project Output -> Primary output from `AdvancePharmacyPOS`
4. Configure prerequisites (.NET Framework 4.8)
5. Build the Setup Project to produce `.msi` or `setup.exe`

## NuGet packages
- Dapper 2.1.35
- PdfSharp 1.50.5147
- BarcodeLib 2.2.5

## Notes
- Binaries (.exe/.msi) and a true SQL Server `.bak` cannot be included from this environment. Follow steps above to build and create a backup on your machine.
- A prepacked zip of the project can be created from this folder if needed. If you are viewing this in a cloud workspace, download `AdvancePharmacyPOS.zip` once generated.

## Credits and License
- Free/open-source libraries as listed above. No paid controls.