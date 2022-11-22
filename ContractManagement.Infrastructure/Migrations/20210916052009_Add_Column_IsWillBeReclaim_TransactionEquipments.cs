using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_IsWillBeReclaim_TransactionEquipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldId",
                table: "OutContractServicePackages");

            migrationBuilder.AddColumn<bool>(
                name: "IsWillBeReclaim",
                table: "TransactionEquipments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWillBeReclaim",
                table: "TransactionEquipments");

            migrationBuilder.AddColumn<int>(
                name: "OldId",
                table: "OutContractServicePackages",
                type: "int",
                nullable: true);
        }
    }
}
