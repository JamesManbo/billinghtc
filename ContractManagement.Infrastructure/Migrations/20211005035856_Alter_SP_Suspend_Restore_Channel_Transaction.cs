using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_Suspend_Restore_Channel_Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateContracts");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateServices");
            migrationBuilder.AlterStoredProcedure("TerminateContracts_v4");
            migrationBuilder.AlterStoredProcedure("TerminateServices_v4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateContracts");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateServices");
            migrationBuilder.AlterStoredProcedure("TerminateContracts_v3");
            migrationBuilder.AlterStoredProcedure("TerminateServices_v3");
        }
    }
}
