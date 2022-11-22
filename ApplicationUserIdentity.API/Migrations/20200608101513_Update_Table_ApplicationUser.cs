using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Update_Table_ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerReviews",
                table: "ApplicationUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerReviews",
                table: "ApplicationUsers");
        }
    }
}
