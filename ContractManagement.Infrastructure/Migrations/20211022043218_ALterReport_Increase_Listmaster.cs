using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class ALterReport_Increase_Listmaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v15");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportIncreasementOutContract");
            migrationBuilder.AlterStoredProcedure("sp_GetReportIncreasementOutContract_v8");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v14");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportIncreasementOutContract");
            migrationBuilder.AlterStoredProcedure("sp_GetReportIncreasementOutContract_v7");
        }
    }
}
