using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class alterStored_GetOutContractSharingRevenueByInContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOutContractSharingRevenueByInContract");
            migrationBuilder.AlterStoredProcedure("GetOutContractSharingRevenueByInContract_v5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOutContractSharingRevenueByInContract");
            migrationBuilder.AlterStoredProcedure("GetOutContractSharingRevenueByInContract_v4");
        }
    }
}
