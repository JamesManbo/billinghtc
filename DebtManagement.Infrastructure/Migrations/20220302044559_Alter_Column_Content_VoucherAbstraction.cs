using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Alter_Column_Content_VoucherAbstraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ReceiptVouchers",
                maxLength: 6000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000) CHARACTER SET utf8mb4",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PaymentVouchers",
                maxLength: 6000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000) CHARACTER SET utf8mb4",
                oldMaxLength: 1000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ReceiptVouchers",
                type: "varchar(1000) CHARACTER SET utf8mb4",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PaymentVouchers",
                type: "varchar(1000) CHARACTER SET utf8mb4",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6000,
                oldNullable: true);
        }
    }
}
