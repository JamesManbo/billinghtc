using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Rename_Tbl_OrganizationUnitUser_2_OrganizationUnitUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUnitUser_OrganizationUnits_OrganizationUnitId",
                table: "OrganizationUnitUser");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUnitUser_Users_UserId",
                table: "OrganizationUnitUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationUnitUser",
                table: "OrganizationUnitUser");

            migrationBuilder.RenameTable(
                name: "OrganizationUnitUser",
                newName: "OrganizationUnitUsers");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationUnitUser_UserId",
                table: "OrganizationUnitUsers",
                newName: "IX_OrganizationUnitUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationUnitUsers",
                table: "OrganizationUnitUsers",
                columns: new[] { "OrganizationUnitId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUnitUsers_OrganizationUnits_OrganizationUnitId",
                table: "OrganizationUnitUsers",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUnitUsers_Users_UserId",
                table: "OrganizationUnitUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUnitUsers_OrganizationUnits_OrganizationUnitId",
                table: "OrganizationUnitUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUnitUsers_Users_UserId",
                table: "OrganizationUnitUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationUnitUsers",
                table: "OrganizationUnitUsers");

            migrationBuilder.RenameTable(
                name: "OrganizationUnitUsers",
                newName: "OrganizationUnitUser");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationUnitUsers_UserId",
                table: "OrganizationUnitUser",
                newName: "IX_OrganizationUnitUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationUnitUser",
                table: "OrganizationUnitUser",
                columns: new[] { "OrganizationUnitId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUnitUser_OrganizationUnits_OrganizationUnitId",
                table: "OrganizationUnitUser",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUnitUser_Users_UserId",
                table: "OrganizationUnitUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
