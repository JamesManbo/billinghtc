using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_MapContractSharingRevenueLineToHead_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MapContractSharingRevenueLineToHead");
            migrationBuilder.AlterStoredProcedure("MapContractSharingRevenueLineToHead_v3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MapContractSharingRevenueLineToHead");
            migrationBuilder.AlterStoredProcedure("MapContractSharingRevenueLineToHead_v2");
        }
    }
}
