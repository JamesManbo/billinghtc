using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Ref_ContractAggregateId_To_TransactionAggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractSlaId",
                table: "TransactionSLAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractPbtId",
                table: "TransactionPriceBusTables",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractChannelTaxId",
                table: "TransactionChannelTaxes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractPointId",
                table: "TransactionChannelPoints",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractSlaId",
                table: "TransactionSLAs");

            migrationBuilder.DropColumn(
                name: "ContractPbtId",
                table: "TransactionPriceBusTables");

            migrationBuilder.DropColumn(
                name: "ContractChannelTaxId",
                table: "TransactionChannelTaxes");

            migrationBuilder.DropColumn(
                name: "ContractPointId",
                table: "TransactionChannelPoints");
        }
    }
}
