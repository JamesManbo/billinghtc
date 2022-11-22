using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_FK_InContracts_Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OutContracts_OutContractId",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "InContractId",
                table: "Transactions",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OutContracts_OutContractId",
                table: "Transactions",
                column: "OutContractId",
                principalTable: "OutContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_InContracts_InContractId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OutContracts_OutContractId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_InContractId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "InContractId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OutContracts_OutContractId",
                table: "Transactions",
                column: "OutContractId",
                principalTable: "OutContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
