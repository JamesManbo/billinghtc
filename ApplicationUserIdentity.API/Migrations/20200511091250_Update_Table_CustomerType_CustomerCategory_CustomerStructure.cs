using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Update_Table_CustomerType_CustomerCategory_CustomerStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CustomerTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CustomerStructure",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerStructure",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CustomerCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "CustomerTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CustomerStructure");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerStructure");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CustomerCategories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerCategories");
        }
    }
}
