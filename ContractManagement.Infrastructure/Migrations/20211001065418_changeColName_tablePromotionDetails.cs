using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class changeColName_tablePromotionDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "PromotionDetails");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "PromotionDetails");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "PromotionDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "PromotionDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "PromotionDetails");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "PromotionDetails");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "PromotionDetails",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AreaId",
                table: "PromotionDetails",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
