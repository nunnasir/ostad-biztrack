# Smart Inventory – Detailed Architecture Guide

This document describes the architecture of the **Smart Inventory** ASP.NET Core solution so you can reuse the same structure and patterns in your next project.

---

## 1. Overview

- **Stack:** ASP.NET Core 8 (MVC), Entity Framework Core 8, SQL Server, ASP.NET Core Identity  
- **Style:** Layered architecture with clear separation: **Web** → **BLL** → **DAL**; **Contract** and **Model** are shared.  
- **Patterns:** Repository, Unit of Work, Service layer, Result pattern, Request/Response DTOs, dependency injection.

---

## 2. Solution Structure

```
SmartInventory.sln
├── SmartInventory.Web      ← MVC UI (Controllers, Views, wwwroot)
├── SmartInventory.Api      ← Optional REST API (separate host)
├── SmartInventory.BLL      ← Business logic (services, helpers, mapping)
├── SmartInventory.DAL      ← Data access (DbContext, repositories, unit of work)
├── SmartInventory.Contract ← DTOs: Request/Response, no business or DB logic
└── SmartInventory.Model    ← Domain entities and interfaces
```

### Project reference rules

| Project        | References (only)        | Purpose                          |
|----------------|--------------------------|----------------------------------|
| **SmartInventory.Web** | BLL, Contract            | UI; no direct DAL/Model in controllers for domain logic |
| **SmartInventory.Api**  | BLL, Contract, DAL, Model | API endpoints; can mirror Web or be standalone |
| **SmartInventory.BLL**  | Contract, DAL, Model     | Services, validation, mapping    |
| **SmartInventory.DAL**  | Model                    | EF Core, repositories, DbContext |
| **SmartInventory.Contract** | (none)               | Request/Response DTOs            |
| **SmartInventory.Model**   | (none)               | Entities, base classes           |

- **Web** does **not** reference DAL or Model directly; it talks to the app through **BLL** and **Contract**.
- **Contract** and **Model** have no project references (stay free of infrastructure).

---

## 3. Layer Responsibilities

### 3.1 SmartInventory.Model

- **Domain entities** used by EF Core and BLL (e.g. `Product`, `ApplicationUser`, `Permission`, `RolePermission`).
- **Base types** for entities, e.g.:
  - `IEntity<TKey>` (optional): Id, audit fields.
  - `Entity` (int key): Id, CreatedBy, CreatedTime, UpdatedBy, UpdatedAt.
- **Identity:** Custom user type (e.g. `ApplicationUser : IdentityUser`) with extra properties (FirstName, LastName, IsActive, CreatedAt).
- **No** references to other projects. No business logic, no DTOs.

### 3.2 SmartInventory.Contract

- **Request DTOs:** Input for operations (e.g. `CreateProductRequest`, `UpdateProductRequest`, `LoginModel`, `RegisterModel`, `DataTablesRequest`).
- **Response DTOs:** Output shapes (e.g. `DataTablesResponse<T>`).
- **Data annotations** for validation (e.g. `[Required]`, `[EmailAddress]`) are allowed here.
- **No** references to BLL, DAL, or Model. No logic, only data shapes.

Typical layout:

- `Request/` – Create/Update/Command DTOs, account models, `DataTablesRequest`, etc.
- `Response/` – List/detail responses, `DataTablesResponse<T>`, etc.

### 3.3 SmartInventory.DAL (Data Access Layer)

- **DbContext:** Single EF Core context (e.g. `SmartInventoryDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>`).
  - DbSets for all entities.
  - `OnModelCreating` for relationships, indexes, and Identity config.
- **Core abstractions:**
  - **IRepository<TEntity, TKey, TContext>** – generic CRUD + query (GetAsync with selector/predicate/orderBy/include, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync, pagination overload).
  - **Repository<TEntity, TKey, TContext>** – base implementation using `DbSet` and `IQueryable`.
  - **IUnitOfWork** – `SaveChanges`, `SaveChangesAsync`, `RollBack`, `Dispose`.
  - **UnitOfWork** – wraps the same `DbContext` used by repositories.
- **Concrete repositories:** One interface + implementation per aggregate or entity set (e.g. `IProductRepository` : `IRepository<Product, int, SmartInventoryDbContext>` with extra methods like `CountProduct()`; `ProductRepository` : `Repository<...>`, `IProductRepository`).
- **Unit of Work per feature/aggregate:** e.g. `IProductUnitOfWork : IUnitOfWork` with `IProductRepository ProductRepository { get; }`; `ProductUnitOfWork` receives `DbContext` and `IProductRepository` and implements save/rollback.
- **Migrations** live in the DAL project (e.g. `Migrations/`).

DAL references **only Model**. No Contract, no BLL.

### 3.4 SmartInventory.BLL (Business Logic Layer)

- **Service interfaces:** e.g. `IProductService` – methods return `Task<Result<T>>` or `Task<DataTablesResponse<T>>`.
- **Service implementations:** Inject **unit of work** (e.g. `IProductUnitOfWork`), not repositories directly when one UoW groups several repos. Use repositories only via UoW.
- **Result pattern:** `Result<T>` with `Success`, `Error`, `Data`; static helpers `Result<T>.SuccessResult(data)` and `Result<T>.FaileResult(error)` (keep typo for compatibility or rename to `FailResult` in new projects).
- **Mapping:** Extension methods in a dedicated class (e.g. `ContractMapping`): Request → Entity (e.g. `CreateProductRequest` → `Product`), Entity → Request (e.g. `Product` → `UpdateProductRequest`). No AutoMapper required.
- **Helpers:** Reusable logic (e.g. `DataTablesHelper` for pagination and search predicate, `EmailValidator`). No UI or HTTP references.

BLL references **Contract**, **DAL**, and **Model**.

### 3.5 SmartInventory.Web (Presentation – MVC)

- **Program.cs / Startup:**
  - Register DbContext (SQL Server).
  - Register Identity (IdentityUser → ApplicationUser, IdentityRole, cookie options).
  - Call `AddRepositories()` and `AddServices()` (from a `DependencyInjection` class).
  - Optionally run a **DbInitializer** (seed roles and default admin) after build.
  - Pipeline: HTTPS redirection, static files, routing, authentication, authorization, then MVC default route.
- **Controllers:**
  - Inject **services** (e.g. `IProductService`), not repositories or DbContext.
  - Use **Contract** types for action parameters and view models (e.g. `CreateProductRequest`, `UpdateProductRequest`, `DataTablesRequest`).
  - Return `Result<T>` from services; check `result.Success` and set TempData for success/error messages.
  - Use constants for message strings (e.g. `ProductMessages.*`, `TempDataKeys.*`).
  - Apply `[Authorize]` / `[Authorize(Roles = "Admin")]` where needed; `[AllowAnonymous]` for login/register.
  - Use `[ValidateAntiForgeryToken]` on POST actions that change data; `[IgnoreAntiforgeryToken]` only for specific endpoints (e.g. DataTables POST from client).
- **Views:** Standard MVC (Razor), shared layout, partials for validation scripts. Use ViewData/TempData for titles and messages.
- **Constants:** Centralize TempData keys and per-feature messages (e.g. under `Constants/`).
- **Data:** `DbInitializer` in a `Data/` folder, called once at startup.

Web references **BLL** and **Contract** only (no DAL/Model in controller logic).

### 3.6 SmartInventory.Api (Optional)

- Separate web project for REST API (JWT, Swagger, etc.).
- Can reference BLL, Contract, DAL, Model and reuse the same services and DTOs.
- Use same Result/Contract patterns for consistency.

---

## 4. Key Patterns in Code

### 4.1 Repository

- **Interface:** Extends generic `IRepository<TEntity, TKey, TContext>` and adds entity-specific methods.
- **Implementation:** Extends `Repository<TEntity, TKey, TContext>`, implements the entity interface, uses the same `TContext` (e.g. `SmartInventoryDbContext`).
- All data access goes through repositories; controllers never use `DbContext` directly.

### 4.2 Unit of Work

- One UoW type per “aggregate” or feature (e.g. `IProductUnitOfWork` with `ProductRepository`).
- UoW holds the same `DbContext` as the repositories and exposes repositories as properties.
- Services call `UoW.Repo.AddAsync(...)` then `UoW.SaveChangesAsync()` so one scope = one transaction.

### 4.3 Service Layer

- One interface + implementation per domain area (e.g. `IProductService`, `ProductService`).
- Methods: `GetAllAsync`, `GetByIdAsync`, `AddAsync(CreateXxxRequest)`, `UpdateAsync(UpdateXxxRequest)`, `DeleteAsync(id)`, and optionally `GetDataTablesAsync(DataTablesRequest)`.
- Return `Result<T>` for commands and single-item queries; return `DataTablesResponse<T>` for DataTables.
- Services perform validation, mapping (Request → Entity), and call UoW/repositories; they do not reference HTTP or UI.

### 4.4 Result Pattern

- `Result<T>`: `Success`, `Error`, `Data`.
- Use `Result<T>.SuccessResult(data)` and `Result<T>.FaileResult(message)` (or `FailResult`).
- Controllers check `result.Success` and either redirect with TempData or return the view with ModelState errors.

### 4.5 Request/Response (Contract)

- **Create:** e.g. `CreateProductRequest` (Name, Description, Price, StockQuantity) – no Id.
- **Update:** e.g. `UpdateProductRequest` (Id, Name, Description, Price, StockQuantity).
- **DataTables:** `DataTablesRequest` (Draw, Start, Length, Search, Order, Columns); `DataTablesResponse<T>` (Draw, RecordsTotal, RecordsFiltered, Data, Error).
- Mapping lives in BLL (e.g. `CreateProductRequest` → `Product`; `Product` → `UpdateProductRequest`).

### 4.6 Dependency Injection

- **Registration:** In Web (or Api), a static class `DependencyInjection` with:
  - `AddRepositories(IServiceCollection)`: register each `IRepository`/`IUnitOfWork` implementation as **Scoped**.
  - `AddServices(IServiceCollection)`: register each service interface as **Scoped**.
- DbContext and Identity are also Scoped. One scope per HTTP request gives one DbContext and one UoW per request.

---

## 5. Data Flow (Example: Create Product)

1. **Browser** → POST form with `CreateProductRequest` (Name, Description, Price, StockQuantity).
2. **ProductController.Create(CreateProductRequest):** ModelState valid → call `_productService.AddAsync(request)`.
3. **ProductService.AddAsync:**  
   - Validate (e.g. duplicate name).  
   - Map `request` → `Product` (e.g. `request.MapToProduct()`).  
   - `_productUnitOfWork.ProductRepository.AddAsync(product)` → `_productUnitOfWork.SaveChangesAsync()`.  
   - Return `Result<int>.SuccessResult(product.Id)` or `Result<int>.FaileResult("...")`.
4. **Controller:** If success, set TempData success message and redirect to Details(id); else add error to ModelState and return View(request).
5. **DAL:** Repository adds to DbSet; UoW saves the shared DbContext.

---

## 6. Naming and Organization Conventions

- **Entities:** PascalCase, singular (e.g. `Product`, `ApplicationUser`).
- **Requests:** `CreateXxxRequest`, `UpdateXxxRequest`; account: `LoginModel`, `RegisterModel`.
- **Responses:** `DataTablesResponse<T>`, or specific DTOs like `XxxDetailResponse`.
- **Repositories:** `IProductRepository`, `ProductRepository` in `DAL/Interfaces` and `DAL/Implementation`.
- **Unit of Work:** `IProductUnitOfWork`, `ProductUnitOfWork` in same namespaces.
- **Services:** `IProductService`, `ProductService` in `BLL/Interfaces` and `BLL/Implementations`.
- **Mapping:** Static class e.g. `ContractMapping` in `BLL/Mapping` with extension methods.
- **Constants:** `TempDataKeys`, `ProductMessages` (or per feature) in Web `Constants/`.
- **DbContext:** Single context, e.g. `SmartInventoryDbContext`; DbSet names plural (e.g. `Products`).

---

## 7. Authentication and Authorization

- **Identity:** ASP.NET Core Identity with `ApplicationUser` and `IdentityRole`, stored in the same DbContext.
- **Cookie:** Login path `/Account/Login`, logout `/Account/Logout`, access denied `/Account/AccessDenied`; sliding expiration (e.g. 30 minutes).
- **Roles:** Seeded via `DbInitializer` (e.g. "Admin", "User"); default admin user (e.g. admin@smartinventory.com) in "Admin" role.
- **Controllers:** Use `[Authorize]` for authenticated-only; `[Authorize(Roles = "Admin")]` for admin-only; `[AllowAnonymous]` for login/register.

---

## 8. Checklist for Your Next Project

Use this as a short checklist when starting a new solution:

- [ ] Create solution with projects: **Web**, **Api** (optional), **BLL**, **DAL**, **Contract**, **Model**.
- [ ] Set references: Web → BLL, Contract; Api → BLL, Contract, DAL, Model; BLL → Contract, DAL, Model; DAL → Model; Contract and Model → none.
- [ ] **Model:** Add entities and base types (`Entity`, optional `IEntity`); add custom `ApplicationUser` if using Identity.
- [ ] **Contract:** Add Request/Response folders and DTOs (Create/Update, DataTables if needed).
- [ ] **DAL:** Add DbContext; generic `IRepository`/`Repository` and `IUnitOfWork`/`UnitOfWork`; per-entity repository and UoW (e.g. `IProductRepository`, `ProductRepository`, `IProductUnitOfWork`, `ProductUnitOfWork`).
- [ ] **BLL:** Add `Result<T>`; service interfaces and implementations using UoW; mapping extensions (Request ↔ Entity); helpers (e.g. DataTables, validation).
- [ ] **Web:** In `Program.cs` register DbContext, Identity, then `AddRepositories()` and `AddServices()`; add `DependencyInjection` class; run DbInitializer if needed; configure pipeline and default route.
- [ ] **Web:** Controllers inject services only; use Contract types and Result; use constants for messages and TempData keys; apply Authorize/AllowAnonymous and anti-forgery where appropriate.
- [ ] **Migrations:** Add and apply migrations from the DAL project.

---

## 9. Summary Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│  SmartInventory.Web (MVC)                                        │
│  Controllers → IProductService, Contract DTOs, TempData, Views   │
└───────────────────────────────┬─────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│  SmartInventory.BLL                                              │
│  IProductService / ProductService, Result<T>, ContractMapping,   │
│  DataTablesHelper, validation                                    │
└───────────────────────────────┬─────────────────────────────────┘
                                │
                ┌───────────────┴───────────────┐
                ▼                               ▼
┌───────────────────────────┐     ┌───────────────────────────────┐
│  SmartInventory.Contract  │     │  SmartInventory.DAL            │
│  Request/Response DTOs    │     │  IProductUnitOfWork,           │
│  (no dependencies)        │     │  IProductRepository, DbContext  │
└───────────────────────────┘     └───────────────┬────────────────┘
                                                  │
                                                  ▼
                                    ┌───────────────────────────────┐
                                    │  SmartInventory.Model         │
                                    │  Product, ApplicationUser, etc.│
                                    └───────────────────────────────┘
```

This architecture keeps the Web layer thin, business rules in BLL, and data access in DAL, so you can reuse the same layout and patterns in your next project.
