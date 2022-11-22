using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_InvoicingAddress_To_Contracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoicingAddress",
                table: "OutContracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoicingAddress",
                table: "InContracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoicingAddress",
                table: "OutContracts");

            migrationBuilder.DropColumn(
                name: "InvoicingAddress",
                table: "InContracts");
        }
    }
}
