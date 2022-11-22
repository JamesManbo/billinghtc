using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_ChannelIndex_OutContractServicePackages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelIndex",
                table: "TransactionServicePackages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChannelIndex",
                table: "OutContractServicePackages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelIndex",
                table: "TransactionServicePackages");

            migrationBuilder.DropColumn(
                name: "ChannelIndex",
                table: "OutContractServicePackages");
        }
    }
}
