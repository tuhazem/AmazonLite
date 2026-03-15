# Project Documentation

## Overview
- Clean layered architecture: API, Application, Domain, Infrastructure
- Database: SQL Server LocalDB via EF Core
- Authentication: ASP.NET Core Identity with JWT (JSON Web Tokens)
- New features implemented:
  - Authentication: User registration and login
  - Cart: add/update/remove/clear items
  - Checkout: creates Order from Cart and reduces product stock
  - **Business Logic Hardening**:
    - **Transactions**: Atomic checkout flow using `UnitOfWork`.
    - **Optimistic Concurrency**: `RowVersion` on Products to prevent race conditions.
    - **Order Lifecycle**: `OrderStatus` enum (Pending, Processing, etc.).
  - **Enhanced Querying & Search**:
    - Paged, filtered, and sorted product search.
    - Advanced filters: Price range (`minPrice`/`maxPrice`) and availability (`inStock`).
  - **Validation & Data Integrity**:
    - Advanced DTO validation using **FluentValidation**.
    - Centralized validation logic outside of DTO classes.
  - **Logging & Observability**:
    - Structured logging using **Serilog**.
    - Console and daily rolling File sinks.
    - Automatic request and error logging in middleware.
  - Global exception handling returning ProblemDetails JSON

## Authentication API
- POST `api/Account/register`
  - Body: `{ "fullName": "string", "email": "user@example.com", "password": "Password123!" }`
- POST `api/Account/login`
  - Body: `{ "email": "user@example.com", "password": "Password123!" }`
  - Response: `{ "token": "JWT_TOKEN", "email": "user@example.com", "expiration": "ISO_DATE" }`

## Security
- Most endpoints require a valid JWT token in the `Authorization` header: `Bearer YOUR_TOKEN`.
- Public endpoints (AllowAnonymous):
  - GET `api/Product` (List/Search)
  - GET `api/Product/{id}`
  - GET `api/Category` (List)
  - GET `api/Category/{id}`
- Protected endpoints (Authorize):
  - All Cart and Order operations.
  - All POST/PUT/DELETE operations for Products and Categories.

## Cart API
- POST `api/Cart`
  - Creates a new cart
  - Response: `201 Created` + `Location: /api/Cart/{id}`
- GET `api/Cart/{id}`
  - Returns cart with items
- POST `api/Cart/{id}/items`
  - Body: `{ "productId": number, "quantity": number }`
  - Adds item or increases quantity; unit price snaps from Product.Price
- PUT `api/Cart/{id}/items/{productId}`
  - Body: `{ "quantity": number }`
  - Updates quantity for an item
- DELETE `api/Cart/{id}/items/{productId}`
  - Removes item from cart
- DELETE `api/Cart/{id}`
  - Clears the cart

## Checkout / Order API
- POST `api/Order/checkout/{cartId}`
  - Creates an order from the cart:
    - Validates cart exists and has items
    - Loads each product, reduces stock, adds order item with current name and price
    - Persists order and clears cart
  - Response: `OrderDTO` with items and total
- GET `api/Order/{id}`
  - Returns order with items and total
- PUT `api/Order/{id}/status`
  - Body: `number` (Enum value: 0=Pending, 1=Processing, 2=Shipped, 3=Delivered, 4=Cancelled)
  - Updates the order status. Returns `204 No Content`.

## Product API
- GET `api/Product`
  - Query options:
    - `pageNumber` (default 1), `pageSize` (default 20)
    - `search` (filters by name contains, case-insensitive)
    - `categoryId` (optional filter by category)
    - `minPrice` / `maxPrice` (optional price range filters)
    - `inStock` (boolean, if true returns only products with stock > 0)
    - `sortBy` = `name` | `price`, `sortDir` = `asc` | `desc`
  - Filtering/sorting/paging is executed in the repository for performance
  - Returns a `PagedResult<ProductDTO>`: items, totalCount, pageNumber, pageSize
- POST `api/Product`
  - Creates a product
  - Response: `201 Created` + `Location: /api/Product/{id}`
- GET `api/Product/{id}` → returns product or `404`
- PUT `api/Product/{id}` → update price, returns `204`
- DELETE `api/Product/{id}` → returns `204`

## Category API
- GET `api/Category` → list categories
- GET `api/Category/{id}` → returns category or `404`
- POST `api/Category`
  - Creates a category
  - Response: `201 Created` + `Location: /api/Category/{id}`
- PUT `api/Category/{id}` → update, returns `204`
- DELETE `api/Category/{id}`
  - Returns `204`
  - Returns `409 Conflict` if the category has products

## Exception Handling
- Global middleware maps exceptions to ProblemDetails JSON
- Status mapping:
  - `ArgumentException` → 400
  - `InvalidOperationException` → 409
  - `KeyNotFoundException` → 404
  - Message contains “not found” → 404
  - Message contains “cannot be deleted” or “not enough stock” → 409
  - Others → 500
  
### Service Exception Conventions
- Services throw:
  - `KeyNotFoundException` when an entity is missing (e.g., product/category/cart)
  - `InvalidOperationException` for business constraints (e.g., delete category with products, empty cart on checkout)
  - `ArgumentException` for invalid inputs (e.g., non‑positive quantity)

## Database Migrations
- Create tables for Identity:
  - `dotnet ef migrations add AddIdentityTables --project Amazon.Infrastructure --startup-project Amazon.API`
  - `dotnet ef database update --project Amazon.Infrastructure --startup-project Amazon.API`
- Create tables for Cart:
  - `dotnet ef migrations add AddCartTables --project Amazon.Infrastructure --startup-project Amazon.API`
  - `dotnet ef database update --project Amazon.Infrastructure --startup-project Amazon.API`
- Create tables for Order:
  - `dotnet ef migrations add AddOrderTables --project Amazon.Infrastructure --startup-project Amazon.API`
  - `dotnet ef database update --project Amazon.Infrastructure --startup-project Amazon.API`

## Running
- Build: `dotnet build Amazon.sln`
- Run API: `dotnet run --project Amazon.API`
- Swagger UI: `http://localhost:5271/`

## Behavior Notes
- Cart does not reserve stock; stock is reduced at checkout
- Unit prices on cart items snapshot current product price; orders store product name and price at checkout

## Quick Start Examples
- Create Category
  - Request:

    ```http
    POST /api/Category
    Content-Type: application/json

    { "name": "Electronics" }
    ```
  - Response: `201 Created` + `Location: /api/Category/{id}`

- Create Product
  - Request:

    ```http
    POST /api/Product
    Content-Type: application/json

    {
      "name": "Phone X",
      "description": "128GB",
      "price": 999.99,
      "stockQuantity": 25,
      "categoryId": 1
    }
    ```
  - Response: `201 Created` + `Location: /api/Product/{id}`

- Create Cart and Add Item
  - Create cart:

    ```http
    POST /api/Cart
    ```
  - Response: `201 Created` + `Location: /api/Cart/{id}`
  - Add item:

    ```http
    POST /api/Cart/{id}/items
    Content-Type: application/json

    { "productId": 1, "quantity": 2 }
    ```
  - Response: `204 No Content`

- Checkout

  ```http
  POST /api/Order/checkout/{cartId}
  ```

  - Response: `200 OK` with `OrderDTO` (items + total)

## Pagination & Queries
- `GET /api/Product` supports query parameters:
  - `pageNumber`, `pageSize`, `search`, `categoryId`, `sortBy=name|price`, `sortDir=asc|desc`
- Implementation detail:
  - Filtering/sorting/paging is executed in the repository (EF Core) for performance and clean architecture boundaries.
  - Controller maps query → application `ProductListQuery`; service returns `PagedResult<ProductDTO>`.

## Error Responses (ProblemDetails)
- Format:

  ```json
  {
    "title": "An error occurred.",
    "status": 404,
    "detail": "Product not found"
  }
  ```
- Mapped by global middleware from exceptions:
  - `KeyNotFoundException` → 404
  - `InvalidOperationException` → 409
  - `ArgumentException` → 400
  - Others → 500


