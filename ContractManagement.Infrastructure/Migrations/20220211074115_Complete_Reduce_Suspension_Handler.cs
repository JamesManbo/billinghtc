using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Complete_Reduce_Suspension_Handler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmountBeforeTax",
                table: "ServicePackageSuspensionTimes",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmountBeforeTax",
                table: "ServicePackageSuspensionTimes",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "ServicePackageSuspensionTimes",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionServicePackages_OutContractId",
                table: "TransactionServicePackages",
                column: "OutContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionServicePackages_OutContracts_OutContractId",
                table: "TransactionServicePackages",
                column: "OutContractId",
                principalTable: "OutContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS RestoreOrSuspendServices");
            migrationBuilder.AlterStoredProcedure("RestoreOrSuspendServices_v4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionServicePackages_OutContracts_OutContractId",
                table: "TransactionServicePackages");

            migrationBuilder.DropIndex(
                name: "IX_TransactionServicePackages_OutContractId",
                table: "TransactionServicePackages");

            migrationBuilder.DropColumn(
                name: "DiscountAmountBeforeTax",
                table: "ServicePackageSuspensionTimes");

            migrationBuilder.DropColumn(
                name: "RemainingAmountBeforeTax",
                table: "ServicePackageSuspensionTimes");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "ServicePackageSuspensionTimes");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS RestoreOrSuspendServices");
            migrationBuilder.AlterStoredProcedure("RestoreOrSuspendServices_v3");
        }
    }
}
