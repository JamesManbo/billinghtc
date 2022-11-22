using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Optimize_ContractSharingRevenues_Aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractSharingRevenueLines_ContractSharingRevenues_CsrId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropTable(
                name: "ContractSharingRevenues");

            migrationBuilder.DropIndex(
                name: "IX_ContractSharingRevenueLines_CsrId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "CsrId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "CsrUid",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "PointType",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "PointTypeName",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "ServicePackageId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "ServicePackageName",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "SharedFixedAmount",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "SharedInstallFeePercent",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "SharedPackagePercent",
                table: "ContractSharingRevenueLines");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyUnitCode",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InContractCode",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InContractId",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InContractId1",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InSharedFixedAmount",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "InSharedInstallFeePercent",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "InSharedPackagePercent",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "OutContractId",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutContractId1",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutContractServicePackageId",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OutSharedFixedAmount",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "OutSharedInstallFeePercent",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OutSharedPackagePercent",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0f);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CurrencyUnitCode",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "InContractCode",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "InContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "InContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "InSharedFixedAmount",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "InSharedInstallFeePercent",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "InSharedPackagePercent",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutContractId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutContractId1",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutContractServicePackageId",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutSharedFixedAmount",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutSharedInstallFeePercent",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "OutSharedPackagePercent",
                table: "ContractSharingRevenueLines");

            migrationBuilder.AddColumn<int>(
                name: "CsrId",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CsrUid",
                table: "ContractSharingRevenueLines",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PointType",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PointTypeName",
                table: "ContractSharingRevenueLines",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "ContractSharingRevenueLines",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicePackageId",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServicePackageName",
                table: "ContractSharingRevenueLines",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SharedFixedAmount",
                table: "ContractSharingRevenueLines",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "SharedInstallFeePercent",
                table: "ContractSharingRevenueLines",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SharedPackagePercent",
                table: "ContractSharingRevenueLines",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "ContractSharingRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChannelCid = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ChannelName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ChannelTemporaryId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CostTerm = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Culture = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CurrencyUnitCode = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CurrencyUnitId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    InContractCode = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    InContractId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OrganizationPath = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OutChannelId = table.Column<int>(type: "int", nullable: false),
                    OutContractCode = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    OutContractId = table.Column<int>(type: "int", nullable: true),
                    OutContractServicePackageId = table.Column<int>(type: "int", nullable: true),
                    SharingType = table.Column<int>(type: "int", nullable: false),
                    TaxMoney = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalAmountAfterTax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Uid = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSharingRevenues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenues_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenues_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenues_OutContractServicePackages_OutContra~",
                        column: x => x.OutContractServicePackageId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_CsrId",
                table: "ContractSharingRevenueLines",
                column: "CsrId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenues_InContractId",
                table: "ContractSharingRevenues",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenues_OutContractId",
                table: "ContractSharingRevenues",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenues_OutContractServicePackageId",
                table: "ContractSharingRevenues",
                column: "OutContractServicePackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractSharingRevenueLines_ContractSharingRevenues_CsrId",
                table: "ContractSharingRevenueLines",
                column: "CsrId",
                principalTable: "ContractSharingRevenues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
