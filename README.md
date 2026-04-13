#  BookStore API

A RESTful ASP.NET Core Web API for managing a book store. It supports book and category management, a shopping cart, and JWT-based authentication with role-based authorization.

---

##  Architecture Overview

The project follows a clean layered architecture:

- **Controllers** — Handle HTTP requests and delegate to services
- **Services** — Contain business logic, implementing service interfaces
- **Repositories** — Data access via a generic repository pattern and Unit of Work
- **Entities** — Core domain models (`Book`, `Category`, `Cart`, `CartItem`)
- **DbContexts** — Two separate EF Core contexts: one for app data, one for identity/auth

---

##  Getting Started

### Prerequisites

- [.NET 7+](https://dotnet.microsoft.com/download)
- SQL Server (or localdb)
- Visual Studio 2022 / VS Code

### Configuration

Add the following to your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=BookStoreDb;...",
    "AuthConnection": "Server=...;Database=BookStoreAuthDb;..."
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "https://yourdomain.com",
    "Audience": "https://yourdomain.com"
  }
}
```

### Database Migrations

```bash
# Apply application database migrations
dotnet ef database update --context ApplicationDbContext

# Apply auth database migrations
dotnet ef database update --context AuthDbContext
```

### Run the Application

```bash
dotnet run
```

Swagger UI is available at the root URL (`/`) in development mode.

---

##  Authentication

The API uses **JWT Bearer tokens** with two seeded roles: `Admin` and `User`.

### Register

```
POST /api/Auth/register
```

```json
{
  "userName": "john",
  "email": "john@example.com",
  "password": "secret123",
  "roles": ["User"]
}
```

### Login

```
POST /api/Auth/Login
```

```json
{
  "email": "john@example.com",
  "password": "secret123"
}
```

Returns a JWT token. Include it in all subsequent requests:

```
Authorization: Bearer <token>
```

---

##  API Endpoints

### Books

| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| GET | `/api/Book` | User, Admin | Get all books |
| GET | `/api/Book/{id}` | User, Admin | Get book by ID |
| GET | `/api/Book/search/{title}` | User, Admin | Search book by title |
| POST | `/api/Book` | Admin | Create a book |
| PUT | `/api/Book` | Admin | Update a book |
| DELETE | `/api/Book/{id}` | Admin | Delete a book |

### Categories

| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| GET | `/api/Category` | User, Admin | Get all categories |
| GET | `/api/Category/{id}` | User, Admin | Get category by ID |
| POST | `/api/Category` | Admin | Create a category |
| PUT | `/api/Category` | Admin | Update a category |
| DELETE | `/api/Category/{id}` | Admin | Delete a category |

---

##  Project Structure

```
BookStore/
├── Controllers/
│   ├── AuthController.cs
│   ├── BookController.cs
│   └── CategoryController.cs
├── Core/
│   ├── Entities/
│   │   ├── Book.cs
│   │   ├── Category.cs
│   │   ├── Cart.cs
│   │   └── CartItem.cs
│   ├── Repositories/Contract/
│   │   ├── IGenericRepository.cs
│   │   ├── ITokenRepository.cs
│   │   └── IUnitOfWork.cs
│   └── Services/Contract/
│       ├── IBookService.cs
│       ├── ICategoryService.cs
│       └── ICartService.cs
├── Repository/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── AuthDbContext.cs
│   └── Repositories/
│       ├── GenericRepository.cs
│       └── TokenRepository.cs
├── Services/
│   ├── BookService.cs
│   ├── CategoryService.cs
│   └── CartService.cs
└── Program.cs
```

---

##  Authorization Roles

| Role | Permissions |
|------|-------------|
| `User` | Read books and categories, manage own cart |
| `Admin` | Full CRUD on books and categories |

Roles are seeded automatically into the `AuthDbContext` on startup.

---

##  Cart Service

The cart is user-scoped and supports:

- `AddToCartAsync(userId, bookId, quantity)` — Add or update a cart item
- `RemoveFromCart(userId, bookId)` — Remove a specific item
- `ClearCart(userId)` — Remove the entire cart
- `GetCartItemsAsync(userId)` — List all items in a user's cart

> **Note:** A `CartController` exposing these endpoints is not yet included and can be added as a next step.

---

##  Dependencies

- `Microsoft.AspNetCore.Identity`
- `Microsoft.EntityFrameworkCore` + SQL Server provider
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.IdentityModel.Tokens`
- `Swashbuckle.AspNetCore` (Swagger)
