\# BizTrack — Feature \& Task Breakdown (MVP)



\*\*Platform:\*\* ASP.NET Core MVC (.NET 8)  

\*\*Architecture:\*\* Layered  

\*\*Database:\*\* MS SQL Server  

\*\*Deployment:\*\* Single Owner / Non-SaaS



---



\## 1. Foundation \& Infrastructure



\### Features

\- Solution \& project setup

\- Layered architecture structure

\- Authentication \& basic security

\- Environment configuration



\### Tasks

\- Create .NET 8 solution with projects:

&nbsp; - BizTrack.Web

&nbsp; - BizTrack.Application

&nbsp; - BizTrack.Domain

&nbsp; - BizTrack.Infrastructure

\- Configure project references between layers

\- Setup EF Core with SQL Server

\- Configure connection strings (Dev/Prod)

\- Setup EF Core migrations

\- Implement basic logging

\- Implement cookie-based authentication

\- Create base layout and navigation

\- Setup global error handling

\- Setup anti-forgery protection



---



\## 2. Product \& Inventory Management



\### Features

\- Manage products

\- Track stock

\- Support warehouses/locations

\- Stock movement history

\- Low stock alerts



\### Tasks

\- Create Product entity in Domain

\- Create Warehouse entity in Domain

\- Create Stock entity in Domain

\- Create EF Core mappings

\- Create ProductsController

\- Create ProductService in Application layer

\- Implement:

&nbsp; - Create product

&nbsp; - Edit product

&nbsp; - Delete product

&nbsp; - List products

\- Implement:

&nbsp; - Create warehouse

&nbsp; - Edit warehouse

&nbsp; - List warehouses

\- Implement stock tracking logic

\- Implement stock movement records (in/out)

\- Implement low stock threshold \& alert logic

\- Create UI views for all above



---



\## 3. Supplier Management



\### Features

\- Manage suppliers

\- Track supplier payable (due)

\- View supplier history



\### Tasks

\- Create Supplier entity

\- Create SupplierService

\- Create SuppliersController

\- Implement:

&nbsp; - Create supplier

&nbsp; - Edit supplier

&nbsp; - Delete supplier

&nbsp; - List suppliers

\- Implement supplier details page:

&nbsp; - Total purchases

&nbsp; - Total paid

&nbsp; - Current payable

\- Implement supplier payment history view

\- Create Razor views for supplier screens



---



\## 4. Purchase Management (Buying)



\### Features

\- Record purchases

\- Auto-increase stock

\- Track supplier due

\- Edit \& cancel purchases



\### Tasks

\- Create Purchase and PurchaseItem entities

\- Create PurchaseService

\- Create PurchasesController

\- Implement:

&nbsp; - Create purchase invoice

&nbsp; - Add multiple products to purchase

&nbsp; - Enter unit cost \& quantity

&nbsp; - Enter paid amount \& calculate due

\- Implement transaction:

&nbsp; - Increase stock

&nbsp; - Create purchase record

&nbsp; - Update supplier payable

\- Implement:

&nbsp; - List purchases

&nbsp; - View purchase details

&nbsp; - Edit purchase (with stock re-adjustment)

&nbsp; - Cancel/delete purchase (with stock rollback)

\- Create Razor views for purchase flows



---



\## 5. Customer Management



\### Features

\- Manage customers

\- Track customer receivable (due)

\- View customer history



\### Tasks

\- Create Customer entity

\- Create CustomerService

\- Create CustomersController

\- Implement:

&nbsp; - Create customer

&nbsp; - Edit customer

&nbsp; - Delete customer

&nbsp; - List customers

\- Implement customer details page:

&nbsp; - Total sales

&nbsp; - Total paid

&nbsp; - Current due

\- Implement customer payment history view

\- Create Razor views for customer screens



---



\## 6. Sales Management (Selling)



\### Features

\- Create sales invoices

\- Flexible pricing per sale

\- Auto-decrease stock

\- Track customer due

\- Edit \& cancel sales

\- Print/PDF invoice



\### Tasks

\- Create Sale and SaleItem entities

\- Create SalesService

\- Create SalesController

\- Implement:

&nbsp; - Create sales invoice

&nbsp; - Add multiple products

&nbsp; - Set selling price per item

&nbsp; - Enter paid amount \& calculate due

\- Implement transaction:

&nbsp; - Decrease stock

&nbsp; - Create sale record

&nbsp; - Update customer due

\- Implement:

&nbsp; - List sales

&nbsp; - View sale details

&nbsp; - Edit sale (with stock re-adjustment)

&nbsp; - Cancel/delete sale (with stock rollback)

\- Implement invoice print/PDF generation

\- Create Razor views for sales flows



---



\## 7. Payment \& Due Management



\### Features

\- Record payments from customers

\- Record payments to suppliers

\- Track dues and payables

\- Due summary views



\### Tasks

\- Create Payment entity

\- Create PaymentService

\- Implement:

&nbsp; - Record customer payment

&nbsp; - Record supplier payment

\- Implement:

&nbsp; - Apply payment to invoice or overall balance

&nbsp; - Update customer due

&nbsp; - Update supplier payable

\- Create:

&nbsp; - Customer due list view

&nbsp; - Supplier payable list view

\- Create summary widgets:

&nbsp; - Total customer due

&nbsp; - Total supplier payable

\- Create Razor views for payment flows



---



\## 8. Profit \& Financial Calculation



\### Features

\- Calculate real profit

\- Profit by invoice

\- Profit by product

\- Daily \& monthly profit summary



\### Tasks

\- Implement cost calculation logic (based on purchase cost)

\- Implement profit calculation per sale

\- Implement:

&nbsp; - Profit per invoice report

&nbsp; - Profit per product report

\- Implement:

&nbsp; - Daily profit summary

&nbsp; - Monthly profit summary

\- Create ReportService

\- Create Razor views for profit reports



---



\## 9. Dashboard \& Reports



\### Features

\- Business overview dashboard

\- Sales, purchase, stock, due, profit reports



\### Tasks

\- Create DashboardController

\- Implement dashboard widgets:

&nbsp; - Today’s sales

&nbsp; - This month’s sales

&nbsp; - This month’s profit

&nbsp; - Total customer due

&nbsp; - Total supplier payable

&nbsp; - Low stock alerts

\- Implement reports:

&nbsp; - Sales report (date range)

&nbsp; - Purchase report (date range)

&nbsp; - Stock report

&nbsp; - Due report

&nbsp; - Profit report

\- Optimize queries using projections

\- Create Razor views for dashboard \& reports



---



\## 10. User \& Settings Management



\### Features

\- Business profile settings

\- Change password

\- Basic system settings

\- Backup \& restore (manual)



\### Tasks

\- Create SettingsController

\- Implement:

&nbsp; - Update business profile (name, address, phone)

&nbsp; - Change password

&nbsp; - Update basic settings (currency, date format)

\- Implement:

&nbsp; - Manual database backup

&nbsp; - Restore from backup (admin only)

\- Create Razor views for settings pages



---



\## 11. Quality, Security \& Performance



\### Features

\- Data validation

\- Business rule enforcement

\- Performance optimization

\- Production readiness



\### Tasks

\- Add input validation to all forms

\- Enforce domain rules:

&nbsp; - No negative stock

&nbsp; - No overpayment

&nbsp; - No selling more than stock

\- Add pagination to:

&nbsp; - Sales list

&nbsp; - Purchase list

&nbsp; - Payment list

\- Add indexes to frequently queried columns

\- Add basic audit logs (who did what)

\- Secure all controllers with authorization

\- Prepare production build

\- Deploy to server

\- Smoke test production environment



---



\## 12. MVP Completion Criteria



\- Products, purchases, sales fully working

\- Stock auto-updates correctly

\- Customer \& supplier dues tracked correctly

\- Profit \& dashboard visible

\- System stable for real shop usage



---



End of Document

