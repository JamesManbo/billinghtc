using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class add_nextBilling_forIncontratct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeLine_NextBillingDate",
                table: "InContracts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeLine_StartBillingDate",
                table: "InContracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "TimeLine_NextBillingDate",
                table: "InContracts");

            migrationBuilder.DropColumn(
                name: "TimeLine_StartBillingDate",
                table: "InContracts");
        }
    }
}
