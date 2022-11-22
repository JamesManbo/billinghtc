using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_GetOutContractSimpleAllByInContractId_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOutContractSimpleAllByInContractId");
            migrationBuilder.AlterStoredProcedure("GetOutContractSimpleAllByInContractId_v4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOutContractSimpleAllByInContractId");
            migrationBuilder.AlterStoredProcedure("GetOutContractSimpleAllByInContractId_v3");
        }
    }
}
