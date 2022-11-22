using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Store_GetTotalFTTHProjectRevenue_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
        }
    }
}
