using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Update_SP_sp_GetReportServiceOutDebtData_v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceOutDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceOutDebtData_v7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceOutDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceOutDebtData_v6");
        }
    }
}
