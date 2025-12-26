# 🚀 CurlCode - Competitive Programming Platform

[![.NET](https://img.shields.io/badge/.NET-10.0-512bd4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/sql-server/)
[![Redis](https://img.shields.io/badge/Redis-Distributed_Cache-DC382D?style=for-the-badge&logo=redis)](https://redis.io/)

CurlCode is a competitive programming platform designed for practicing data structures and algorithms, following Clean Architecture and modern .NET patterns.

---

## ✨ Features

- **🔐 Robust Auth**: Complete user registration and authentication using ASP.NET Core Identity & JWT.
- **🏆 Contest Module**: Participate in timed contests with automated standings and participation management.
- **📚 Problem Repository**: Browse and solve algorithmic problems categorized by difficulty and topics.
- **� Community Solutions**: Share solution posts, discuss optimizations, and engage with others via likes and comments.
- **⚡ Performance First**: Integrated **Redis** for distributed caching of profile data and frequently accessed resources.
- **🩺 Observability**: Structured logging with **Serilog** and optional **Seq** integration for real-time monitoring.
- **⚙️ Automated Judging (Simulated)**: Submissions are currently evaluated through a simulated process (real judge engine integration planned).

---

## 🏗️ Architecture

The project follows the **Clean Architecture** pattern to maintain high decoupling and testability:

- **`CurlCode.Domain`**: Core entities (Problems, Submissions, Contests) and domain enums.
- **`CurlCode.Application`**: Business logic, DTOs, interfaces, and service implementations.
- **`CurlCode.Infrastructure`**: EF Core persistence, Redis caching implementation, and Identity configuration.
- **`CurlCode.API`**: RESTful endpoints, custom middleware, and Swagger documentation.

---

## � API Endpoints

### 🔑 Authentication
- `POST /api/auth/register` - Create a new account
- `POST /api/auth/login` - Authenticate and get JWT

### 👤 Profiles
- `GET /api/profiles/me` - Get personal profile stats
- `PUT /api/profiles/me` - Update profile information
- `GET /api/profiles/{username}` - View public profile of another user

### 🏆 Contests
- `GET /api/contests/upcoming` - List upcoming contests
- `GET /api/contests/running` - List current contests
- `GET /api/contests/past` - View completed contests
- `GET /api/contests/{id}/standings` - View contest leaderboard
- `POST /api/contests/{id}/join` - Register for a contest (Auth required)

### 🧩 Problems & Topics
- `GET /api/problems` - List all problems (supports paging/filtering)
- `GET /api/problems/{id}` - Detailed problem statement
- `GET /api/topics` - View available problem tags
- `POST /api/problems` - [Admin] Create a new problem

### 📤 Submissions
- `POST /api/submissions` - Submit code for a problem (Auth required)
- `GET /api/submissions/me` - My submission history
- `GET /api/submissions/{id}` - Specific submission result

---

## 🛠️ Tech Stack

- **Backend**: .NET 10.0, ASP.NET Core API
- **ORM**: Entity Framework Core 10.0 (SQL Server)
- **Caching**: StackExchange.Redis
- **Security**: ASP.NET Core Identity, JWT Bearer
- **Mapping & Validation**: AutoMapper, FluentValidation
- **Diagnostics**: Serilog, Seq, Swagger

---

## 🏁 Getting Started

1. **Configure Database**: Update `DefaultConnection` in `CurlCode.API/appsettings.json`.
2. **Configure Redis**: Ensure Redis is running (default `localhost:6379`).
3. **Database Setup**: The app auto-migrates and seeds on startup. Run it using:
   ```bash
   dotnet run --project CurlCode.API
   ```
4. **API UI**: Access the Swagger documentation at `https://localhost:5001/swagger`.


