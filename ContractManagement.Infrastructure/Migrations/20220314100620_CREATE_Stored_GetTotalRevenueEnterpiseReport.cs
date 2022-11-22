using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class CREATE_Stored_GetTotalRevenueEnterpiseReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
            migrationBuilder.AlterStoredProcedure("GetTotalRevenueEnterpiseReport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
           
        }
    }
}
