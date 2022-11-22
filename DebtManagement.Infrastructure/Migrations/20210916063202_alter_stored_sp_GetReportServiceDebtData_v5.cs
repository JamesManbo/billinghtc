using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class alter_stored_sp_GetReportServiceDebtData_v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceDebtData_v5");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceDebtData_v4");
        }
    }
}
