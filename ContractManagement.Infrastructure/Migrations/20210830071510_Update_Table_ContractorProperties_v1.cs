using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Update_Table_ContractorProperties_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractorCategoryName",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractorClassId",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractorClassName",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractorGroupNames",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractorIndustryId",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractorIndustryName",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractorStructureName",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractorTypeId",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractorTypeName",
                table: "ContractorProperties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractorCategoryName",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorClassId",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorClassName",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorGroupNames",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorIndustryId",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorIndustryName",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorStructureName",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorTypeId",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorTypeName",
                table: "ContractorProperties");
        }
    }
}
