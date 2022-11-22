using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Alter_Users_Allow_Belong_To_Multiple_Org : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationUnitUser",
                columns: table => new
                {
                    OrganizationUnitId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUnitUser", x => new { x.OrganizationUnitId, x.UserId });
                    table.ForeignKey(
                        name: "FK_OrganizationUnitUser_OrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "OrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationUnitUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitUser_UserId",
                table: "OrganizationUnitUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationUnitUser");
        }
    }
}
