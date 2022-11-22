using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Remove_Redundant_In_Out_ContractStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InContractStatus");

            migrationBuilder.DropTable(
                name: "OutContractStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InContractStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InContractStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutContractStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContractStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InContractStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Trình ký" },
                    { 2, "Hoàn thành" },
                    { 3, "Thanh lý" },
                    { 4, "Hủy" },
                    { 5, "Khác" }
                });

            migrationBuilder.InsertData(
                table: "OutContractStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chờ ký" },
                    { 2, "Đã ký" },
                    { 3, "Đã nghiệm thu" },
                    { 4, "Tạm ngưng" },
                    { 5, "Đã thanh lý(Hủy)" }
                });
        }
    }
}
