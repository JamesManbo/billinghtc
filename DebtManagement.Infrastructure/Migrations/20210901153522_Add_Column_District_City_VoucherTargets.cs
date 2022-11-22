using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Column_District_City_VoucherTargets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "VoucherTargets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "VoucherTargets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "VoucherTargets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "VoucherTargets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "VoucherTargets");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "VoucherTargets");

            migrationBuilder.DropColumn(
                name: "District",
                table: "VoucherTargets");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "VoucherTargets");
        }
    }
}
