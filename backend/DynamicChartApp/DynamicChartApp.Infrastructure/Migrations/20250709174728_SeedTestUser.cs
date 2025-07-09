using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicChartApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Username" },
                values: new object[] { 1, "ecd71870b6a7ea5e66d303bc3e2554e9755c25a9f1f5c7e6d7f62f8fa6b66109", "testuser" });
        }
    }
}
