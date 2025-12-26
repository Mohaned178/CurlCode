# 🚀 CurlCode - Competitive Programming Platform

[![.NET](https://img.shields.io/badge/.NET-10.0-512bd4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/sql-server/)
[![Redis](https://img.shields.io/badge/Redis-Distributed_Cache-DC382D?style=for-the-badge&logo=redis)](https://redis.io/)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)

CurlCode is a high-performance, real-world competitive programming platform designed for developers to master data structures and algorithms. Built with a focus on scalability, reliability, and clean architecture.

---

## ✨ Features

### 🏆 Competition & Training
- **Contest Module**: Real-time programming contests with live standings, automated timers, and participation management.
- **Problem Bank**: Extensive library of problems categorized by difficulty (Easy, Medium, Hard) and topics.
- **Study Plans**: Structured learning tracks to guide developers through specific algorithm domains.

### 👥 Community & Social
- **Solution Posts**: Share your solutions with the community using Markdown.
- **Interactions**: Like, comment, and discuss optimization strategies on others' solutions.
- **User Profiles**: Comprehensive statistics, contribution heatmaps, and global ranking.

### ⚙️ Technical Core
- **Automated Judging**: (Simulated) Submissions are evaluated against multiple test cases.
- **Distributed Caching**: Redis integration for lightning-fast profile and problem retrieval.
- **Structured Logging**: Centralized logging via Serilog and Seq for production-grade observability.

---

## 🏗️ Architecture

The project adheres to **Clean Architecture** principles, ensuring separation of concerns and maintainability.

- **`CurlCode.Domain`**: Pure entities, enums, and domain-driven logic. No external dependencies.
- **`CurlCode.Application`**: The orchestration layer containing business logic, DTOs, interfaces, and CQRS-style services.
- **`CurlCode.Infrastructure`**: Implementation of persistence (EF Core), identity management, and external services (Redis, Email).
- **`CurlCode.API`**: The entry point providing a RESTful interface, middleware, and OpenAPI documentation.

---

## 🚀 Tech Stack

- **Framework**: .NET 10.0
- **Database**: SQL Server
- **Caching**: StackExchange.Redis
- **Security**: ASP.NET Core Identity + JWT Bearer
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Logging**: Serilog (Console, File, Seq)
- **Documentation**: Swagger/OpenAPI

---

## 📁 Project Structure

```bash
CurlCode/
├── CurlCode.Domain/         # Core Domain Entities & Logic
├── CurlCode.Application/    # Business Logic & Service Interfaces
├── CurlCode.Infrastructure/ # Data Access & External Implementations
├── CurlCode.API/            # Web API Controllers & Configuration
└── CurlCode.Tests/          # Comprehensive Unit Test Suite
```

---

## 🛠️ Getting Started

### 📋 Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (LocalDB or Express)
- [Redis Server](https://redis.io/download/) (Local or Docker)
- [Seq](https://datalust.co/download) (Optional, for logging)

### ⚙️ Configuration

1. Update `appsettings.json` in `CurlCode.API`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_CONNECTION_STRING",
    "Redis": "localhost:6379"
  },
  "Serilog": {
    "WriteTo": [
      { "Name": "Seq", "Args": { "serverUrl": "http://localhost:5341" } }
    ]
  }
}
```

### 🏃 Running the App

1. **Restore & Build**:
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Initialize Database**:
   ```bash
   cd CurlCode.API
   dotnet run --seed # Database auto-migrates and seeds on startup
   ```

3. **Explore the API**:
   Navigate to `https://localhost:5001/swagger` to view the interactive documentation.

---

## 🔐 Authentication

1. Register via `POST /api/auth/register`
2. Login via `POST /api/auth/login` to get your JWT.
3. Add `Bearer <token>` to your request headers.

---

## 📝 Roadmap

- [x] Phase 1: Core Stabilisation & Infrastructure
- [x] Phase 2: Distributed Caching & Scaling
- [/] Phase 3: Contest Module & Advanced Social Features
- [ ] Phase 4: Real-time SignalR Feedback
- [ ] Phase 5: Production Judge Service Integration (C++ Backend)

---

## 📄 License
This project is licensed under the MIT License.
