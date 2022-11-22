using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Columns_For_Renew_Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ContractExpiredDate",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractNewExpirationDate",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ContractRenewMonths",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RenewFee",
                table: "Transactions",
                nullable: true);

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Name", "Permission" },
                values: new object[] { 13, "Gia hạn hợp đồng", "RENEW_CONTRACT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DropColumn(
                name: "ContractExpiredDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ContractNewExpirationDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ContractRenewMonths",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RenewFee",
                table: "Transactions");
        }
    }
}
