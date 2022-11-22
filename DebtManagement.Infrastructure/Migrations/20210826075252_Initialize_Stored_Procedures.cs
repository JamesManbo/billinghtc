using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Initialize_Stored_Procedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterStoredProcedure("ApprovedClearingIdInVoucher");
            migrationBuilder.AlterStoredProcedure("CancelledClearingIdInVoucher");
            migrationBuilder.AlterStoredProcedure("ClearingSuccessVoucher");
            migrationBuilder.AlterStoredProcedure("GetAllVoucherTargetIds");
            migrationBuilder.AlterStoredProcedure("GetReceiptVoucherNumberByIssuedDate");
            migrationBuilder.AlterStoredProcedure("GetRevenueAndTaxAmountInYear");
            migrationBuilder.AlterStoredProcedure("JoinGeneratedVoucherCategories");
            migrationBuilder.AlterStoredProcedure("LockVoucher");
            migrationBuilder.AlterStoredProcedure("PaymentVouchersUpdateOverdue");
            migrationBuilder.AlterStoredProcedure("ReceiptVouchersUpdateOverdueAndBadDebt");
            migrationBuilder.AlterStoredProcedure("SplitReturnTemp");
            migrationBuilder.AlterStoredProcedure("sp_GetCollectedAndUnCollectedVoucherByMonth");
            migrationBuilder.AlterStoredProcedure("sp_GetDailyRevenueByService");
            migrationBuilder.AlterStoredProcedure("sp_GetDailyRevenueByServiceGroup");
            migrationBuilder.AlterStoredProcedure("sp_GetReportCustomer_v2");
            migrationBuilder.AlterStoredProcedure("sp_GetReportIncreasementOutContract");
            migrationBuilder.AlterStoredProcedure("sp_GetReportOutContract_v2");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceDebtData_v2");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceOutDebtData");
            migrationBuilder.AlterStoredProcedure("SP_UpdateApplicationUserClass");
            migrationBuilder.AlterStoredProcedure("UpdateCurrentDebtOfTarget");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
