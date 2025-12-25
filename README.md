# CurlCode - Competitive Programming Platform

CurlCode is a competitive programming platform designed to help developers master data structures and algorithms, similar to LeetCode.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with the following layers:

- **Domain Layer** (`CurlCode.Domain`): Pure entities, enums, and domain logic (no dependencies)
- **Application Layer** (`CurlCode.Application`): Business logic, DTOs, service interfaces and implementations
- **Infrastructure Layer** (`CurlCode.Infrastructure`): Data access, external services (JWT, Identity)
- **API Layer** (`CurlCode.API`): Controllers, middleware, and API configuration

## 🚀 Tech Stack

- **.NET 10.0** - Latest .NET framework
- **SQL Server** - Database
- **Entity Framework Core** - ORM
- **ASP.NET Core Identity** - Authentication & Authorization
- **JWT Bearer** - Token-based authentication
- **AutoMapper** - Object mapping
- **Swagger** - API documentation

## 📁 Project Structure

```
CurlCode/
├── CurlCode.Domain/                    # Domain Layer
│   ├── Common/                         # Base entities
│   ├── Entities/
│   │   ├── Community/                  # Solutions, comments, likes
│   │   ├── Identity/                   # Users, profiles
│   │   ├── Problems/                   # Problems, topics, test cases
│   │   ├── StudyPlans/                 # Study plans and progress
│   │   └── Submissions/                # Code submissions
│   └── Enums/                          # Domain enumerations
│
├── CurlCode.Application/               # Application Layer
│   ├── Common/
│   │   ├── Constants/                  # Cache keys, constants
│   │   ├── Exceptions/                 # Custom exceptions
│   │   ├── Mappings/                   # AutoMapper profiles
│   │   └── Models/                     # Shared models
│   ├── Contracts/
│   │   ├── Infrastructure/             # Service interfaces
│   │   └── Persistence/                # Repository interfaces
│   ├── DTOs/                           # Data Transfer Objects
│   ├── Services/                       # Business logic implementations
│   └── Validators/                     # FluentValidation validators
│
├── CurlCode.Infrastructure/            # Infrastructure Layer
│   ├── Identity/                       # Identity configuration & seeding
│   ├── Persistence/
│   │   ├── Configurations/            # EF Core entity configurations
│   │   ├── Contexts/                  # DbContext
│   │   └── Repositories/              # Repository implementations
│   └── Services/                      # External service implementations
│
├── CurlCode.API/                       # API Layer
│   ├── Controllers/                   # API endpoints
│   └── Middlewares/                   # Custom middleware
│
└── CurlCode.Tests/                    # Test Layer
    └── Services/                      # Unit tests for services
```

## 🔐 API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token
- `POST /refresh-token` - Refresh access token
- `POST /forgot-password` - Request password reset
- `POST /reset-password` - Reset password with token
- `POST /logout - Logout` (invalidate token)

### Profiles
- `GET /api/profiles/me` - Get current user's profile (Auth required)
- `PUT /api/profiles/me` - Update profile (Auth required)
- `GET /api/profiles/{username}` - View another user's profile

### Topics
- `GET /api/topics` - List all topics
- `POST /api/topics` - Create topic (Admin only)
- `PUT /api/topics/{id}` - Update topic (Admin only)
- `DELETE /api/topics/{id}` - Delete topic (Admin only)

### Problems
- `GET /api/problems` - List problems (with pagination, filtering, sorting)
- `GET /api/problems/{id}` - Get problem details
- `POST /api/problems` - Create problem (Admin only)
- `PUT /api/problems/{id}` - Update problem (Admin only)
- `DELETE /api/problems/{id}` - Delete problem (Admin only)

### Test Cases
- `GET /api/problems/{problemId}/testcases` - Get test cases (Admin only)
- `POST /api/problems/{problemId}/testcases` - Add test case (Admin only)
- `DELETE /api/testcases/{id}` - Remove test case (Admin only)

### Submissions
- `POST /api/submissions` - Submit code for judging (Auth required)
- `GET /api/submissions/me` - Get my submission history (Auth required)
- `GET /api/submissions/{id}` - Get submission result (Auth required)

### Solutions & Community
- `GET /api/solutions` - List solutions (with pagination, sorting)
- `GET /api/solutions/{id}` - Read a specific solution
- `GET /api/solutions/problem/{problemId}` - Get solutions for a problem
- `POST /api/solutions` - Create solution post (Auth required)
- `PUT /api/solutions/{id}` - Update solution (Author only)
- `DELETE /api/solutions/{id}` - Delete solution (Author only)
- `POST /api/solutions/{id}/like` - Like/Unlike solution (Auth required)
- `POST /api/solutions/{id}/comments` - Add comment (Auth required)

## 🛠️ Setup & Running

### Prerequisites
- .NET 10.0 SDK
- SQL Server (LocalDB or SQL Server Express)

### Configuration

1. Update the connection string in `CurlCode.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CurlCodeDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

2. Update JWT settings in `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "CurlCode",
    "Audience": "CurlCode"
  }
}
```

### Running the Application

1. Restore packages:
```bash
dotnet restore
```

2. Build the solution:
```bash
dotnet build
```

3. Run the API:
```bash
cd CurlCode.API
dotnet run
```

4. Access Swagger UI:
```
https://localhost:5001/swagger
```

### Database Migrations

The application uses `EnsureCreated()` for development. For production, use migrations:

```bash
cd CurlCode.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../CurlCode.API
dotnet ef database update --startup-project ../CurlCode.API
```

## 🔑 Authentication

1. Register a new user via `POST /api/auth/register`
2. Login via `POST /api/auth/login` to receive a JWT token
3. Include the token in the `Authorization` header for protected endpoints:
```
Authorization: Bearer <your-jwt-token>
```

## 👤 Admin Access

To create an admin user, set `IsAdmin = true` in the database for the user record, or modify the registration logic to allow admin creation.

## 📝 Notes

- The Judge service (C-based backend for code execution) is not implemented in this MVP. The submission service currently simulates acceptance.
- In production, integrate with a proper code execution service or message queue system.
- Update the JWT key in production to a secure, randomly generated key.

## 🎯 Features

- ✅ User registration and authentication (JWT)
- ✅ Problem management (CRUD)
- ✅ Topic management
- ✅ Test case management
- ✅ Code submission (with placeholder judge integration)
- ✅ Solution posts and community features
- ✅ User profiles with statistics
- ✅ Like and comment on solutions
- ✅ Pagination and filtering
- ✅ Role-based authorization (Admin/User)

## 📄 License

This project is part of the CurlCode platform.






