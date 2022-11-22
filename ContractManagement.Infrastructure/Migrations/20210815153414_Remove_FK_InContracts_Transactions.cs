using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Remove_FK_InContracts_Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_InContracts_InContractId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_InContractId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "InContractId",
                table: "Transactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InContractId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InContractId",
                table: "Transactions",
                column: "InContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_InContracts_InContractId",
                table: "Transactions",
                column: "InContractId",
                principalTable: "InContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
