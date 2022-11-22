using Microsoft.EntityFrameworkCore.Migrations;

namespace News.API.Migrations
{
    public partial class Update_table_Article_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Articles",
                type: "NVARCHAR(2000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Articles",
                type: "NVARCHAR(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Articles");
        }
    }
}
