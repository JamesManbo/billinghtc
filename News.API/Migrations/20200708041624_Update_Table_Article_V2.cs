using Microsoft.EntityFrameworkCore.Migrations;

namespace News.API.Migrations
{
    public partial class Update_Table_Article_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Articles",
                type: "NVARCHAR(5000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(2000)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawDescription",
                table: "Articles",
                type: "NVARCHAR(5000)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawDescription",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Articles",
                type: "NVARCHAR(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(5000)",
                oldNullable: true);
        }
    }
}
