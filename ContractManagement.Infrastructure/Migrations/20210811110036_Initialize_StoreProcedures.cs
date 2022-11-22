using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Initialize_StoreProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET GLOBAL log_bin_trust_function_creators = 1;");
            migrationBuilder.AlterStoredProcedure("AddServiceChannelToInContract_v2");
            migrationBuilder.AlterStoredProcedure("ChangeEquipments");
            migrationBuilder.AlterStoredProcedure("ChangeLocationServices");
            migrationBuilder.AlterStoredProcedure("curexchangerates");
            migrationBuilder.AlterStoredProcedure("DeleteContractSharingRevenuesByIds");
            migrationBuilder.AlterStoredProcedure("Func_CalculateSubspendServiceTotal");
            migrationBuilder.AlterStoredProcedure("func_ExchangeMoney");
            migrationBuilder.AlterStoredProcedure("GetContractorInProjectOfOutContract_v2");
            migrationBuilder.AlterStoredProcedure("GetCustomerInMarketArea");
            migrationBuilder.AlterStoredProcedure("GetEquipmentInProjectOfOutContract");
            migrationBuilder.AlterStoredProcedure("GetExpiredOutContract_v2");
            migrationBuilder.AlterStoredProcedure("GetInContractTotalQuantityEveryMonth");
            migrationBuilder.AlterStoredProcedure("GetListMasterCustomerReport_v8");
            migrationBuilder.AlterStoredProcedure("GetNeedToPaymentContractServices_v3");
            migrationBuilder.AlterStoredProcedure("GetOutContractEffectedQuantityEveryMonth");
            migrationBuilder.AlterStoredProcedure("GetOutContractSharingRevenueByInContract_v3");
            migrationBuilder.AlterStoredProcedure("GetOutContractSignedQuantityEveryMonth");
            migrationBuilder.AlterStoredProcedure("GetOutContractSimpleAllByIds");
            migrationBuilder.AlterStoredProcedure("GetOutContractSimpleAllByInContractId_v3");
            migrationBuilder.AlterStoredProcedure("GetOutContractTotalQuantityEveryMonth");
            migrationBuilder.AlterStoredProcedure("MapContractSharingRevenueLineToHead");
            migrationBuilder.AlterStoredProcedure("ReclaimEquipments");
            migrationBuilder.AlterStoredProcedure("RestoreOrSuspendServices");
            migrationBuilder.AlterStoredProcedure("SplitReturnTemp");
            migrationBuilder.AlterStoredProcedure("sp_DeleteServicePackage");
            migrationBuilder.AlterStoredProcedure("sp_GetDataTotalRevenue");
            migrationBuilder.AlterStoredProcedure("sp_GetReportEstimateRevenue_v5");
            migrationBuilder.AlterStoredProcedure("sp_GetReportIncreasementOutContract_v5");
            migrationBuilder.AlterStoredProcedure("sp_UpdatePromotionProduct");
            migrationBuilder.AlterStoredProcedure("TerminateContracts");
            migrationBuilder.AlterStoredProcedure("TerminateServices");
            migrationBuilder.AlterStoredProcedure("UpdateNextBillingDateOfPayingContracts_v2");
            migrationBuilder.AlterStoredProcedure("UpgradeEquipments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
