using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_UpdateNextBillingDateOfPayingContracts_v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateNextBillingDateOfPayingContracts");
            migrationBuilder.AlterStoredProcedure("UpdateNextBillingDateOfPayingContracts_v5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateNextBillingDateOfPayingContracts");
            migrationBuilder.AlterStoredProcedure("UpdateNextBillingDateOfPayingContracts_v4");
        }
    }
}
