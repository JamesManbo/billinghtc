using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class AddColumn_OtherNote_Table_OutContractServicePackages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherNote",
                table: "TransactionServicePackages",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherNote",
                table: "OutContractServicePackages",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherNote",
                table: "TransactionServicePackages");

            migrationBuilder.DropColumn(
                name: "OtherNote",
                table: "OutContractServicePackages");
        }
    }
}
