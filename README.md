# Dynamic Chart Visualizer

## 🚀 Quick Start

1. **Clone the repo & install prerequisites:**
   - .NET 8 SDK
   - Node.js & npm
   - SQL Server (LocalDB, Express, or full)
2. **Backend:**
   - `cd backend/DynamicChartApp`
   - `dotnet build`
   - `dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API`
   - `dotnet run --project DynamicChartApp.API`
   - API: [https://localhost:7185/swagger](https://localhost:7185/swagger)
3. **Frontend:**
   - `cd frontend`
   - `npm install`
   - `npm start`
   - App: [http://localhost:3000](http://localhost:3000)

---

## Features
- **Backend (.NET 8 Web API):**
  - Dynamic MSSQL connection (user-supplied credentials)
  - List and execute views, stored procedures, and functions
  - JWT authentication and logging (all responses, all status codes)
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

## Database Objects & Sample Data
After running migrations, the following are created **automatically**:

- **Tables:**
  - `Products` (Name, Category, Price, Stock)
  - `Sales` (ProductId, Quantity, SaleDate, TotalAmount)
- **View:**
  - `vw_ProductSalesSummary` (Aggregated product sales)
- **Stored Procedure:**
  - `sp_GetTopSellingProducts` (Top 5 selling products)
- **Function:**
  - `fn_GetProductStock(@productId INT)` (Stock for a product)
- **Sample Data:**
  - 10 products, 15 sales records (see migrations for details)

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

## Build & Run Details

### Backend
- **Build:**
  ```sh
  cd backend/DynamicChartApp
  dotnet build
  ```
- **Apply migrations & seed data:**
  ```sh
  dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
  ```
- **Run:**
  ```sh
  dotnet run --project DynamicChartApp.API
  ```
- **Swagger UI:** [https://localhost:7185/swagger](https://localhost:7185/swagger)

### Frontend
- **Install dependencies:**
  ```sh
  cd frontend
  npm install
  ```
- **Run:**
  ```sh
  npm start
  ```
- **App URL:** [http://localhost:3000](http://localhost:3000)

---

## EF Core Database Management
- **Create migration:**
  ```sh
  dotnet ef migrations add MigrationName --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
  ```
- **Apply migrations:**
  ```sh
  dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
  ```
- **Drop database:**
  ```sh
  dotnet ef database drop --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
  ```
- **List migrations:**
  ```sh
  dotnet ef migrations list --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
  ```

---

## User Flow
1. **Login:** Enter your username and password to obtain a JWT token. If your session expires, you will be automatically logged out and prompted to log in again.
2. **Connect to Database:** Enter MSSQL connection info (host, db, username, password).
3. **Select Data Object:** Choose a view, stored procedure, or function from the available list.
4. **Map Data Fields:** Assign columns to X Axis, Y Axis, and (optionally) Labels.
5. **Render Chart:** Instantly visualize your data as a Line, Bar, or Radar chart. Switch chart types on the fly.
6. **Logout:** Use the logout button in the top right to securely end your session.

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

## Troubleshooting
- **Migration fails:** Ensure SQL Server is running and connection string is correct
- **Port conflicts:** Check if ports 7185/5294 are available for backend, 3000 for frontend
- **CORS errors:** Verify CORS configuration in `Program.cs`
- **Database objects missing:** Run `dotnet ef database update` to apply all migrations
- **Reset database:**
  ```sh
  cd backend/DynamicChartApp
  dotnet ef database drop --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
  dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API
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

For any issues, please open an issue or contact the maintainer.