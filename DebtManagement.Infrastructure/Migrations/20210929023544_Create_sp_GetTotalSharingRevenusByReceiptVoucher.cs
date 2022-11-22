using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Create_sp_GetTotalSharingRevenusByReceiptVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTotalSharingRevenusByReceiptVoucher");
            migrationBuilder.AlterStoredProcedure("sp_GetTotalSharingRevenusByReceiptVoucher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTotalSharingRevenusByReceiptVoucher");           
        }
    }
}
