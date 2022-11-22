using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_AccountingCustomerCode_Table_OutContract_And_Incontract_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountingCustomerCode",
                table: "OutContracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountingCustomerCode",
                table: "InContracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountingCustomerCode",
                table: "OutContracts");

            migrationBuilder.DropColumn(
                name: "AccountingCustomerCode",
                table: "InContracts");
        }
    }
}
