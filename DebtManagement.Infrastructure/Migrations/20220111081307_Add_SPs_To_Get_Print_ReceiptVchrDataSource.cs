using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Add_SPs_To_Get_Print_ReceiptVchrDataSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetReceiptVoucherPrintData;");
            migrationBuilder.AlterStoredProcedure("GetReceiptVoucherPrintData");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOpeningDebts;");
            migrationBuilder.AlterStoredProcedure("GetOpeningDebts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetReceiptVoucherPrintData;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOpeningDebts;");
        }
    }
}
