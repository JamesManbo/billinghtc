using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Update_Add_Columns_Bank_Table_ApplicationUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "ApplicationUsers",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankBranch",
                table: "ApplicationUsers",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "ApplicationUsers",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "BankBranch",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "ApplicationUsers");
        }
    }
}
