using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class ALTER_REPORT_STORED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceDebtData_v3");
            
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceOutDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceOutDebtData_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceDebtData_v2");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceOutDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceOutDebtData");

        }
    }
}
