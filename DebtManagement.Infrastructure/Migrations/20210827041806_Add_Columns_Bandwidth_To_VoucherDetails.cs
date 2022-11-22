using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Columns_Bandwidth_To_VoucherDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomesticBandwidth",
                table: "ReceiptVoucherDetails",
                maxLength: 68,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternationalBandwidth",
                table: "ReceiptVoucherDetails",
                maxLength: 68,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomesticBandwidth",
                table: "PaymentVoucherDetails",
                maxLength: 68,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasDistinguishBandwidth",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasStartAndEndPoint",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InternationalBandwidth",
                table: "PaymentVoucherDetails",
                maxLength: 68,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomesticBandwidth",
                table: "ReceiptVoucherDetails");

            migrationBuilder.DropColumn(
                name: "InternationalBandwidth",
                table: "ReceiptVoucherDetails");

            migrationBuilder.DropColumn(
                name: "DomesticBandwidth",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "HasDistinguishBandwidth",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "HasStartAndEndPoint",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "InternationalBandwidth",
                table: "PaymentVoucherDetails");
        }
    }
}
