using Microsoft.EntityFrameworkCore.Migrations;

namespace News.API.Migrations
{
    public partial class Update_Table_Articles_Column_Content : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Articles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(1000)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Articles",
                type: "NVARCHAR(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
