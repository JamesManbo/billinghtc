using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class ALTERSTORED_GetListMasterCustomerReport_Alter_filterByCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v12");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetListMasterCustomerReport");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v11");
        }
    }
}
