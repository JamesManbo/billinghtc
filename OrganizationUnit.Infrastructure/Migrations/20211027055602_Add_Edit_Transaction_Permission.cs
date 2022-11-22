using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Add_Edit_Transaction_Permission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "PermissionCode", "PermissionName", "PermissionPage", "PermissionSetId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 443, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_TRANSACTION", "Chỉnh sửa giao dịch/phụ lục trước khi triển khai", "/transaction-management", 0, "", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 443);
        }
    }
}
