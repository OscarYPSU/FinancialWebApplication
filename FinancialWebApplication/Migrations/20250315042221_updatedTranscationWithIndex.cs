using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class updatedTranscationWithIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AccountKey",
                table: "Transactions",
                newName: "IndexAccountKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IndexAccountKey",
                table: "Transactions",
                newName: "IX_Transactions_AccountKey");
        }
    }
}
