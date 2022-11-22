using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_Multiple_Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS RestoreOrSuspendServices");
            migrationBuilder.AlterStoredProcedure("RestoreOrSuspendServices_v5");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateServices");
            migrationBuilder.AlterStoredProcedure("TerminateServices_v5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS RestoreOrSuspendServices");
            migrationBuilder.AlterStoredProcedure("RestoreOrSuspendServices_v4");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS TerminateServices");
            migrationBuilder.AlterStoredProcedure("TerminateServices_v4");
        }
    }
}
