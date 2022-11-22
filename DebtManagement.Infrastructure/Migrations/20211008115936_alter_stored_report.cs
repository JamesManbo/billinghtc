﻿using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class alter_stored_report : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceDebtData_v6");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceOutDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceOutDebtData_v4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceDebtData_5");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetReportServiceOutDebtData");
            migrationBuilder.AlterStoredProcedure("sp_GetReportServiceOutDebtData_v3");
        }
    }
}
