\# BizTrack — JIRA Epics \& Stories (MVP)



---



\## 🧱 EPIC 1: Project Setup \& Core Infrastructure

\*\*Goal:\*\* Establish the technical foundation of the product.



\### Stories:

\- Set up backend project structure

\- Set up frontend project

\- Configure authentication (login, logout)

\- Create user role: Owner (Admin)

\- Set up database schema \& migrations

\- Implement basic error handling \& logging

\- Set up environment configs (Dev/Prod)

\- Create base layout \& navigation UI



---



\## 📦 EPIC 2: Product \& Inventory Management

\*\*Goal:\*\* Allow user to manage products and track stock.



\### Stories:

\- Create product (name, SKU, category, unit)

\- Edit product

\- Delete product

\- View product list

\- Add warehouse/location

\- Assign product stock to location

\- View stock by product

\- View stock by location

\- Show low stock alert

\- View stock movement history (in/out)



---



\## 🧾 EPIC 3: Supplier Management

\*\*Goal:\*\* Manage suppliers and track payables.



\### Stories:

\- Create supplier

\- Edit supplier

\- Delete supplier

\- View supplier list

\- View supplier details (total purchase, paid, due)

\- View supplier payment history



---



\## 🛒 EPIC 4: Purchase Management (Buying)

\*\*Goal:\*\* Record purchases and auto-update stock \& dues.



\### Stories:

\- Create purchase invoice

\- Add multiple products to purchase

\- Enter unit cost \& quantity

\- Enter paid amount \& auto-calculate due

\- Auto-increase stock on purchase save

\- Auto-update supplier due

\- View purchase list

\- View purchase details

\- Edit purchase (with stock re-adjustment)

\- Delete/cancel purchase (with stock rollback)



---



\## 👥 EPIC 5: Customer Management

\*\*Goal:\*\* Manage customers and track receivables.



\### Stories:

\- Create customer

\- Edit customer

\- Delete customer

\- View customer list

\- View customer details (total sales, paid, due)

\- View customer payment history



---



\## 🧮 EPIC 6: Sales Management (Selling)

\*\*Goal:\*\* Create sales invoices and track revenue, stock, and dues.



\### Stories:

\- Create sales invoice

\- Add multiple products to sale

\- Set flexible selling price per product

\- Enter paid amount \& auto-calculate due

\- Auto-decrease stock on sale save

\- Auto-update customer due

\- View sales list

\- View sales details

\- Edit sales invoice (with stock re-adjustment)

\- Delete/cancel sales invoice (with stock rollback)

\- Generate printable/PDF invoice



---



\## 💳 EPIC 7: Payment \& Due Management

\*\*Goal:\*\* Track who owes money and record payments.



\### Stories:

\- View total customer due summary

\- View total supplier payable summary

\- Record payment from customer

\- Record payment to supplier

\- Apply payment to specific invoice or overall balance

\- Update customer due after payment

\- Update supplier payable after payment

\- View due list (customers)

\- View payable list (suppliers)



---



\## 📈 EPIC 8: Profit \& Financial Calculation

\*\*Goal:\*\* Show real profit based on actual cost and sales.



\### Stories:

\- Calculate profit per sales invoice

\- Calculate profit per product

\- Generate daily profit summary

\- Generate monthly profit summary

\- Show revenue vs cost vs profit

\- Validate profit calculation against purchase cost



---



\## 📊 EPIC 9: Dashboard \& Reports

\*\*Goal:\*\* Give owner a clear business overview.



\### Stories:

\- Dashboard: today’s sales

\- Dashboard: this month’s sales

\- Dashboard: this month’s profit

\- Dashboard: total customer due

\- Dashboard: total supplier payable

\- Dashboard: low stock alerts

\- Sales report (date range)

\- Purchase report (date range)

\- Profit report

\- Stock report

\- Due report (customers \& suppliers)



---



\## 🔐 EPIC 10: User \& Settings Management (MVP Level)

\*\*Goal:\*\* Basic system control \& configuration.



\### Stories:

\- Update business profile (shop name, address, phone)

\- Change password

\- Basic app settings (currency, date format)

\- Manual database backup (admin)

\- Restore from backup (admin)



---



\## 🧪 EPIC 11: Quality, Security \& Deployment

\*\*Goal:\*\* Make the product stable and production-ready.



\### Stories:

\- Input validation on all forms

\- Prevent negative stock

\- Prevent invalid payments

\- Add basic audit logs (who did what)

\- Performance optimization for large lists

\- Secure API endpoints with authentication

\- Prepare production build

\- Deploy to server

\- Smoke test production



---



\## 🏁 MVP Release Criteria



\- User can manage products, purchases, and sales

\- Stock updates automatically

\- Customer \& supplier dues are tracked

\- Profit and dashboard are visible

\- System is stable for real shop usage



---



\## ⭐ Strategic Focus



> EPIC 7 (Due Management) and EPIC 8 (Profit Calculation) are the core differentiators.  

Without them, this becomes just another inventory CRUD app.



---



End of Document

