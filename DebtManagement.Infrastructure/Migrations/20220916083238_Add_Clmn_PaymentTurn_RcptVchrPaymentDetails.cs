using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Clmn_PaymentTurn_RcptVchrPaymentDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentTurn",
                table: "ReceiptVoucherPaymentDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentTurn",
                table: "ReceiptVoucherPaymentDetails");
        }
    }
}
