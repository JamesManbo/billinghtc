using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Update_SP_Report_incontract_fee_andsharing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeTotalReportData_v3");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTotalDataIncontractFeeSharingReport");
            migrationBuilder.AlterStoredProcedure("sp_GetTotalDataIncontractFeeSharingReport_v2");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeTotalReportData_v2");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTotalDataIncontractFeeSharingReport");
            migrationBuilder.AlterStoredProcedure("sp_GetTotalDataIncontractFeeSharingReport");
        }
    }
}
