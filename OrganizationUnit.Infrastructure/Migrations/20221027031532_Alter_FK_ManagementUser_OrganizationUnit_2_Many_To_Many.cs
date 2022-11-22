using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Alter_FK_ManagementUser_OrganizationUnit_2_Many_To_Many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUnits_Users_ManagementUserId",
                table: "OrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_OrganizationUnits_OrganizationUnitId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationUnitId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUnits_ManagementUserId",
                table: "OrganizationUnits");

            migrationBuilder.DropColumn(
                name: "OrganizationUnitId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ManagementUserId",
                table: "OrganizationUnits");

            migrationBuilder.AddColumn<int>(
                name: "PositionLevel",
                table: "OrganizationUnitUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionLevel",
                table: "OrganizationUnitUsers");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationUnitId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagementUserId",
                table: "OrganizationUnits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationUnitId",
                table: "Users",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnits_ManagementUserId",
                table: "OrganizationUnits",
                column: "ManagementUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUnits_Users_ManagementUserId",
                table: "OrganizationUnits",
                column: "ManagementUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OrganizationUnits_OrganizationUnitId",
                table: "Users",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
