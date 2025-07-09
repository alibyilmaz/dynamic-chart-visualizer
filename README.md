# Dynamic Chart Visualizer

## Overview
A full-stack web application for dynamic data visualization from MSSQL databases. Users can connect to any MSSQL database, select a data object (view, stored procedure, or function), map fields, and render interactive charts.

---

## Features
- **Backend (.NET 8 Web API):**
  - Dynamic MSSQL connection (user-supplied credentials)
  - List and execute views, stored procedures, and functions
  - JWT authentication and logging
  - Sample data: Products, Sales, demo view, stored procedure, and function
  - CORS enabled for frontend
- **Frontend (React + TypeScript + Material-UI):**
  - Secure login (JWT, auto-logout on session expiration)
  - Database connection form
  - Dynamic object selection and data mapping
  - Chart rendering (Line, Bar, Radar) with Chart.js
  - Switch chart types on the fly
  - Logout button

---

## Quick Start

### 1. Backend Setup

#### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB, Express, or full SQL Server)
- Update connection string in `backend/DynamicChartApp/DynamicChartApp.API/appsettings.json` if needed

#### Database Setup
Navigate to the backend directory:
```sh
cd backend/DynamicChartApp
```

**Install EF Core tools (if not already installed):**
```sh
dotnet tool install --global dotnet-ef
```

**Build the project:**
```sh
dotnet build
```

**Apply all migrations and create database:**
```sh
dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

**Verify database objects created:**
- Tables: `Products`, `Sales`
- View: `vw_ProductSalesSummary`
- Stored Procedure: `sp_GetTopSellingProducts`
- Function: `fn_GetProductStock`

#### Run the Backend API
```sh
dotnet run --project DynamicChartApp.API
```
- **HTTPS:** `https://localhost:7185`
- **HTTP:** `http://localhost:5294`
- **Swagger UI:** `https://localhost:7185/swagger`

### 2. Frontend Setup
- **Install dependencies:**
  ```sh
  cd frontend
  npm install
  ```
- **Proxy setup:**
  - The React app is configured to proxy API requests to the backend (`http://localhost:5294`). See the `proxy` field in `frontend/package.json`.
- **Run the frontend:**
  ```sh
  npm start
  ```
- **App URL:** [http://localhost:3000](http://localhost:3000)

---

## Database Management Commands

### EF Core Commands (run from `backend/DynamicChartApp`)

**Create a new migration:**
```sh
dotnet ef migrations add MigrationName --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

**Apply migrations:**
```sh
dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

**Apply specific migration:**
```sh
dotnet ef database update MigrationName --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

**Remove last migration (if not applied):**
```sh
dotnet ef migrations remove --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

**Generate SQL script:**
```sh
dotnet ef migrations script --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

**Drop database:**
```sh
dotnet ef database drop --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

**List migrations:**
```sh
dotnet ef migrations list --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

### Database Objects Created
After running migrations, the following objects are created:

**Tables:**
- `Products` - Product catalog with Name, Category, Price, Stock
- `Sales` - Sales records with ProductId, Quantity, SaleDate, TotalAmount

**View:**
- `vw_ProductSalesSummary` - Aggregated product sales data

**Stored Procedure:**
- `sp_GetTopSellingProducts` - Returns top 5 selling products by quantity

**Function:**
- `fn_GetProductStock(@productId INT)` - Returns stock level for a product

**Sample Data:**
- 10 products with various categories and prices
- 15 sales records with realistic data

---

## Development vs Production
- **Development:**
  - Use `npm start` for the frontend and `dotnet run` for the backend.
  - The proxy and CORS settings allow seamless local development.
- **Production:**
  - Build the frontend: `npm run build` (output in `frontend/build`)
  - Publish the backend: `dotnet publish DynamicChartApp.API -c Release -o ./publish`
  - Serve the frontend build with a static file server or integrate with the backend.

---

## End-to-End Testing Checklist
- [ ] Backend running at `https://localhost:7185` or `http://localhost:5294`
- [ ] Frontend running at `http://localhost:3000` (proxy set)
- [ ] CORS enabled for frontend in backend
- [ ] Database created with all tables, views, stored procedures, and functions
- [ ] Login, connect, select object, map fields, render chart, logout all work

---

## User Flow
1. **Login:** Enter your username and password to obtain a JWT token. If your session expires, you will be automatically logged out and prompted to log in again.
2. **Connect to Database:** Enter MSSQL connection info (host, db, username, password).
3. **Select Data Object:** Choose a view, stored procedure, or function from the available list.
4. **Map Data Fields:** Assign columns to X Axis, Y Axis, and (optionally) Labels.
5. **Render Chart:** Instantly visualize your data as a Line, Bar, or Radar chart. Switch chart types on the fly.
6. **Logout:** Use the logout button in the top right to securely end your session.

---

## Sample Connection Info
```
Host: localhost
Database: DynamicChartDb
Username: sa
Password: <yourStrong(!)Password>
```

## Sample Data Objects
- **View:** `vw_ProductSalesSummary`
- **Stored Procedure:** `sp_GetTopSellingProducts`
- **Function:** `fn_GetProductStock`

---

## API Usage
### Authentication
- Obtain a JWT token via the authentication endpoint.
- Pass the token in the `Authorization: Bearer <token>` header for protected endpoints.

### Endpoints
- `POST /api/data/objects` — List all objects of a type (view, procedure, function)
  - Body: `{ "host": ..., "database": ..., "username": ..., "password": ..., "type": "view|procedure|function" }`
- `POST /api/data/execute` — Execute a selected object and get results
  - Body: `{ "host": ..., "database": ..., "username": ..., "password": ..., "objectName": ..., "objectType": ... }`

### Standard API Response
```
{
  "status": "Success",
  "message": "Execution completed successfully.",
  "data": {
    "columns": ["Column1", "Column2", ...],
    "rows": [
      { "Column1": "value1", "Column2": "value2", ... },
      ...
    ]
  },
  "execution": {
    "startedAt": "2024-07-09T19:54:00Z",
    "finishedAt": "2024-07-09T19:54:01Z",
    "durationMs": 1000
  }
}
```

---

## Technologies Used
- .NET 8 Web API
- Entity Framework Core
- Microsoft SQL Server
- JWT Authentication
- Swagger (OpenAPI)
- React.js + TypeScript + Material-UI + Chart.js

## Test Credentials
- Username: `sa`
- Password: `<yourStrong(!)Password>`

## Sample Database Structure
- See `backend/DynamicChartApp/DynamicChartApp.Infrastructure/Data/AppDbContext.cs` and migrations for schema and seed data.
- Demo objects are created in the latest migration.

---

## Troubleshooting

### Common Issues
1. **Migration fails:** Ensure SQL Server is running and connection string is correct
2. **Port conflicts:** Check if ports 7185/5294 are available for backend, 3000 for frontend
3. **CORS errors:** Verify CORS configuration in `Program.cs`
4. **Database objects missing:** Run `dotnet ef database update` to apply all migrations

### Reset Database
If you need to start fresh:
```sh
cd backend/DynamicChartApp
dotnet ef database drop --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
```

---

For any issues, please open an issue or contact the maintainer.