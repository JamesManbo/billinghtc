using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Resolve_Commission_Revenue_InContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutSe~",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropTable(
                name: "InContractServices");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InContractServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Culture = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    InContractId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OrganizationPath = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PointType = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ShareType = table.Column<int>(type: "int", nullable: false),
                    SharedInstallFeePercent = table.Column<float>(type: "float", nullable: false),
                    SharedPackagePercent = table.Column<float>(type: "float", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InContractServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InContractServices_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines",
                column: "OutServiceChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_InContractServices_InContractId",
                table: "InContractServices",
                column: "InContractId");

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
