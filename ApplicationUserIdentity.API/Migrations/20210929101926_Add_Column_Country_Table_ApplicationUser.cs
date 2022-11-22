using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Add_Column_Country_Table_ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryIdentityGuid",
                table: "ApplicationUsers",
                maxLength: 68,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CountryIdentityGuid",
                table: "ApplicationUsers");
        }
    }
}
