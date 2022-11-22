using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Column_CreatedUserFullName_ReceiptVouchers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedUserFullName",
                table: "ReceiptVouchers",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserFullName",
                table: "PaymentVouchers",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUserFullName",
                table: "ReceiptVouchers");

            migrationBuilder.DropColumn(
                name: "CreatedUserFullName",
                table: "PaymentVouchers");
        }
    }
}
