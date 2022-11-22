using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_GetTotalFTTHProjectRevenue_v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS FIRST_DAY");
            migrationBuilder.AlterFunction("FIRST_DAY");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v7");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSummaryFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetSummaryFTTHProjectRevenue_v1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v6");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSummaryFTTHProjectRevenue");
        }
    }
}
