using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Add_Column_BankName_BankBranch_ConfigSystemParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankBranch",
                table: "ConfigurationSystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "ConfigurationSystemParameters",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ConfigurationSystemParameters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BankBranch", "BankName" },
                values: new object[] { "Hội sở chính", "Maritimebank" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankBranch",
                table: "ConfigurationSystemParameters");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "ConfigurationSystemParameters");
        }
    }
}
