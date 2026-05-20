# Medical Scheduling Backend

A modern medical scheduling backend built with .NET 8 using Clean Architecture and CQRS principles.

## Technologies

- .NET 10
- ASP.NET Core Minimal API
- Entity Framework Core
- PostgreSQL
- Redis
- JWT Authentication
- MediatR
- FluentValidation
- Docker
- Docker Compose

---

# Architecture

This project follows Clean Architecture principles.

```txt
Presentation
    ↓
Application
    ↓
Domain

Infrastructure
    ↓
Application
    ↓
Domain
```

---

# Layers

## Domain

Contains:
- Entities
- Enums
- Interfaces
- Value Objects
- Business Rules

---

## Application

Contains:
- CQRS
- Commands
- Queries
- DTOs
- Validators
- Use Cases

---

## Infrastructure

Contains:
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Redis Cache
- Repositories
- External Services

---

## Presentation

Contains:
- Minimal APIs
- Endpoints
- Middlewares
- Dependency Injection
- Swagger

---

# Features

- JWT Authentication
- Doctor and Patient Accounts
- Appointment Scheduling
- Schedule Blocking
- Medical History
- Dashboard
- Redis Cache
- Rate Limiting
- Automated Tests

---

# Project Structure

```txt
src
 ├── MedicalScheduling.Domain
 ├── MedicalScheduling.Application
 ├── MedicalScheduling.Infrastructure
 └── MedicalScheduling.Presentation
```

---

# Project References

```txt
Presentation -> Application
Presentation -> Infrastructure

Infrastructure -> Application
Infrastructure -> Domain

Application -> Domain
```

---

# Getting Started

## Clone repository

```bash
git clone <repository-url>
```

---

# Run Docker Services

## Start PostgreSQL and Redis

```bash
docker compose up -d
```

---

# Run API

```bash
cd MedicalScheduling.Presentation
dotnet run
```

---

# Entity Framework Migrations

## Create migration

```bash
dotnet ef migrations add InitialCreate \
--project MedicalScheduling.Infrastructure \
--startup-project MedicalScheduling.Presentation
```

---

## Update database

```bash
dotnet ef database update \
--project MedicalScheduling.Infrastructure \
--startup-project MedicalScheduling.Presentation
```

---

# Docker Services

## PostgreSQL
- Port: 5432

## Redis
- Port: 6379

---

# Future Improvements

- Background Jobs
- Email Notifications
- SMS Notifications
- Refresh Tokens
- OpenTelemetry
- Unit Tests
- Integration Tests
- CI/CD Pipeline
- Kubernetes Deployment

---

# Author

Developed by Raphael Bernardino.
