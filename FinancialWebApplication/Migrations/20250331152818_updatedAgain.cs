using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class updatedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "monthlyBudget",
                columns: table => new
                {
                    accountKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    budgetMonth = table.Column<DateOnly>(type: "date", nullable: false),
                    monthlyBudget = table.Column<int>(type: "int", nullable: false)
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
        }
    }
}
