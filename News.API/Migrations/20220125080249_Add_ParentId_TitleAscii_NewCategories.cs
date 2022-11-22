using Microsoft.EntityFrameworkCore.Migrations;

namespace News.API.Migrations
{
    public partial class Add_ParentId_TitleAscii_NewCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleAscii",
                table: "Articles",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ArticleCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreePath",
                table: "ArticleCategories",
                maxLength: 4000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleAscii",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ArticleCategories");

            migrationBuilder.DropColumn(
                name: "TreePath",
                table: "ArticleCategories");
        }
    }
}
