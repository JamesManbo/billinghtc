using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Add_View_Transaction_By_Department_Permissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23,
                column: "PermissionName",
                value: "Xem tất cả phụ lục");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "PermissionCode", "PermissionName", "PermissionPage", "PermissionSetId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 442, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_TRANSACTION_OF_SERVICE_SUPPLIER", "Xem phụ lục của phòng CCDV", "/transaction-management", 0, "", null });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "PermissionCode", "PermissionName", "PermissionPage", "PermissionSetId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 441, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_TRANSACTION_OF_SUPPORTER", "Xem phụ lục của phòng Kỹ Thuật", "/transaction-management", 0, "", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 441);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 442);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23,
                column: "PermissionName",
                value: "Chi tiết phụ lục");
        }
    }
}
