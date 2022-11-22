using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Add_Column_AccountingCustomerCode_To_ApplicationUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountingCustomerCode",
                table: "ApplicationUsers",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountingCustomerCode",
                table: "ApplicationUsers");
        }
    }
}
