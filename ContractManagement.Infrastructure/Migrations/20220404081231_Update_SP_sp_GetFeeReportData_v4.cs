using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Update_SP_sp_GetFeeReportData_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeReportData_v4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeReportData_v3");
        }
    }
}
