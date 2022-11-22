using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class ALTER_STOREDPRO_sp_GetReportCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportCustomer");
            migrationBuilder.AlterStoredProcedure("sp_GetReportCustomer_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportCustomer");
            migrationBuilder.AlterStoredProcedure("sp_GetReportCustomer");
        }
    }
}
