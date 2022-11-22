using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Update_SP_GetTotalFTTHProjectRevenue_v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v5");
        }
    }
}
