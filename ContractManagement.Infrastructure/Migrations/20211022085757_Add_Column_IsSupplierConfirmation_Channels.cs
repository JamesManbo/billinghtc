using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_IsSupplierConfirmation_Channels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupplierConfirmation",
                table: "TransactionServicePackages",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSupplierConfirmation",
                table: "OutContractServicePackages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSupplierConfirmation",
                table: "TransactionServicePackages");

            migrationBuilder.DropColumn(
                name: "IsSupplierConfirmation",
                table: "OutContractServicePackages");
        }
    }
}
