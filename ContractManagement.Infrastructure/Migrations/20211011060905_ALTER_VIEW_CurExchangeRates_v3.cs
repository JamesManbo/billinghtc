﻿using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class ALTER_VIEW_CurExchangeRates_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS CurExchangeRates");
            migrationBuilder.AlterStoredProcedure("curexchangerates_v3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS CurExchangeRates");
            migrationBuilder.AlterStoredProcedure("curexchangerates_v2");
        }
    }
}
