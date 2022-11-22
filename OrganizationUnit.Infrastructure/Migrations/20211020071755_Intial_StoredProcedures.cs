using Microsoft.EntityFrameworkCore.Migrations;
using OrganizationUnit.Infrastructure.SqlScripts;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Intial_StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterStoredProcedure("GetSystemMenusByPermission");
            migrationBuilder.AlterStoredProcedure("GetPermissionOfUser_v3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
