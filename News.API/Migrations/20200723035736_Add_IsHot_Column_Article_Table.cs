using Microsoft.EntityFrameworkCore.Migrations;

namespace News.API.Migrations
{
    public partial class Add_IsHot_Column_Article_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHot",
                table: "Articles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHot",
                table: "Articles");
        }
    }
}
