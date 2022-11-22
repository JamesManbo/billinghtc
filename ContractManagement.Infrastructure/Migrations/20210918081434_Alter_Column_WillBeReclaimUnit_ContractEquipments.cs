using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_Column_WillBeReclaimUnit_ContractEquipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWillBeReclaim",
                table: "TransactionEquipments");

            migrationBuilder.AlterColumn<float>(
                name: "WillBeReclaimUnit",
                table: "TransactionEquipments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WillBeReclaimUnit",
                table: "TransactionEquipments",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<bool>(
                name: "IsWillBeReclaim",
                table: "TransactionEquipments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
