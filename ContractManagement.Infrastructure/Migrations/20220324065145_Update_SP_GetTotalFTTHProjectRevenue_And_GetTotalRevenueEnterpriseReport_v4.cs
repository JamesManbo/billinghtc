using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Update_SP_GetTotalFTTHProjectRevenue_And_GetTotalRevenueEnterpriseReport_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v4");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
            migrationBuilder.AlterStoredProcedure("GetTotalRevenueEnterpiseReport_v4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
        }
    }
}
