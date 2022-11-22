using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class ReAdd_FK_InContracts_ContractSharingRevenues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_InServiceChannelId",
                table: "ContractSharingRevenueLines",
                column: "InServiceChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines",
                column: "OutServiceChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_InSer~",
                table: "ContractSharingRevenueLines",
                column: "InServiceChannelId",
                principalTable: "OutContractServicePackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_InSer~",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutSe~",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_InServiceChannelId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines");
        }
    }
}
