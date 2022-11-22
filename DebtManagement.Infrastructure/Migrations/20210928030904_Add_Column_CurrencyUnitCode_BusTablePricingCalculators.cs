using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Column_CurrencyUnitCode_BusTablePricingCalculators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyUnitCode",
                table: "BusTablePricingCalculators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyUnitCode",
                table: "BusTablePricingCalculators");
        }
    }
}
