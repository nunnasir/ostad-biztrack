

---



\## 4. Layer Responsibilities



\### 4.1 Presentation Layer (BizTrack.Web)

\*\*Responsibilities:\*\*

\- MVC Controllers

\- Razor Views

\- ViewModels / DTOs

\- Input validation

\- Authentication \& authorization

\- Calls Application Layer services



\*\*Contains:\*\*

\- Controllers (ProductsController, SalesController, PurchasesController, etc.)

\- Views

\- ViewModels

\- Filters



---



\### 4.2 Application Layer (BizTrack.Application)

\*\*Responsibilities:\*\*

\- Implements business use cases

\- Coordinates workflows

\- Manages transactions

\- Calls Domain and Infrastructure

\- No dependency on UI



\*\*Contains:\*\*

\- Services:

&nbsp; - ProductService

&nbsp; - SalesService

&nbsp; - PurchaseService

&nbsp; - PaymentService

&nbsp; - ReportService

\- Interfaces

\- DTOs

\- Use cases:

&nbsp; - Create Sale

&nbsp; - Record Purchase

&nbsp; - Record Payment

&nbsp; - Update Stock

&nbsp; - Calculate Profit

&nbsp; - Generate Reports



---



\### 4.3 Domain Layer (BizTrack.Domain)

\*\*Responsibilities:\*\*

\- Core business rules

\- Business entities

\- Invariants and validations



\*\*Contains:\*\*

\- Entities:

&nbsp; - Product

&nbsp; - Customer

&nbsp; - Supplier

&nbsp; - Purchase

&nbsp; - Sale

&nbsp; - Payment

&nbsp; - Stock

&nbsp; - Warehouse

\- Enums:

&nbsp; - PaymentStatus

&nbsp; - InvoiceStatus

\- Business Rules:

&nbsp; - Stock cannot go negative

&nbsp; - Payment cannot exceed due

&nbsp; - Sale cannot exceed available stock



\*\*Rule:\*\*

> Domain layer has NO dependency on MVC, EF Core, or SQL Server.



---



\### 4.4 Infrastructure Layer (BizTrack.Infrastructure)

\*\*Responsibilities:\*\*

\- Data access implementation

\- EF Core DbContext

\- Repository implementations

\- Migrations

\- SQL Server integration



\*\*Contains:\*\*

\- BizTrackDbContext

\- Entity configurations (Fluent API)

\- Repositories

\- EF Core migrations



---



\## 5. Data Access Strategy



\- ORM: Entity Framework Core 8

\- Pattern:

&nbsp; - DbContext as Unit of Work

&nbsp; - Optional Repository pattern

\- Migrations:

&nbsp; - Code-first

\- Indexes on:

&nbsp; - ProductId

&nbsp; - CustomerId

&nbsp; - SupplierId

&nbsp; - InvoiceDate

&nbsp; - PaymentDate



---



\## 6. Core Modules Mapping



| Module | Layers Involved |

|--------|-----------------|

| Products \& Inventory | Web + Application + Domain + Infrastructure |

| Purchases | Web + Application + Domain + Infrastructure |

| Sales | Web + Application + Domain + Infrastructure |

| Customers | Web + Application + Domain + Infrastructure |

| Suppliers | Web + Application + Domain + Infrastructure |

| Payments \& Dues | Web + Application + Domain + Infrastructure |

| Profit \& Reports | Web + Application + Infrastructure |

| Dashboard | Web + Application |

| Users \& Auth | Web + Infrastructure |



---



\## 7. Transaction \& Consistency Rules



\- \*\*Create Sale:\*\*

&nbsp; - Decrease stock

&nbsp; - Create sale record

&nbsp; - Update customer due

&nbsp; - Save in a single DB transaction



\- \*\*Create Purchase:\*\*

&nbsp; - Increase stock

&nbsp; - Create purchase record

&nbsp; - Update supplier payable

&nbsp; - Save in a single DB transaction



\- \*\*Record Payment:\*\*

&nbsp; - Update due/payable

&nbsp; - Create payment record

&nbsp; - Save in a single DB transaction



---



\## 8. Security Model



\- Authentication:

&nbsp; - Cookie-based login

\- Authorization:

&nbsp; - Role-based (Owner, later Staff)

\- Protection:

&nbsp; - Anti-forgery tokens

&nbsp; - Input validation

&nbsp; - Prevent over-posting

\- Database:

&nbsp; - EF Core parameterized queries

&nbsp; - No raw SQL for user input



---



\## 9. Performance Considerations



\- Pagination for:

&nbsp; - Sales

&nbsp; - Purchases

&nbsp; - Payments

\- Proper indexing

\- Use projections for reports

\- Avoid loading large object graphs



---



\## 10. Deployment Model



\- Single instance deployment

\- One database per business

\- Apply EF migrations on deploy

\- Config via:

&nbsp; - appsettings.Development.json

&nbsp; - appsettings.Production.json



---



\## 11. MVP Technical Scope



Included:

\- MVC UI

\- Layered architecture

\- EF Core + SQL Server

\- Products, Sales, Purchases

\- Customers, Suppliers

\- Payments \& Dues

\- Profit \& Dashboard

\- Basic authentication



Excluded:

\- Multi-tenant SaaS features

\- Subscription \& billing

\- SMS/WhatsApp integration

\- Mobile app

\- Advanced analytics



---



\## 12. Proposed Solution Structure



