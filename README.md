# TaskManager Fullstack

## Structure

- backend/ — ASP.NET Core Web API (JWT, EF Core, SQLite)
- frontend/ — React + Vite (Login/Register, Tasks UI)

## Run backend

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run

Swagger: http://localhost:5051/swagger

## Run frontend
cd frontend
npm install
npm run dev

Frontend: http://localhost:5173
```
