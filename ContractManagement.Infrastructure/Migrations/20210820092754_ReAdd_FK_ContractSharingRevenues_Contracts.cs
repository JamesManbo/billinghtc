using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class ReAdd_FK_ContractSharingRevenues_Contracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_InContractId",
                table: "ContractSharingRevenueLines",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutContractId",
                table: "ContractSharingRevenueLines",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines",
                column: "OutServiceChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_InContracts_InContractId",
                table: "ContractSharingRevenueLines",
                column: "InContractId",
                principalTable: "InContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContracts_OutContractId",
                table: "ContractSharingRevenueLines",
                column: "OutContractId",
                principalTable: "OutContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutSe~",
                table: "ContractSharingRevenueLines",
                column: "OutServiceChannelId",
                principalTable: "OutContractServicePackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_InContracts_InContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContracts_OutContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutSe~",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_InContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines");
        }
    }
}
