using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class monthlyBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBudget",
                table: "AccountDetails");

            migrationBuilder.CreateTable(
                name: "monthlyBudget",
                columns: table => new
                {
                    accountKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    budgetMonth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountBudget = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monthlyBudget", x => x.accountKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "monthlyBudget");

            migrationBuilder.AddColumn<decimal>(
                name: "AccountBudget",
                table: "AccountDetails",
                type: "decimal(20,2)",
                precision: 20,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
