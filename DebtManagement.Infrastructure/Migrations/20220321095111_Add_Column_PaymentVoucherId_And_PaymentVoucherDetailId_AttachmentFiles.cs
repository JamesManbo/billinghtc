using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Column_PaymentVoucherId_And_PaymentVoucherDetailId_AttachmentFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentVoucherDetailId",
                table: "AttachmentFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentVoucherId",
                table: "AttachmentFiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentVoucherDetailId",
                table: "AttachmentFiles");

            migrationBuilder.DropColumn(
                name: "PaymentVoucherId",
                table: "AttachmentFiles");
        }
    }
}
