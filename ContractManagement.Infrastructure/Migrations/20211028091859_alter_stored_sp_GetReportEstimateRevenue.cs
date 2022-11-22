using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class alter_stored_sp_GetReportEstimateRevenue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportEstimateRevenue");
            migrationBuilder.AlterStoredProcedure("sp_GetReportEstimateRevenue_v6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportEstimateRevenue");
            migrationBuilder.AlterStoredProcedure("sp_GetReportEstimateRevenue_5");
        }
    }
}
