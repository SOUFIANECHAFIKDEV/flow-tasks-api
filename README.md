# Flow Tasks API

Backend for the Flow Tasks App — a collaborative to-do list demo project built with **ASP.NET Core 8** and **Entity Framework Core**.

## 🚀 Features
- Minimal APIs (clean and simple endpoint definitions)
- Entity Framework Core with SQL Server
- Database migrations and seeding
- CRUD operations for tasks
- Task assignment to users
- Pagination, sorting, and filtering
- Optimistic concurrency (row versioning)
- Soft delete functionality
- Swagger UI for API documentation

## 🛠️ Tech Stack
- **Framework**: ASP.NET Core 8
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

## 📂 Project Structure
- `Flow.Tasks.Api` – API entry point (endpoints, DI configuration, Swagger)
- `Flow.Tasks.Domain` – Entities & enums
- `Flow.Tasks.Infrastructure` – DbContext, repositories, migrations
- `Flow.Tasks.Application` – Abstractions and service layer

## ▶️ Run the API locally
1. Ensure **SQL Server** is running (or update connection string in `appsettings.json`).
2. Apply migrations (automatic or via CLI):
   ```bash
   dotnet ef database update --project Flow.Tasks.Infrastructure --startup-project Flow.Tasks.Api
Run the API:

bash
Copier le code
cd Flow.Tasks.Api
dotnet run
Open Swagger at https://localhost:7121/swagger.

📌 Example Endpoints
GET /tasks?page=1&pageSize=5&sortBy=title&desc=true&search=keyword

POST /tasks → create a new task

PATCH /tasks/{id}/status → update status

DELETE /tasks/{id} → soft delete

GET /users → list users

yaml
Copier le code
