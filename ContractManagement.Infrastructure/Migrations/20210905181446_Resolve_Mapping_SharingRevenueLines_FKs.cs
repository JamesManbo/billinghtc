using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Resolve_Mapping_SharingRevenueLines_FKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uid",
                table: "TransactionServicePackages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uid",
                table: "OutContractServicePackages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InServiceChannelUid",
                table: "ContractSharingRevenueLines",
                maxLength: 68,
                nullable: true);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MapContractSharingRevenueLineToHead");
            migrationBuilder.AlterStoredProcedure("MapContractSharingRevenueLineToHead_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uid",
                table: "TransactionServicePackages");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "OutContractServicePackages");

            migrationBuilder.DropColumn(
                name: "InServiceChannelUid",
                table: "ContractSharingRevenueLines");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MapContractSharingRevenueLineToHead");
            migrationBuilder.AlterStoredProcedure("MapContractSharingRevenueLineToHead");
        }
    }
}
