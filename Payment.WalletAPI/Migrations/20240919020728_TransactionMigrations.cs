using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.WalletAPI.Migrations
{
    /// <inheritdoc />
    public partial class TransactionMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transactions",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Transactions",
                newName: "TransactionType");
        }
    }
}
