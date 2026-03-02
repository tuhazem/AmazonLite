# Project Documentation

## Overview
- Clean layered architecture: API, Application, Domain, Infrastructure
- Database: SQL Server LocalDB via EF Core
- New features implemented:
  - Cart: add/update/remove/clear items
  - Checkout: creates Order from Cart and reduces product stock
  - Global exception handling returning ProblemDetails JSON

## Layers and Key Files
- API
  - Controllers: [CartController.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.API/Controllers/CartController.cs), [OrderController.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.API/Controllers/OrderController.cs)
  - Middleware: [ExceptionHandlingMiddleware.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.API/Middleware/ExceptionHandlingMiddleware.cs)
  - Registration: [Program.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.API/Program.cs)
- Application
  - Services: [CartService.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/Services/CartService.cs), [OrderService.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/Services/OrderService.cs)
  - Interfaces: [ICartService.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/Interfaces/ICartService.cs), [IOrderService.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/Interfaces/IOrderService.cs)
  - DTOs: [CartDTO.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/DTOs/CartDTO.cs), [OrderDTO.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/DTOs/OrderDTO.cs)
  - Mappings: [CartProfile.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/Mappings/CartProfile.cs), [OrderProfile.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Application/Mappings/OrderProfile.cs)
- Domain
  - Entities: [Cart.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Domain/Entities/Cart.cs), [CartItem.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Domain/Entities/CartItem.cs), [Order.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Domain/Entities/Order.cs), [OrderItem.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Domain/Entities/OrderItem.cs)
  - Repositories: [ICartRepository.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Domain/Interfaces/ICartRepository.cs), [IOrderRepository.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Domain/Interfaces/IOrderRepository.cs)
- Infrastructure
  - DbContext: [AmazonDbContext.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Infrastructure/Persistence/AmazonDbContext.cs)
  - Repositories: [CartRepository.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Infrastructure/Repositories/CartRepository.cs), [OrderRepository.cs](file:///d:/Dot%20Net%20iti/Back/Amazon.Infrastructure/Repositories/OrderRepository.cs)

## Cart API
- POST `api/Cart`
  - Creates a new cart
  - Response: `{ "id": number }`
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

## Exception Handling
- Global middleware maps exceptions to ProblemDetails JSON
- Status mapping:
  - `ArgumentException` → 400
  - `InvalidOperationException` → 409
  - `KeyNotFoundException` → 404
  - Message contains “not found” → 404
  - Message contains “cannot be deleted” or “not enough stock” → 409
  - Others → 500

## Database Migrations
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

