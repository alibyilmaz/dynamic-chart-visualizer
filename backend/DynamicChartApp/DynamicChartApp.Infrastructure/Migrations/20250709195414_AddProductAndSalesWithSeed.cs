using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DynamicChartApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAndSalesWithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, "Electronics", "Laptop", 1200.00m, 50 },
                    { 2, "Electronics", "Smartphone", 800.00m, 120 },
                    { 3, "Furniture", "Desk Chair", 150.00m, 75 },
                    { 4, "Appliances", "Coffee Maker", 90.00m, 40 },
                    { 5, "Stationery", "Notebook", 5.00m, 500 },
                    { 6, "Stationery", "Pen", 1.50m, 1000 },
                    { 7, "Electronics", "Monitor", 300.00m, 60 },
                    { 8, "Accessories", "Backpack", 45.00m, 200 },
                    { 9, "Accessories", "Water Bottle", 15.00m, 350 },
                    { 10, "Furniture", "Desk Lamp", 35.00m, 80 }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "ProductId", "Quantity", "SaleDate", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 1, 3, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3600.00m },
                    { 2, 2, 5, new DateTime(2024, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000.00m },
                    { 3, 3, 7, new DateTime(2024, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1050.00m },
                    { 4, 4, 2, new DateTime(2024, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 180.00m },
                    { 5, 5, 50, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 250.00m },
                    { 6, 6, 100, new DateTime(2024, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 150.00m },
                    { 7, 7, 4, new DateTime(2024, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200.00m },
                    { 8, 8, 10, new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 450.00m },
                    { 9, 9, 20, new DateTime(2024, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 300.00m },
                    { 10, 10, 6, new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 210.00m },
                    { 11, 1, 2, new DateTime(2024, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2400.00m },
                    { 12, 2, 3, new DateTime(2024, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2400.00m },
                    { 13, 3, 5, new DateTime(2024, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 750.00m },
                    { 14, 4, 1, new DateTime(2024, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 90.00m },
                    { 15, 5, 100, new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 500.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_ProductSalesSummary AS
                SELECT p.Name AS ProductName, SUM(s.Quantity) AS TotalQuantity, SUM(s.TotalAmount) AS TotalSales
                FROM Products p
                JOIN Sales s ON p.Id = s.ProductId
                GROUP BY p.Name;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetTopSellingProducts
                AS
                BEGIN
                    SELECT TOP 5 p.Name, SUM(s.Quantity) AS TotalSold
                    FROM Products p
                    JOIN Sales s ON p.Id = s.ProductId
                    GROUP BY p.Name
                    ORDER BY TotalSold DESC
                END
            ");
            migrationBuilder.Sql(@"
                CREATE FUNCTION fn_GetProductStock(@productId INT)
                RETURNS INT
                AS
                BEGIN
                    DECLARE @stock INT;
                    SELECT @stock = Stock FROM Products WHERE Id = @productId;
                    RETURN @stock;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ProductSalesSummary;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTopSellingProducts;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_GetProductStock;");
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
