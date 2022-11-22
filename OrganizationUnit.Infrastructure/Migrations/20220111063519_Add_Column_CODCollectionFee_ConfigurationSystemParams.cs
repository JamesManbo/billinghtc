using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Add_Column_CODCollectionFee_ConfigurationSystemParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CODCollectionFee",
                table: "ConfigurationSystemParameters",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ConfigurationSystemParameters",
                keyColumn: "Id",
                keyValue: 1,
                column: "CODCollectionFee",
                value: 5500m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CODCollectionFee",
                table: "ConfigurationSystemParameters");
        }
    }
}
