using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Columns_SupporterHoldedUnit_DeploymentEquipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "SupporterHoldedUnit",
                table: "TransactionEquipments",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "WillBeHoldUnit",
                table: "TransactionEquipments",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SupporterHoldedUnit",
                table: "ContractEquipments",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.Sql("UPDATE TransactionEquipments SET SupporterHoldedUnit = 0 WHERE 1 = 1;");
            migrationBuilder.Sql("UPDATE ContractEquipments SET SupporterHoldedUnit = 0 WHERE 1 = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupporterHoldedUnit",
                table: "TransactionEquipments");

            migrationBuilder.DropColumn(
                name: "WillBeHoldUnit",
                table: "TransactionEquipments");

            migrationBuilder.DropColumn(
                name: "SupporterHoldedUnit",
                table: "ContractEquipments");
        }
    }
}
