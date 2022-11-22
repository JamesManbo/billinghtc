using Microsoft.EntityFrameworkCore.Migrations;
using OrganizationUnit.Infrastructure.SqlScripts;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Update_Stored_GetPermissionOfUser_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPermissionOfUser");
            migrationBuilder.AlterStoredProcedure("GetPermissionOfUser_v3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPermissionOfUser");
            migrationBuilder.AlterStoredProcedure("GetPermissionOfUser_v2");
        }
    }
}
