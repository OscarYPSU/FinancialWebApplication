using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingAccountBudgetWithTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AccountDetails_AccountKey",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "AccountKey",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountBudget",
                table: "AccountDetails",
                type: "decimal(20,2)",
                precision: 20,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AccountDetails_AccountKey",
                table: "Transactions",
                column: "AccountKey",
                principalTable: "AccountDetails",
                principalColumn: "AccountKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AccountDetails_AccountKey",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "AccountKey",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountBudget",
                table: "AccountDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)",
                oldPrecision: 20,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AccountDetails_AccountKey",
                table: "Transactions",
                column: "AccountKey",
                principalTable: "AccountDetails",
                principalColumn: "AccountKey",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
