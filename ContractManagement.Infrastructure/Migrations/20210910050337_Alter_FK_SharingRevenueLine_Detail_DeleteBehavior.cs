using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_FK_SharingRevenueLine_Detail_DeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharingRevenueLineDetails_ContractSharingRevenueLines_Sharin~",
                table: "SharingRevenueLineDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_SharingRevenueLineDetails_ContractSharingRevenueLines_Sharin~",
                table: "SharingRevenueLineDetails",
                column: "SharingLineId",
                principalTable: "ContractSharingRevenueLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharingRevenueLineDetails_ContractSharingRevenueLines_Sharin~",
                table: "SharingRevenueLineDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_SharingRevenueLineDetails_ContractSharingRevenueLines_Sharin~",
                table: "SharingRevenueLineDetails",
                column: "SharingLineId",
                principalTable: "ContractSharingRevenueLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
