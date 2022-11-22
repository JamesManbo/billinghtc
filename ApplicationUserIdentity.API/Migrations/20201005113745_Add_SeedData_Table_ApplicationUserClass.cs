using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Add_SeedData_Table_ApplicationUserClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationUserClasses",
                columns: new[] { "Id", "ClassCode", "ClassName", "ConditionEndPoint", "ConditionStartPoint", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "COPPER_LEVEL", "Hạng đồng", 499999m, 0m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, null, null },
                    { 2, "SILVER_LEVEL", "Hạng bạc", 999999m, 500000m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, null, null },
                    { 3, "GOLD_LEVEL", "Hạng vàng", 2999999m, 1000000m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, null, null },
                    { 4, "DIAMOND_LEVEL", "Hạng kim cương", 5000000m, 3000000m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUserClasses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApplicationUserClasses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ApplicationUserClasses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ApplicationUserClasses",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
