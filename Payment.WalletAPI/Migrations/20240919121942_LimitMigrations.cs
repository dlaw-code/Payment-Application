using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.WalletAPI.Migrations
{
    /// <inheritdoc />
    public partial class LimitMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DailyLimit",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DailySpent",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyLimit",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DailySpent",
                table: "Accounts");
        }
    }
}
