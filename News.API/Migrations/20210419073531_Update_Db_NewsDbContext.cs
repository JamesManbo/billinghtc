using Microsoft.EntityFrameworkCore.Migrations;

namespace News.API.Migrations
{
    public partial class Update_Db_NewsDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "ArticleCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "ArticleArticleCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "ArticleCategories");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "ArticleArticleCategories");
        }
    }
}
