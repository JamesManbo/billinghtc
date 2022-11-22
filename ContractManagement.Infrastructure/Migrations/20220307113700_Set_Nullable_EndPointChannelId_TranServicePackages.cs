using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Set_Nullable_EndPointChannelId_TranServicePackages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionServicePackages_TransactionChannelPoints_EndPoint~",
                table: "TransactionServicePackages");

            migrationBuilder.AlterColumn<int>(
                name: "EndPointChannelId",
                table: "TransactionServicePackages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionServicePackages_TransactionChannelPoints_EndPoint~",
                table: "TransactionServicePackages",
                column: "EndPointChannelId",
                principalTable: "TransactionChannelPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionServicePackages_TransactionChannelPoints_EndPoint~",
                table: "TransactionServicePackages");

            migrationBuilder.AlterColumn<int>(
                name: "EndPointChannelId",
                table: "TransactionServicePackages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionServicePackages_TransactionChannelPoints_EndPoint~",
                table: "TransactionServicePackages",
                column: "EndPointChannelId",
                principalTable: "TransactionChannelPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
