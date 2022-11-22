using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class CREATE_STORED_GetFeeReportData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeReportData");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeTotalReportData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
        }
    }
}
