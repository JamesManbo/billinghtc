using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Fix_FK_ContractSharingRevenues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_InContracts_InContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_InContracts_InContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_InSer~",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContracts_OutContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContracts_OutContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutCo~",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutSe~",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_InContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_InContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_InServiceChannelId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutContractServicePackageId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "InContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutContractServicePackageId",
                table: "ContractSharingRevenueLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InContractId1",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutContractId1",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutContractServicePackageId",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_InContractId",
                table: "ContractSharingRevenueLines",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_InContractId1",
                table: "ContractSharingRevenueLines",
                column: "InContractId1");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_InServiceChannelId",
                table: "ContractSharingRevenueLines",
                column: "InServiceChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutContractId",
                table: "ContractSharingRevenueLines",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutContractId1",
                table: "ContractSharingRevenueLines",
                column: "OutContractId1");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutContractServicePackageId",
                table: "ContractSharingRevenueLines",
                column: "OutContractServicePackageId");

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
                name: "FK_ContractSharingRevenueLines_InContracts_InContractId1",
                table: "ContractSharingRevenueLines",
                column: "InContractId1",
                principalTable: "InContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_InSer~",
                table: "ContractSharingRevenueLines",
                column: "InServiceChannelId",
                principalTable: "OutContractServicePackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContracts_OutContractId",
                table: "ContractSharingRevenueLines",
                column: "OutContractId",
                principalTable: "OutContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContracts_OutContractId1",
                table: "ContractSharingRevenueLines",
                column: "OutContractId1",
                principalTable: "OutContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutCo~",
                table: "ContractSharingRevenueLines",
                column: "OutContractServicePackageId",
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
    }
}
