using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_IsMultipleTransaction_Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMultipleTransaction",
                table: "TransactionServicePackages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultipleTransaction",
                table: "Transactions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMultipleTransaction",
                table: "TransactionServicePackages");

            migrationBuilder.DropColumn(
                name: "IsMultipleTransaction",
                table: "Transactions");
        }
    }
}
