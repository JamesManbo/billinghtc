using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_TimeLine_LatestBilling_Contracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeLine_LatestBilling",
                table: "OutContracts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeLine_LatestBilling",
                table: "InContracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeLine_LatestBilling",
                table: "OutContracts");

            migrationBuilder.DropColumn(
                name: "TimeLine_LatestBilling",
                table: "InContracts");
        }
    }
}
