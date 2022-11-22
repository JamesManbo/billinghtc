using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_Column_PromotionForVoucherId_TemporaryGeneratingVouchers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PromotionForVoucherId",
                table: "TemporaryGeneratingVouchers",
                nullable: true);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS JoinGeneratedVoucherCategories");
            migrationBuilder.AlterStoredProcedure("JoinGeneratedVoucherCategories_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionForVoucherId",
                table: "TemporaryGeneratingVouchers");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS JoinGeneratedVoucherCategories");
            migrationBuilder.AlterStoredProcedure("JoinGeneratedVoucherCategories");
        }
    }
}
