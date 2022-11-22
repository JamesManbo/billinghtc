using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class CreateView_curexchangerates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET GLOBAL log_bin_trust_function_creators = 1;");
            
            migrationBuilder.Sql("DROP VIEW IF EXISTS CurExchangeRates;");
            migrationBuilder.AlterStoredProcedure("curexchangerates_v2");

            migrationBuilder.Sql("DROP FUNCTION IF EXISTS ExchangeMoney;");            
            migrationBuilder.AlterStoredProcedure("func_ExchangeMoney_v2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS CurExchangeRates;");
            migrationBuilder.AlterStoredProcedure("curexchangerates");

            migrationBuilder.Sql("DROP FUNCTION IF EXISTS ExchangeMoney;");            
            migrationBuilder.AlterStoredProcedure("func_ExchangeMoney");
        }
    }
}
