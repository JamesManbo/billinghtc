using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_TerminateServices_TerminateContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateContracts");
            migrationBuilder.AlterStoredProcedure("TerminateContracts_v3");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateServices");
            migrationBuilder.AlterStoredProcedure("TerminateServices_v3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateContracts");
            migrationBuilder.AlterStoredProcedure("TerminateContracts_v2");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateServices");
            migrationBuilder.AlterStoredProcedure("TerminateServices_v2");
        }
    }
}
