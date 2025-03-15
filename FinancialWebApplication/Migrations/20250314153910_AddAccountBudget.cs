using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountBudget",
                table: "AccountDetails",
                type: "int",
                nullable: false,
                defaultValue: 250);

            // Set default value to 200 for existing records
            migrationBuilder.Sql("UPDATE AccountDetails SET AccountBudget = 200 WHERE AccountBudget IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBudget",
                table: "AccountDetails");
        }
    }
}
