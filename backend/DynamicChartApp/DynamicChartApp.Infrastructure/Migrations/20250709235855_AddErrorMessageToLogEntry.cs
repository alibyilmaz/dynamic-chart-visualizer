using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicChartApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorMessageToLogEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "Logs");
        }
    }
}
