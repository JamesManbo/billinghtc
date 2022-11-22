using ContractManagement.Domain.AggregatesModel.BaseContract;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Remove_Redundant_Contract_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"UPDATE OutContracts SET ContractStatusId = {ContractStatus.Signed.Id} WHERE ContractStatusId = 3 OR ContractStatusId = 4;");
            migrationBuilder.Sql($"UPDATE OutContracts SET ContractStatusId = {ContractStatus.Draft.Id} WHERE ContractStatusId = 6;");

            migrationBuilder.Sql($"UPDATE InContracts SET ContractStatusId = {ContractStatus.Signed.Id} WHERE ContractStatusId = 3 OR ContractStatusId = 4;");
            migrationBuilder.Sql($"UPDATE InContracts SET ContractStatusId = {ContractStatus.Draft.Id} WHERE ContractStatusId = 6;");

            migrationBuilder.DeleteData(
                table: "ContractStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ContractStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ContractStatus",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ContractStatus",
                keyColumn: "Id",
                keyValue: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Đã nghiệm thu" },
                    { 4, "Tạm ngưng" },
                    { 6, "Trình ký" },
                    { 10, "Khác" }
                });
        }
    }
}
