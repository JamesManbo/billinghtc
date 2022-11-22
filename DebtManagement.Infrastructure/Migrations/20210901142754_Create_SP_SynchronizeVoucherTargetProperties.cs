using DebtManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Create_SP_SynchronizeVoucherTargetProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VoucherTargetProperties_TargetId",
                table: "VoucherTargetProperties",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherTargetProperties_VoucherTargets_TargetId",
                table: "VoucherTargetProperties",
                column: "TargetId",
                principalTable: "VoucherTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SynchronizeVoucherTargetProperties");
            migrationBuilder.AlterStoredProcedure("SynchronizeVoucherTargetProperties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherTargetProperties_VoucherTargets_TargetId",
                table: "VoucherTargetProperties");

            migrationBuilder.DropIndex(
                name: "IX_VoucherTargetProperties_TargetId",
                table: "VoucherTargetProperties");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SynchronizeVoucherTargetProperties");
        }
    }
}
