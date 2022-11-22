using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_MapContractSharingRevenueLineToHead_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutServiceChannelUid",
                table: "ContractSharingRevenueLines",
                maxLength: 68,
                nullable: true);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MapContractSharingRevenueLineToHead");
            migrationBuilder.AlterStoredProcedure("MapContractSharingRevenueLineToHead_v4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutServiceChannelUid",
                table: "ContractSharingRevenueLines");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MapContractSharingRevenueLineToHead");
            migrationBuilder.AlterStoredProcedure("MapContractSharingRevenueLineToHead_v3");
        }
    }
}
