using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Update_SP_Reports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalDebtReport");
            migrationBuilder.AlterStoredProcedure("GetTotalDebtReport_v1");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v2");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
            migrationBuilder.AlterStoredProcedure("GetTotalRevenueEnterpiseReport_v2");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeReportData_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalDebtReport");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");
        }
    }
}
