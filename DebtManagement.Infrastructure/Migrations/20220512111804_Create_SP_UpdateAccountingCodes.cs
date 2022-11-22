using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Create_SP_UpdateAccountingCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateAccountingCodes");
            migrationBuilder.AlterStoredProcedure("UpdateAccountingCodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
