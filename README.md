#  BookStore API

##  About

RESTful Book Store API built with ASP.NET Core using Clean Architecture, Entity Framework, and SQL Server. Supports managing books, categories, and carts with scalable and maintainable design.

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
├── Dtos/
│   ├── Incoming/
│   │   ├── CreateBookDto.cs
│   │   ├── UpdateBookDto.cs
│   │   ├── CreateCategoryDto.cs
│   │   ├── UpdateCategoryDto.cs
│   │   └── LoginRequestDto.cs
│   └── outgoingDtos/
│       ├── RegisterRequestDto.cs
│       └── LoginResponseDto.cs
└── Program.cs
```

---

##  DTOs (Data Transfer Objects)

### Incoming (Request)

| DTO | Used In | Fields |
|-----|---------|--------|
| `CreateBookDto` | `POST /api/Book` | `Title`, `Price`, `CategoryId?` |
| `UpdateBookDto` | `PUT /api/Book` | `Id`, `Title`, `Price` |
| `CreateCategoryDto` | `POST /api/Category` | `Name` |
| `UpdateCategoryDto` | `PUT /api/Category` | `Id`, `Name` |
| `LoginRequestDto` | `POST /api/Auth/Login` | `Email` *(required, email)*, `Password` *(required)* |
| `RegisterRequestDto` | `POST /api/Auth/register` | `UserName`, `Email`, `Password` *(all required)*, `Roles[]` |

### Outgoing (Response)

| DTO | Used In | Fields |
|-----|---------|--------|
| `LoginResponseDto` | `POST /api/Auth/Login` | `Token` |

---

##  Authorization Roles

| Role | Permissions |
|------|-------------|
| `User` | Read books and categories, manage own cart |
| `Admin` | Full CRUD on books and categories |

Roles are seeded automatically into the `AuthDbContext` on startup.

---

##  Book Service

The book service handles all book-related business logic:

- `GetAllBooksAsync()` — Retrieve all books
- `GetBookByIdAsync(id)` — Find a book by its GUID
- `GetBookByTitleAsync(title)` — Search books by title (partial match supported)
- `CreateBookAsync(book)` — Add a new book linked to a category
- `UpdateBookAsync(book)` — Update book title and price
- `DeleteBookAsync(id)` — Remove a book by ID

---

##  Category Service

The category service manages book categories:

- `GetAllCategoriesAsync()` — Retrieve all categories
- `GetCategoryByIdAsync(id)` — Find a category by its GUID
- `CreateCategoryAsync(category)` — Create a new category
- `UpdateCategoryAsync(category)` — Rename an existing category
- `DeleteCategoryAsync(id)` — Remove a category by ID

> **Note:** Each `Book` has an optional `CategoryId` foreign key. Deleting a category while books are still linked to it may cause constraint violations depending on your cascade delete configuration.

---

##  Cart Service

The cart is user-scoped and supports:

- `AddToCartAsync(userId, bookId, quantity)` — Add or update a cart item
- `RemoveFromCart(userId, bookId)` — Remove a specific item
- `ClearCart(userId)` — Remove the entire cart
- `GetCartItemsAsync(userId)` — List all items in a user's cart

> **Note:** A `CartController` exposing these endpoints is not yet included and can be added as a next step.

---

##  Swagger Configuration

Swagger is configured as an extension method in `SwaggerServiceExtensions.cs` to keep `Program.cs` clean.

### Registration (in `Program.cs`)

```csharp
builder.Services.AddSwaggerDocumentation();
// ...
app.UseSwaggerDocumentation();
```

### What it does

- **`AddSwaggerDocumentation`** — registers Swagger with API info (title: `Book Store`, version: `v1`) and adds JWT Bearer security definition so you can authorize directly from the Swagger UI using your token.
- **`UseSwaggerDocumentation`** — enables the Swagger middleware and serves the UI at the **root URL** (`/`) instead of `/swagger`.

### Authorizing in Swagger UI

1. Run the app and open `http://localhost:<port>/`
2. Call `POST /api/Auth/Login` to get your JWT token
3. Click the **Authorize ** button at the top right
4. Enter your token as:
   ```
   Bearer <your-token-here>
   ```
5. All subsequent requests will include the token automatically

### Extension Class

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BookStore
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Book Store" });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                c.RoutePrefix = string.Empty; // Swagger at root URL
            });
            return app;
        }
    }
}
```

---

##  Dependencies

- `Microsoft.AspNetCore.Identity`
- `Microsoft.EntityFrameworkCore` + SQL Server provider
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.IdentityModel.Tokens`
- `Swashbuckle.AspNetCore` (Swagger)
