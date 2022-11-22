using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_NonProject_Customer_Revenue_Report : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalRevenueEnterpiseReport");
            migrationBuilder.AlterStoredProcedure("GetTotalRevenueEnterpiseReport_v6");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetTotalFTTHProjectRevenue_v7");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSummaryFTTHProjectRevenue");
            migrationBuilder.AlterStoredProcedure("GetSummaryFTTHProjectRevenue_v1");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSummaryPersonalCustomerRevenue");
            migrationBuilder.AlterStoredProcedure("GetSummaryPersonalCustomerRevenue");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTransactionOfOutContract");
            migrationBuilder.AlterStoredProcedure("GetTransactionOfOutContract_v2");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTotalDebtReport");
            migrationBuilder.AlterStoredProcedure("GetTotalDebtReport_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
