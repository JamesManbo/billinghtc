using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_sp_GetFeeTotalReportData_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeTotalReportData_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeTotalReportData_v1");
        }
    }
}
