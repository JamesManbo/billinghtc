using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class ALETR_STORED_GetTotalFTTHProjectRevenue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v5");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
            migrationBuilder.AlterStoredProcedure("GetTotalRevenueEnterpiseReport_v5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v4");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
            migrationBuilder.AlterStoredProcedure("GetTotalRevenueEnterpiseReport_v4");
        }
    }
}
