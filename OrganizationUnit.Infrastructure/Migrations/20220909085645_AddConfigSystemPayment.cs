using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class AddConfigSystemPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentBankBranch",
                table: "ConfigurationSystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentBankName",
                table: "ConfigurationSystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentBankNumber",
                table: "ConfigurationSystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentCompany",
                table: "ConfigurationSystemParameters",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ConfigurationSystemParameters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PaymentBankBranch", "PaymentBankName", "PaymentBankNumber", "PaymentCompany" },
                values: new object[] { "", "", "", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentBankBranch",
                table: "ConfigurationSystemParameters");

            migrationBuilder.DropColumn(
                name: "PaymentBankName",
                table: "ConfigurationSystemParameters");

            migrationBuilder.DropColumn(
                name: "PaymentBankNumber",
                table: "ConfigurationSystemParameters");

            migrationBuilder.DropColumn(
                name: "PaymentCompany",
                table: "ConfigurationSystemParameters");
        }
    }
}
