using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_GetNeedToPaymentContractServices_v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetNeedToPaymentContractServices");
            migrationBuilder.AlterStoredProcedure("GetNeedToPaymentContractServices_v9");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetNeedToPaymentContractServices");
            migrationBuilder.AlterStoredProcedure("GetNeedToPaymentContractServices_v8");
        }
    }
}
