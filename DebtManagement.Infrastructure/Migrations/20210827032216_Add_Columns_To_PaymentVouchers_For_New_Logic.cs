using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Columns_To_PaymentVouchers_For_New_Logic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHasPrice",
                table: "ReceiptVoucherDetails");

            migrationBuilder.DropColumn(
                name: "ServiceChannelId",
                table: "PaymentVoucherDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartBillingDate",
                table: "PaymentVoucherDetails",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndBillingDate",
                table: "PaymentVoucherDetails",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "CId",
                table: "PaymentVoucherDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyUnitCode",
                table: "PaymentVoucherDetails",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyUnitId",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EquipmentTotalAmount",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrandTotalBeforeTax",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InstallationFee",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstDetailOfService",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "OffsetUpgradePackageAmount",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "OutContractServicePackageId",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PackagePrice",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PaymentVoucherId1",
                table: "PaymentVoucherDetails",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReductionFee",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotalBeforeTax",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "TaxPercent",
                table: "PaymentVoucherDetails",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "PaymentVoucherLineTaxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    VoucherLineId = table.Column<int>(nullable: true),
                    TaxName = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    TaxValue = table.Column<float>(nullable: false),
                    IsAutomaticGenerate = table.Column<bool>(nullable: false),
                    PaymentVoucherLineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVoucherLineTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentVoucherLineTaxes_PaymentVoucherDetails_PaymentVoucher~",
                        column: x => x.PaymentVoucherLineId,
                        principalTable: "PaymentVoucherDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherDetails_PaymentVoucherId1",
                table: "PaymentVoucherDetails",
                column: "PaymentVoucherId1");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherLineTaxes_PaymentVoucherLineId",
                table: "PaymentVoucherLineTaxes",
                column: "PaymentVoucherLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentVoucherDetails_PaymentVouchers_PaymentVoucherId1",
                table: "PaymentVoucherDetails",
                column: "PaymentVoucherId1",
                principalTable: "PaymentVouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVoucherDetails_PaymentVouchers_PaymentVoucherId1",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropTable(
                name: "PaymentVoucherLineTaxes");

            migrationBuilder.DropIndex(
                name: "IX_PaymentVoucherDetails_PaymentVoucherId1",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "CId",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyUnitCode",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyUnitId",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "EquipmentTotalAmount",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "GrandTotalBeforeTax",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "InstallationFee",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "IsFirstDetailOfService",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "OffsetUpgradePackageAmount",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "OutContractServicePackageId",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "PackagePrice",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "PaymentVoucherId1",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "ReductionFee",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "SubTotalBeforeTax",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "TaxPercent",
                table: "PaymentVoucherDetails");

            migrationBuilder.AddColumn<bool>(
                name: "IsHasPrice",
                table: "ReceiptVoucherDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartBillingDate",
                table: "PaymentVoucherDetails",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndBillingDate",
                table: "PaymentVoucherDetails",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceChannelId",
                table: "PaymentVoucherDetails",
                type: "int",
                nullable: true);
        }
    }
}
