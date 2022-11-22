using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Update_Column_ContractorIndustryId_Contractors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractorIndustryId",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorIndustryName",
                table: "ContractorProperties");

            migrationBuilder.AddColumn<string>(
                name: "ContractorIndustryIds",
                table: "ContractorProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractorIndustryNames",
                table: "ContractorProperties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractorIndustryIds",
                table: "ContractorProperties");

            migrationBuilder.DropColumn(
                name: "ContractorIndustryNames",
                table: "ContractorProperties");

            migrationBuilder.AddColumn<int>(
                name: "ContractorIndustryId",
                table: "ContractorProperties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractorIndustryName",
                table: "ContractorProperties",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
