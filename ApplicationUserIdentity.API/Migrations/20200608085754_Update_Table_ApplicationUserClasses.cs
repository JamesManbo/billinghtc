using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Update_Table_ApplicationUserClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConditionEndPoint",
                table: "ApplicationUserClasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConditionStartPoint",
                table: "ApplicationUserClasses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConditionEndPoint",
                table: "ApplicationUserClasses");

            migrationBuilder.DropColumn(
                name: "ConditionStartPoint",
                table: "ApplicationUserClasses");
        }
    }
}
