using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_ConnectionPoint_And_Country_Table_DeploymentChannelPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConnectionPoint",
                table: "TransactionChannelPoints",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InstallationAddress_Country",
                table: "TransactionChannelPoints",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationAddress_CountryId",
                table: "TransactionChannelPoints",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConnectionPoint",
                table: "OutputChannelPoints",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InstallationAddress_Country",
                table: "OutputChannelPoints",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationAddress_CountryId",
                table: "OutputChannelPoints",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionPoint",
                table: "TransactionChannelPoints");

            migrationBuilder.DropColumn(
                name: "InstallationAddress_Country",
                table: "TransactionChannelPoints");

            migrationBuilder.DropColumn(
                name: "InstallationAddress_CountryId",
                table: "TransactionChannelPoints");

            migrationBuilder.DropColumn(
                name: "ConnectionPoint",
                table: "OutputChannelPoints");

            migrationBuilder.DropColumn(
                name: "InstallationAddress_Country",
                table: "OutputChannelPoints");

            migrationBuilder.DropColumn(
                name: "InstallationAddress_CountryId",
                table: "OutputChannelPoints");
        }
    }
}
