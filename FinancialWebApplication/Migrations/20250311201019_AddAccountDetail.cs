using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountKey",
                table: "Account",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountKey",
                table: "Account");
        }
    }
}
