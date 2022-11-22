using System;
using ContractManagement.Infrastructure.SqlScripts;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Alter_SP_GetNeedToPaymentContractServices_v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 4);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 5);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 8);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 10);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 12);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 14);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 16);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 18);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 2);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 3);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 4);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 5);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 6);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 7);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 8);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 9);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 10);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 11);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 12);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 13);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 14);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 15);

            //migrationBuilder.DeleteData(
            //    table: "Services",
            //    keyColumn: "Id",
            //    keyValue: 16);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 2);

            //migrationBuilder.DeleteData(
            //    table: "ServiceGroups",
            //    keyColumn: "Id",
            //    keyValue: 3);
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetNeedToPaymentContractServices");
            migrationBuilder.AlterStoredProcedure("GetNeedToPaymentContractServices_v5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetNeedToPaymentContractServices");
            migrationBuilder.AlterStoredProcedure("GetNeedToPaymentContractServices_v4");
            //migrationBuilder.InsertData(
            //    table: "ServiceGroups",
            //    columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "GroupCode", "GroupName", "IsActive", "IsDeleted", "OrganizationPath", "UpdatedBy", "UpdatedDate" },
            //    values: new object[,]
            //    {
            //        { 1, "", new DateTime(2020, 9, 9, 2, 11, 35, 964, DateTimeKind.Unspecified).AddTicks(6270), "", 0, "NTD", "Nhóm truyền dẫn", true, false, "", "", null },
            //        { 2, "", new DateTime(1900, 1, 20, 2, 11, 48, 891, DateTimeKind.Unspecified).AddTicks(6470), "", 0, "DVT", "Nhóm dịch vụ thoại IP", true, false, "", "", null },
            //        { 3, "", new DateTime(1900, 1, 20, 2, 12, 0, 105, DateTimeKind.Unspecified).AddTicks(2170), "", 0, "GTGT", "Nhóm dịch vụ GTGT", true, false, "", "", null },
            //        { 4, "", new DateTime(2020, 9, 9, 2, 12, 15, 264, DateTimeKind.Unspecified), "", 0, "CNTTGPTH", "Nhóm các dịch vụ CNTT/ Giải pháp tích hợp", true, false, "", "", new DateTime(1900, 1, 20, 8, 42, 11, 620, DateTimeKind.Unspecified).AddTicks(3180) },
            //        { 5, "", new DateTime(1900, 1, 20, 8, 42, 44, 138, DateTimeKind.Unspecified).AddTicks(1960), "", 0, "DVTT01", "Nhóm dịch vụ TT truyền thông", true, true, "", "", new DateTime(1900, 1, 20, 8, 42, 51, 149, DateTimeKind.Unspecified).AddTicks(4590) },
            //        { 8, "", new DateTime(2020, 10, 19, 8, 28, 34, 323, DateTimeKind.Unspecified).AddTicks(360), "", 0, "DVTD", "Nhóm các dịch vụ truyền dẫn", true, false, "", "", null },
            //        { 10, "", new DateTime(2020, 10, 19, 8, 28, 58, 726, DateTimeKind.Unspecified), "", 0, "NDVT", "Nhóm dịch vụ thoại IP", true, false, "", "", new DateTime(2020, 10, 19, 8, 32, 29, 706, DateTimeKind.Unspecified).AddTicks(1280) },
            //        { 12, "", new DateTime(2020, 10, 19, 7, 43, 30, 728, DateTimeKind.Unspecified).AddTicks(1010), "", 0, "CNTTGPTH", "Nhóm các dịch vụ CNTT/ Giải pháp tích hợp", true, true, "", "", null },
            //        { 14, "", new DateTime(2020, 10, 19, 7, 43, 47, 721, DateTimeKind.Unspecified).AddTicks(4780), "", 0, "GTGT", "Nhóm dịch vụ GTGT", true, true, "", "", null },
            //        { 16, "", new DateTime(2020, 10, 19, 7, 44, 0, 123, DateTimeKind.Unspecified).AddTicks(4350), "", 0, "DVT", "Nhóm dịch vụ thoại IP", true, true, "", "", null },
            //        { 18, "", new DateTime(2020, 10, 19, 8, 30, 9, 26, DateTimeKind.Unspecified).AddTicks(6230), "", 0, "CNTT", "Nhóm dịch vụ CNTT", true, false, "", "", null }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Services",
            //    columns: new[] { "Id", "AvatarId", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "GroupId", "HasCableKilometers", "HasDistinguishBandwidth", "HasLineQuantity", "HasPackages", "HasStartAndEndPoint", "IsActive", "IsDeleted", "OrganizationPath", "ServiceCode", "ServiceName", "ServicePrice", "UpdatedBy", "UpdatedDate" },
            //    values: new object[,]
            //    {
            //        { 1, 0, "", new DateTime(2020, 9, 9, 4, 20, 48, 323, DateTimeKind.Unspecified), "", 0, 1, false, true, false, false, true, true, false, "", "01", "Internet trực tiếp (Internet leased line - ILL)", 0m, "", new DateTime(2020, 9, 9, 6, 47, 0, 966, DateTimeKind.Unspecified).AddTicks(1590) },
            //        { 2, 0, "", new DateTime(2020, 9, 9, 4, 21, 30, 719, DateTimeKind.Unspecified).AddTicks(3380), "", 0, 1, false, false, false, false, false, true, false, "", "02", "Internet bán ra nước ngoài", 0m, "", null },
            //        { 3, 0, "", new DateTime(2020, 9, 9, 4, 27, 55, 793, DateTimeKind.Unspecified), "", 0, 1, false, false, false, false, true, true, false, "", "03", "Kênh thuê riêng trong nước (SDH,)", 0m, "", new DateTime(2020, 9, 15, 2, 34, 44, 515, DateTimeKind.Unspecified).AddTicks(6240) },
            //        { 4, 0, "", new DateTime(2020, 9, 9, 4, 28, 10, 304, DateTimeKind.Unspecified).AddTicks(2800), "", 0, 1, true, false, true, false, false, true, false, "", "04", "Thuê sợi quang", 0m, "", null },
            //        { 5, 0, "", new DateTime(2020, 9, 9, 4, 28, 24, 554, DateTimeKind.Unspecified).AddTicks(490), "", 0, 1, false, false, false, false, false, true, false, "", "05", "Kênh thuê riêng quốc tế (Có điểm kết nối ngoài Việt Nam)", 0m, "", null },
            //        { 6, 0, "", new DateTime(2020, 9, 9, 4, 29, 54, 479, DateTimeKind.Unspecified).AddTicks(610), "", 0, 1, false, false, false, false, false, true, false, "", "06", "VPN/Metro/MPLS trong nước (Các loại IP)", 0m, "", null },
            //        { 7, 0, "", new DateTime(2020, 9, 9, 4, 30, 6, 652, DateTimeKind.Unspecified).AddTicks(6790), "", 0, 1, false, false, false, false, false, true, false, "", "07", "VPN/Metro/MPLS quốc tế ", 0m, "", null },
            //        { 8, 0, "", new DateTime(2020, 9, 9, 4, 30, 25, 66, DateTimeKind.Unspecified).AddTicks(3260), "", 0, 1, false, true, false, true, false, true, false, "", "08", "Internet băng rộng FTTH ", 0m, "", null },
            //        { 9, 0, "", new DateTime(2020, 9, 9, 4, 31, 26, 432, DateTimeKind.Unspecified), "", 0, 1, false, false, false, false, true, true, false, "", "09", "DV truyền dẫn khác", 0m, "", new DateTime(2020, 9, 14, 7, 42, 29, 20, DateTimeKind.Unspecified).AddTicks(8370) },
            //        { 10, 0, "", new DateTime(2020, 9, 9, 4, 33, 34, 692, DateTimeKind.Unspecified).AddTicks(60), "", 0, 2, false, false, false, false, false, true, false, "", "10", "Điện thoại cố định", 0m, "", null },
            //        { 11, 0, "", new DateTime(2020, 9, 9, 4, 33, 58, 286, DateTimeKind.Unspecified).AddTicks(1170), "", 0, 2, false, false, false, false, false, true, false, "", "11", "SMS Marketing/SMS", 0m, "", null },
            //        { 12, 0, "", new DateTime(2020, 9, 9, 4, 34, 40, 991, DateTimeKind.Unspecified).AddTicks(2040), "", 0, 2, false, false, false, false, false, true, false, "", "12", "1900/1800", 0m, "", null },
            //        { 13, 0, "", new DateTime(2020, 9, 9, 4, 37, 15, 139, DateTimeKind.Unspecified).AddTicks(4560), "", 0, 2, false, false, false, false, false, true, false, "", "13", "Tổng đài ảo", 0m, "", null },
            //        { 14, 0, "", new DateTime(2020, 9, 9, 4, 37, 30, 897, DateTimeKind.Unspecified).AddTicks(5020), "", 0, 2, false, false, false, false, false, true, false, "", "14", "Các dịch vụ thoại IP khác", 0m, "", null },
            //        { 15, 0, "", new DateTime(2020, 9, 9, 4, 38, 7, 741, DateTimeKind.Unspecified).AddTicks(5550), "", 0, 3, false, false, false, false, false, true, false, "", "15", "Các dịch vụ Hosting (Tele + Web + data center, thiết kế web, thuê đặt máy chủ)", 0m, "", null },
            //        { 16, 0, "", new DateTime(2020, 9, 9, 4, 38, 17, 737, DateTimeKind.Unspecified), "", 0, 3, false, false, false, false, false, true, false, "", "16", "Các dịch vụ GTGT khác (Video…)", 0m, "", new DateTime(2020, 10, 14, 8, 43, 45, 787, DateTimeKind.Unspecified).AddTicks(9530) }
            //    });
        }
    }
}
