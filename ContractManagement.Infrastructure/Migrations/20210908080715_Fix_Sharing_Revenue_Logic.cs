using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Fix_Sharing_Revenue_Logic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "ContractSharingRevenueLines");

            migrationBuilder.AlterColumn<int>(
                name: "InContractId",
                table: "ContractSharingRevenueLines",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "SharedTotalAmount",
                table: "ContractSharingRevenueLines",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SharingDuration",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SharingFrom",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SharingTo",
                table: "ContractSharingRevenueLines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uid",
                table: "ContractSharingRevenueLines",
                maxLength: 68,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SharingRevenueLineDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    SharingLineUid = table.Column<string>(maxLength: 68, nullable: true),
                    SharingLineId = table.Column<int>(nullable: true),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    SharingType = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    SharingAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharingRevenueLineDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharingRevenueLineDetails_ContractSharingRevenueLines_Sharin~",
                        column: x => x.SharingLineId,
                        principalTable: "ContractSharingRevenueLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharingRevenueLineDetails_SharingLineId",
                table: "SharingRevenueLineDetails",
                column: "SharingLineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharingRevenueLineDetails");

            migrationBuilder.DropColumn(
                name: "SharedTotalAmount",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "SharingDuration",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "SharingFrom",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "SharingTo",
                table: "ContractSharingRevenueLines");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "ContractSharingRevenueLines");

            migrationBuilder.AlterColumn<int>(
                name: "InContractId",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "ContractSharingRevenueLines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
