using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_OrganizationUnitCode_Contracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationUnitCode",
                table: "OutContracts",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationUnitCode",
                table: "InContracts",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationUnitCode",
                table: "OutContracts");

            migrationBuilder.DropColumn(
                name: "OrganizationUnitCode",
                table: "InContracts");
        }
    }
}
