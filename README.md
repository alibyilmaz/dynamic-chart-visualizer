## 📸 Demo

![Dynamic Chart Visualizer Demo](./images/demo.png)

*The application interface showing a bar chart of top-selling products from the `sp_GetTopSellingProducts` stored procedure.*

## 🚀 Quick Start

### Prerequisites

* .NET 8 SDK
* Node.js 18+ & npm
* SQL Server (LocalDB, Express, or full)

### 1. Backend Setup

```bash
# Restore NuGet packages
cd backend/DynamicChartApp
dotnet restore

# Build project
dotnet build

# Run migrations
dotnet ef database update --project DynamicChartApp.Infrastructure --startup-project DynamicChartApp.API

# Start API server
dotnet run --project DynamicChartApp.API
```

**API:** [https://localhost:7185/swagger](https://localhost:7185/swagger)

### 2. Frontend Setup

```bash
cd frontend
npm install
npm start
```

**App:** [http://localhost:3000](http://localhost:3000)

## ✨ Features

* **Dynamic SQL Server Connection** – Connect to any SQL Server database with user credentials
* **Database Object Execution** – List and execute views, stored procedures, and functions
* **Interactive Charts** – Visualize data with Line, Bar, and Radar charts (Chart.js)
* **JWT Authentication** – Secure login with auto-logout on session expiration
* **Material-UI Interface** – Modern, responsive React frontend
* **Real-time Data Mapping** – Dynamically map database columns to chart axes

## 📊 Sample Data

The application automatically creates demo data:

**Tables:**

* `Products` (10 sample products)
* `Sales` (15 sample sales records)

**Database Objects:**

* **View:** `vw_ProductSalesSummary` – Aggregated product sales data
* **Stored Procedure:** `sp_GetTopSellingProducts` – Top 5 selling products
* **Function:** `fn_GetProductStock(productId)` – Get stock for a specific product

## 🔧 Frontend Development Commands

```bash
# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build
```

## 🔗 API Endpoints

* `POST /api/Auth/token` – User authentication (Basic Auth required)
* `POST /api/Data/objects` – List database objects (Requires Bearer token)
* `POST /api/Data/execute` – Execute selected object (Requires Bearer token)

**Login Credentials:**
```
Username: testuser
Password: test123
```

**Sample Request:**

```json
{
  "Host": "localhost",
  "Database": "DynamicChartDb",
  "Username": "sa",
  "Password": "yourPassword",
  "ObjectName": "vw_ProductSalesSummary",
  "ObjectType": "View"
}
```

## 🧰 Tech Stack

**Backend:**

* .NET 8 Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger/OpenAPI

**Frontend:**

* React 18 + TypeScript
* Material-UI (MUI)
* Chart.js
* Axios for API calls

## 🐛 Troubleshooting

* **Database connection fails:** Verify SQL Server is running and credentials are correct.
* **Port conflicts:** Ensure ports 7185 (API) and 3000 (React) are available.
* **Missing demo data:** Run `dotnet ef database update` to apply migrations.

---

Need help? Open an issue on GitHub or check the Swagger docs at `/swagger`.
