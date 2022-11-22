using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class alter_stored_GetListMasterCustomerReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v10");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v9");
        }
    }
}
