using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_ActivatedUnit_ContractEquipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ActivatedUnit",
                table: "TransactionEquipments",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ActivatedUnit",
                table: "ContractEquipments",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivatedUnit",
                table: "TransactionEquipments");

            migrationBuilder.DropColumn(
                name: "ActivatedUnit",
                table: "ContractEquipments");
        }
    }
}
