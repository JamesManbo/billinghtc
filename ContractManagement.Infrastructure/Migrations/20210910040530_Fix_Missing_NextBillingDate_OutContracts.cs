using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Fix_Missing_NextBillingDate_OutContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeLine_NextBillingDate",
                table: "OutContracts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeLine_StartBillingDate",
                table: "OutContracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeLine_NextBillingDate",
                table: "OutContracts");

            migrationBuilder.DropColumn(
                name: "TimeLine_StartBillingDate",
                table: "OutContracts");
        }
    }
}
