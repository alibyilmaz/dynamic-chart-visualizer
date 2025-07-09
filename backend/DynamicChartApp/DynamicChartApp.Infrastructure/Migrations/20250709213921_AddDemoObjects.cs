using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicChartApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the view if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_ProductSalesSummary]'))
                BEGIN
                    EXEC('CREATE VIEW vw_ProductSalesSummary AS
                    SELECT p.Name AS ProductName, SUM(s.Quantity) AS TotalQuantity, SUM(s.TotalAmount) AS TotalSales
                    FROM Products p
                    JOIN Sales s ON p.Id = s.ProductId
                    GROUP BY p.Name')
                END
            ");

            // Create the stored procedure if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetTopSellingProducts]'))
                BEGIN
                    EXEC('CREATE PROCEDURE sp_GetTopSellingProducts
                    AS
                    BEGIN
                        SELECT TOP 5 p.Name, SUM(s.Quantity) AS TotalSold
                        FROM Products p
                        JOIN Sales s ON p.Id = s.ProductId
                        GROUP BY p.Name
                        ORDER BY TotalSold DESC
                    END')
                END
            ");

            // Create the function if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND object_id = OBJECT_ID(N'[dbo].[fn_GetProductStock]'))
                BEGIN
                    EXEC('CREATE FUNCTION fn_GetProductStock(@productId INT)
                    RETURNS INT
                    AS
                    BEGIN
                        DECLARE @stock INT;
                        SELECT @stock = Stock FROM Products WHERE Id = @productId;
                        RETURN @stock;
                    END')
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ProductSalesSummary;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTopSellingProducts;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_GetProductStock;");
        }
    }
}
