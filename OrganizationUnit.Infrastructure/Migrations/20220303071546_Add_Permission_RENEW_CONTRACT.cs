using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Add_Permission_RENEW_CONTRACT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "PermissionCode", "PermissionName", "PermissionPage", "PermissionSetId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { -1, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "RENEW_CONTRACT", "Gia hạn hợp đồng", "/contract-output-management", 0, "", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
