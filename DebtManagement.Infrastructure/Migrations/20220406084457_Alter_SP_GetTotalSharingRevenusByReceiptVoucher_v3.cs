using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_GetTotalSharingRevenusByReceiptVoucher_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTotalSharingRevenusByReceiptVoucher");
            migrationBuilder.AlterStoredProcedure("sp_GetTotalSharingRevenusByReceiptVoucher_v3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTotalSharingRevenusByReceiptVoucher");
            migrationBuilder.AlterStoredProcedure("sp_GetTotalSharingRevenusByReceiptVoucher_v2");
        }
    }
}
