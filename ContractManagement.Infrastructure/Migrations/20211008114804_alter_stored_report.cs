using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class alter_stored_report : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeTotalReportData_v1");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeReportData_v1");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportIncreasementOutContract");
            migrationBuilder.AlterStoredProcedure("sp_GetReportIncreasementOutContract_v6");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v13");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeTotalReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeTotalReportData");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFeeReportData");
            migrationBuilder.AlterStoredProcedure("sp_GetFeeReportData");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportIncreasementOutContract");
            migrationBuilder.AlterStoredProcedure("sp_GetReportIncreasementOutContract_v5");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v12");
        }
    }
}
