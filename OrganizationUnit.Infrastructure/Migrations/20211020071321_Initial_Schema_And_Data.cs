using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.Migrations
{
    public partial class Initial_Schema_And_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigurationSystemParameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ChangeRecordExportExcel = table.Column<int>(nullable: false),
                    ChangeRecordExportPdf = table.Column<int>(nullable: false),
                    OrganizationUnit = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    BankAccount = table.Column<string>(nullable: true),
                    TelephoneSwitchboard = table.Column<string>(nullable: true),
                    RepresentativePersonName = table.Column<string>(nullable: true),
                    RpPosition = table.Column<string>(nullable: true),
                    AuthorizationLetterNumber = table.Column<string>(nullable: true),
                    TradingAddress = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    NumberDaysBadDebt = table.Column<int>(nullable: false),
                    NumberDaysOverdue = table.Column<int>(nullable: false),
                    NotifyChannelExpirationDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationSystemParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FCMTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ReceiverId = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCMTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Otps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Otp = table.Column<string>(nullable: true),
                    DateExpired = table.Column<DateTime>(nullable: true),
                    IsUse = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PermissionSetId = table.Column<int>(nullable: false),
                    PermissionName = table.Column<string>(maxLength: 256, nullable: true),
                    PermissionCode = table.Column<string>(maxLength: 256, nullable: false),
                    PermissionPage = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    FileName = table.Column<string>(maxLength: 256, nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Order = table.Column<int>(nullable: true),
                    PictureType = table.Column<int>(nullable: false),
                    Extension = table.Column<string>(nullable: false),
                    RedirectLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 256, nullable: false),
                    RoleCode = table.Column<string>(maxLength: 256, nullable: false),
                    RoleDescription = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false),
                    Grant = table.Column<bool>(nullable: false),
                    Deny = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IdentityGuid = table.Column<string>(nullable: false),
                    AccountingCustomerCode = table.Column<string>(maxLength: 256, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(maxLength: 256, nullable: true),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    FullName = table.Column<string>(maxLength: 512, nullable: true),
                    ShortName = table.Column<string>(maxLength: 512, nullable: true),
                    MobilePhoneNo = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(maxLength: 1000, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IdNo = table.Column<string>(maxLength: 12, nullable: true),
                    DateOfIssueID = table.Column<DateTime>(type: "datetime", nullable: true),
                    PlaceOfIssueID = table.Column<string>(maxLength: 256, nullable: true),
                    LastIpAddress = table.Column<string>(nullable: true),
                    Password = table.Column<string>(type: "LONGTEXT", nullable: true),
                    SecurityStamp = table.Column<string>(maxLength: 68, nullable: true),
                    AvatarId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    OrganizationUnitId = table.Column<int>(nullable: true),
                    FaxNo = table.Column<string>(maxLength: 50, nullable: true),
                    TaxIdNo = table.Column<string>(nullable: true),
                    RepresentativePersonName = table.Column<string>(maxLength: 256, nullable: true),
                    RpPhoneNo = table.Column<string>(maxLength: 256, nullable: true),
                    RpDateOfBirth = table.Column<DateTime>(nullable: true),
                    RpJobPosition = table.Column<string>(maxLength: 256, nullable: true),
                    BusinessRegCertificate = table.Column<string>(maxLength: 256, nullable: true),
                    BrcDateOfIssue = table.Column<DateTime>(nullable: true),
                    BrcIssuedBy = table.Column<string>(maxLength: 1000, nullable: true),
                    JobPosition = table.Column<string>(maxLength: 256, nullable: true),
                    JobTitle = table.Column<string>(maxLength: 256, nullable: true),
                    IsLock = table.Column<bool>(nullable: false),
                    IsPartner = table.Column<bool>(nullable: false),
                    IsEnterprise = table.Column<bool>(nullable: false),
                    IsCustomer = table.Column<bool>(nullable: false),
                    ApplicationUserIdentityGuid = table.Column<string>(nullable: true),
                    IsCustomerInternational = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    TradingAddress = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Pictures_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationPersonalAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    AllowSendEmail = table.Column<bool>(nullable: false),
                    AllowSendNotification = table.Column<bool>(nullable: false),
                    AllowSendSMS = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationPersonalAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigurationPersonalAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 10, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IdentityGuid = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    ShortName = table.Column<string>(maxLength: 256, nullable: true),
                    Address = table.Column<string>(maxLength: 1000, nullable: true),
                    NumberPhone = table.Column<string>(maxLength: 100, nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    ProvinceId = table.Column<string>(nullable: true),
                    TreePath = table.Column<string>(nullable: true),
                    ManagementUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationUnits_Users_ManagementUserId",
                        column: x => x.ManagementUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(maxLength: 256, nullable: true),
                    BankAccountNumber = table.Column<string>(maxLength: 256, nullable: true),
                    BankBranch = table.Column<string>(maxLength: 256, nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBankAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ConfigurationSystemParameters",
                columns: new[] { "Id", "Address", "AuthorizationLetterNumber", "BankAccount", "ChangeRecordExportExcel", "ChangeRecordExportPdf", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "NotifyChannelExpirationDays", "NumberDaysBadDebt", "NumberDaysOverdue", "OrganizationPath", "OrganizationUnit", "RepresentativePersonName", "RpPosition", "TaxCode", "TelephoneSwitchboard", "TradingAddress", "UpdatedBy", "UpdatedDate", "Website" },
                values: new object[] { 1, "Tầng 6 – Lotus Building, số 2, Duy Tân, Phường Dịch Vọng Hậu, Quận Cầu Giấy, Thành phố Hà Nội, Việt Nam", "", "", 1000, 100, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, 30, 30, 30, "", "Công ty cổ phần HTC viễn thông quốc tế", "TRỊNH MINH CHÂU", "", "0102362584", "(024) 3573 9419 - (024) 3538 1916", "Tầng 6 – Lotus Building, số 2, Duy Tân, Phường Dịch Vọng Hậu, Quận Cầu Giấy, Thành phố Hà Nội, Việt Nam", "", null, "https://htc-itc.com.vn/" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "PermissionCode", "PermissionName", "PermissionPage", "PermissionSetId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 176, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_ARTICLE", "Xóa danh mục bài viết", "/article", 0, "", null },
                    { 177, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_ARTICLE", "Xuất excel danh  sách bài viết", "/article", 0, "", null },
                    { 178, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_ARTICLE", "Xuất pdf danh sách bài viết", "/article", 0, "", null },
                    { 179, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_ARTICLE", "Chỉnh sửa bài viết", "/article", 0, "", null },
                    { 180, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_PERMISSION", "Hiển thị danh sách quyền", "/permission", 0, "", null },
                    { 181, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_PERMISSION", "Thêm mới quyền", "/permission", 0, "", null },
                    { 182, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_PERMISSION", "Xóa quyền", "/permission", 0, "", null },
                    { 183, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_PERMISSION", "Xuất excel danh sách quyền", "/permission", 0, "", null },
                    { 184, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_PERMISSION", "Xuất pdf danh sách quyền", "/permission", 0, "", null },
                    { 185, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_PERMISSION", "Chỉnh sửa quyền", "/permission", 0, "", null },
                    { 186, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_ROLE", "Hiển thị danh sách vai trò người dùng", "/role", 0, "", null },
                    { 187, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_ROLE", "Thêm mới vai trò người dùng", "/role", 0, "", null },
                    { 188, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_ROLE", "Xóa vai trò người dùng", "/role", 0, "", null },
                    { 189, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_ROLE", "Xuất excel vai trò người dùng", "/role", 0, "", null },
                    { 175, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_ARTICLE", "Thêm mới bài viết", "/article", 0, "", null },
                    { 190, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_ROLE", "Xuất pdf vai trò người dùng", "/role", 0, "", null },
                    { 192, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ASSIGNMENT_REVOKE_ROLE", "Gắn/bỏ quyền quản trị", "/role", 0, "", null },
                    { 193, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ACCOUNT_USERS_IN_ROLE", "Danh sách người dùng trong vai trò ", "/role", 0, "", null },
                    { 194, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_LOCATION", "Hiển thị đơn vị hành chính", "/location", 0, "", null },
                    { 195, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_LOCATION", "Thêm mới đơn vị hành chính", "/location", 0, "", null },
                    { 196, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_LOCATION", "Xóa đơn vị hành chính", "/location", 0, "", null },
                    { 197, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_LOCATION", "Xuất dữ liệu EXCEL đơn vị hành chính", "/location", 0, "", null },
                    { 198, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_LOCATION", "Xuất dữ liệu PDF đơn vị hành chính", "/location", 0, "", null },
                    { 199, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_LOCATION", "Chỉnh sửa đơn vị hành chính", "/location", 0, "", null },
                    { 200, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_ORGANIZATION_UNIT", "Hiển thị đơn vị tổ chức", "/organization-unit", 0, "", null },
                    { 201, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_ORGANIZATION_UNIT", "Thêm mới đơn vị tổ chức", "/organization-unit", 0, "", null },
                    { 202, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_ORGANIZATION_UNIT", "Xóa đơn vị tổ chức", "/organization-unit", 0, "", null },
                    { 203, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_ORGANIZATION_UNIT", "Xuất dữ liệu EXCEL đơn vị tổ chức", "/organization-unit", 0, "", null },
                    { 204, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_ORGANIZATION_UNIT", "Xuất dữ liệu PDF đơn vị tổ chức", "/organization-unit", 0, "", null },
                    { 205, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_ORGANIZATION_UNIT", "Chỉnh sửa đơn vị tổ chức", "/organization-unit", 0, "", null },
                    { 191, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_ROLE", "Chỉnh sửa vai trò người dùng", "/role", 0, "", null },
                    { 174, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_ARTICLE", "Hiển thị danh sách bài viết", "/article", 0, "", null },
                    { 173, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_ARTICLE_CATEGORY", "Chỉnh sửa danh mục bài viết", "/article-category", 0, "", null },
                    { 172, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_ARTICLE_CATEGORY", "Xuất pdf danh sách danh mục bài viết", "/article-category", 0, "", null },
                    { 140, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_PROMOTION", "Hiển thị danh sách khuyến mãi", "/promotion", 0, "", null },
                    { 141, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_PROMOTION", "Thêm mới khuyến mãi", "/promotion", 0, "", null },
                    { 142, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_PROMOTION", "Xóa khuyến mãi", "/promotion", 0, "", null },
                    { 143, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_PROMOTION", "Chỉnh sửa khuyến mãi", "/promotion", 0, "", null },
                    { 144, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_MARKET_AREA", "Hiển thị danh sách vùng thị trường", "/market-area", 0, "", null },
                    { 145, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_MARKET_AREA", "Thêm mới vùng thị trường", "/market-area", 0, "", null },
                    { 146, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_MARKET_AREA", "Xóa vùng thị trường", "/market-area", 0, "", null },
                    { 148, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_MARKET_AREA", "Xuất pdf danh sách vùng thị trường", "/market-area", 0, "", null },
                    { 149, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_MARKET_AREA", "Chỉnh sửa vùng thị trường", "/market-area", 0, "", null },
                    { 150, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_PROJECT", "Hiển thị danh sách dự án", "/project-management", 0, "", null },
                    { 151, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_PROJECT", "Thêm mới dự án", "/project-management", 0, "", null },
                    { 152, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_PROJECT", "Xóa dự án", "/project-management", 0, "", null },
                    { 153, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_PROJECT", "Xuất excel danh  sách dự án", "/project-management", 0, "", null },
                    { 154, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_PROJECT", "Xuất pdf danh sách dự án", "/project-management", 0, "", null },
                    { 155, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_PROJECT", "Chỉnh sửa dự án", "/project-management", 0, "", null },
                    { 156, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_TAX_CATEGORIES", "Hiển thị danh sách danh mục thuế", "/tax-catagories", 0, "", null },
                    { 157, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_TAX_CATEGORIES", "Thêm mới danh mục thuế", "/tax-catagories", 0, "", null },
                    { 171, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_ARTICLE_CATEGORY", "Xuất excel danh  sách danh mục bài viết", "/article-category", 0, "", null },
                    { 170, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_ARTICLE_CATEGORY", "Xóa danh mục bài viết", "/article-category", 0, "", null },
                    { 169, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_ARTICLE_CATEGORY", "Thêm mới danh mục bài viết", "/article-category", 0, "", null },
                    { 168, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_ARTICLE_CATEGORY", "Hiển thị danh sách danh mục bài viết", "/article-category", 0, "", null },
                    { 167, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_UNIT_OF_MEASUREMENT", "Chỉnh sửa đơn vị đo", "/unit-of-measurement", 0, "", null },
                    { 166, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_UNIT_OF_MEASUREMENT", "Xuất pdf danh sách đơn vị đo", "/unit-of-measurement", 0, "", null },
                    { 206, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_USER", "Hiển thị tài khoản người dùng", "/user", 0, "", null },
                    { 165, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_UNIT_OF_MEASUREMENT", "Xuất excel danh  sách đơn vị đo", "/unit-of-measurement", 0, "", null },
                    { 163, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_UNIT_OF_MEASUREMENT", "Thêm mới đơn vị đo", "/unit-of-measurement", 0, "", null },
                    { 162, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_UNIT_OF_MEASUREMENT", "Hiển thị danh sách đơn vị đo", "/unit-of-measurement", 0, "", null },
                    { 161, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_TAX_CATEGORIES", "Chỉnh sửa danh mục thuế", "/tax-catagories", 0, "", null },
                    { 160, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_TAX_CATEGORIES", "Xuất pdf danh sách danh mục thuế", "/tax-catagories", 0, "", null },
                    { 159, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_TAX_CATEGORIES", "Xuất excel danh  sách danh mục thuế", "/tax-catagories", 0, "", null },
                    { 158, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_TAX_CATEGORIES", "Xóa danh mục thuế", "/tax-catagories", 0, "", null },
                    { 164, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_UNIT_OF_MEASUREMENT", "Xóa đơn vị đo", "/unit-of-measurement", 0, "", null },
                    { 207, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_USER", "Thêm mới tài khoản người dùng", "/user", 0, "", null },
                    { 208, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_USER", "Xóa tài khoản người dùng", "/user", 0, "", null },
                    { 209, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_USER", "Xuất dữ liệu EXCEL tài khoản người dùng", "/user", 0, "", null },
                    { 245, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_INDUSTRY", "Xóa lĩnh vực khách hàng", "/industry", 0, "", null },
                    { 246, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_INDUSTRY", "Xuất excel danh sách lĩnh vực khách hàng", "/industry", 0, "", null },
                    { 247, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_INDUSTRY", "Xuất pdf danh sách lĩnh vực khách hàng", "/industry", 0, "", null },
                    { 433, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_TOTAL_REVENUE", "Xem báo cáo tổng doanh thu", "/report/report-total-revenue", 0, "", null },
                    { 434, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CONFIRM_IN_OUT_EQUIPMENT", "Xác nhận vào/ra thiết bị", "/transaction-management", 0, "", null },
                    { 435, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "TECHNICAL_ACCEPTANCE", "Nghiệm thu kĩ thuật", "/transaction-management", 0, "", null },
                    { 436, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_GENERAL_CSKH_FTTH", "Xem báo cáo khách hàng tổng hợp ", "/report/general-cskh-ffth", 0, "", null },
                    { 437, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_EXPORT_INVOICE", "Xem báo cáo file xuất hóa đơn", "/report/report-export-invoice", 0, "", null },
                    { 438, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_PAYMENT_VOUCHER", "Xem Báo cáo công nợ phải chi doanh nghiệp FTTH CSKH", "/report/payment-voucher", 0, "", null },
                    { 439, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_CHANNEL_FEE_REPORT", "Xem báo cáo chi phí kênh truyền", "/report/channel-fee-report", 0, "", null },
                    { 440, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "IMPORT_SEED_CONTRACT", "Nhập lên tệp tin dữ liệu hợp đồng", "/contract-output-management", 0, "", null },
                    { 322, "", new DateTime(2020, 10, 9, 9, 2, 8, 0, DateTimeKind.Unspecified), "", "Xem thông tin Radius Server", 0, true, false, "", "VIEW_RADIUS_SERVER_INFO", "Xem thông tin Radius Server", "/radius-bras/radius-server-info", 0, "", null },
                    { 323, "", new DateTime(2020, 10, 9, 9, 2, 41, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "ADD_RADIUS_SERVER_INFO", "Thêm mới thông tin Radius Server", "/radius-bras/radius-server-info", 0, "", null },
                    { 324, "", new DateTime(2020, 10, 9, 9, 3, 14, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "EDIT_RADIUS_SERVER_INFO", "Chỉnh sửa thông tin Radius Server", "/radius-bras/radius-server-info", 0, "", null },
                    { 325, "", new DateTime(2020, 10, 9, 9, 3, 32, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "DELETE_RADIUS_SERVER_INFO", "Xóa thông tin Radius Server", "/radius-bras/radius-server-info", 0, "", null },
                    { 326, "", new DateTime(2020, 10, 9, 9, 4, 49, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEW_BRAS_INFO", "Xem thông tin BRAS", "/radius-bras/bras-info", 0, "", null },
                    { 327, "", new DateTime(2020, 10, 9, 9, 5, 3, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "ADD_BRAS_INFO", "Thêm mới thông tin BRAS", "/radius-bras/bras-info", 0, "", null },
                    { 432, "", new DateTime(2020, 12, 8, 4, 15, 2, 0, DateTimeKind.Unspecified), "", "Xem công nợ đầu vào theo hợp đồng", 0, true, false, "", "VIEW_IN_DEBT_BY_CONTRACT", "Xem công nợ đầu vào theo hợp đồng", "/debt-management/in-debt-by-contracts", 0, "", null },
                    { 431, "", new DateTime(2020, 10, 30, 8, 19, 56, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "PAYMENT_CONFIRMATION", "Kế toán phê duyệt chi tiền", "/debt-management/payment-voucher", 0, "", null },
                    { 339, "", new DateTime(2020, 10, 29, 2, 58, 18, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "CONFIRM_COLLECTION_ON_BEHALF_DEBT", "Xác nhận công nợ thu hộ", "/debt-management/accountant-confirmation", 0, "", null },
                    { 338, "", new DateTime(2020, 10, 28, 2, 5, 9, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEW_IN_DEBT_BY_PARTNER", "Xem công nợ đầu vào của đối tác/đại lý", "/debt-management/in-debt-by-partners", 0, "", null },
                    { 337, "", new DateTime(2020, 10, 28, 2, 4, 38, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEW_OUT_DEBT_COLLECTION_ON_BEHALF", "Xem công nợ thu hộ của nhân viên/đại lý", "/debt-management/debt-collection-on-behalf", 0, "", null },
                    { 336, "", new DateTime(2020, 10, 28, 2, 4, 26, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEW_OUT_DEBT_BY_CONTRACT", "Xem công nợ đầu ra theo hợp đồng", "/debt-management/out-debt-by-contracts", 0, "", null },
                    { 244, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_INDUSTRY", "Thêm mới lĩnh vực khách hàng", "/industry", 0, "", null },
                    { 335, "", new DateTime(2020, 10, 28, 2, 4, 4, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEW_OUT_DEBT_BY_CUSTOMER", "Xem công nợ đầu ra", "/debt-management/out-debt-by-customers", 0, "", null },
                    { 333, "", new DateTime(2020, 10, 14, 8, 31, 18, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEWLIST_CLEARING_TO_CUSTOMER", "Hiển thị danh sách bù trừ công nợ cho khách hàng", "/debt-management/clearing-to-customer", 0, "", null },
                    { 332, "", new DateTime(2020, 10, 14, 8, 30, 32, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEWLIST_FOR_STAFF", "Hiển thị danh sách công nợ cho nhân viên/ đối tác thu hộ", "/debt-management/debt-for-staft-partner", 0, "", null },
                    { 331, "", new DateTime(2020, 10, 14, 8, 29, 12, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEWLIST_DEBT_BY_CUSTOMER", "Hiển thị danh sách công nợ đầu ra theo khách hàng", "/debt-management/debt-by-customers", 0, "", new DateTime(2020, 10, 22, 8, 47, 5, 0, DateTimeKind.Unspecified) },
                    { 330, "", new DateTime(2020, 10, 14, 8, 28, 31, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEWLIST_IN_DEBT", "Hiển thị danh sách công nợ đầu vào", "/debt-management/in-debt", 0, "", null },
                    { 329, "", new DateTime(2020, 10, 9, 9, 5, 30, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "DELETE_BRAS_INFO", "Xóa thông tin BRAS", "/radius-bras/bras-info", 0, "", null },
                    { 328, "", new DateTime(2020, 10, 9, 9, 5, 17, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "EDIT_BRAS_INFO", "Chỉnh sửa thông tin BRAS", "/radius-bras/bras-info", 0, "", null },
                    { 334, "", new DateTime(2020, 10, 22, 8, 46, 51, 0, DateTimeKind.Unspecified), "", "", 0, true, false, "", "VIEW_DEBT_BY_CONTRACTS", "Hiển thị danh sách công nợ đầu ra theo hợp đồng", "/debt-management/debt-by-contracts", 0, "", null },
                    { 139, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_PARTNER", "Chỉnh sửa tài khoản đại lý đối tác", "/partner-management", 0, "", null },
                    { 243, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_INDUSTRY", "Hiển thị danh sách lĩnh vực khách hàng", "/industry", 0, "", null },
                    { 241, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_CONTRACT_FORM", "Xuất dữ liệu EXCEL mẫu hợp đồng", "/contract-form", 0, "", null },
                    { 210, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_USER", "Xuất dữ liệu PDF tài khoản người dùng", "/user", 0, "", null },
                    { 211, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_USER", "Chỉnh sửa tài khoản người dùng", "/user", 0, "", null },
                    { 212, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_DASHBOARD", "Hiển thị Dashboard", "/dashboard", 0, "", null },
                    { 213, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_CONFIGURATION_SYSTEM", "Hiển thị màn hình thay đổi tham số cấu hình hệ thống", "/user-profile/configuration-system", 0, "", null },
                    { 214, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_FEEDBACK", "Thêm mới feeback", "/feedback", 0, "", null },
                    { 215, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_FEEDBACK", "Xóa feedback", "/feedback", 0, "", null },
                    { 216, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_FEEDBACK", "Sửa feedback", "/feedback", 0, "", null },
                    { 217, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_FEEDBACK", "Xuất ra file excel", "/feedback", 0, "", null },
                    { 218, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_FEEDBACK", "Xuất ra file PDF", "/feedback", 0, "", null },
                    { 219, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_BILLING_RECEIPT_VOUCHER", "Phiếu thu tính cước", "/debt-management/receipt-voucher", 0, "", null },
                    { 220, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_OUT_CONTRACT_SIGNED", "Cập nhật ngày nghiệm thu, thiết bị, ghi chú", "/contract-output-management", 0, "", null },
                    { 221, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "PRINT_OUT_CONTRACT", "In hợp đồng đầu ra", "/contract-output-management", 0, "", null },
                    { 222, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_CLEARING_TO_CUSTOMER", "Hiển thị bù trừ công nợ", "/debt-management/clearing-to-customer", 0, "", null },
                    { 223, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_CLEARING_DEBT", "Hiển thị biên bản bù trừ", "/debt-management/clearing-debt", 0, "", null },
                    { 224, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_CLEARING_DEBT", "Hiển thị nợ xấu", "/debt-management/bad-debt", 0, "", null },
                    { 225, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_DEPLOY_NEW_OUT_CONTRACT", "Duyệt triển khai hợp đồng mới", "/transaction-management", 0, "", null },
                    { 226, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_ALL_RECEIPT_VOUCHER", "Xem tất cả phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 240, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_CONTRACT_FORM", "Xóa mẫu hợp đồng", "/contract-form", 0, "", null },
                    { 239, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CONTRACT_FORM", "Chỉnh sửa mẫu hợp đồng", "/contract-form", 0, "", null },
                    { 238, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_CONTRACT_FORM", "Thêm mới mẫu hợp đồng", "/contract-form", 0, "", null },
                    { 237, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_CONTRACT_FORM", "Hiển thị mẫu hợp đồng", "/contract-form", 0, "", null },
                    { 236, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_CONTRACTOR_HTC", "Xuất dữ liệu PDF công ty", "/contractor-htc", 0, "", null },
                    { 235, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_CONTRACTOR_HTC", "Xuất dữ liệu EXCEL công ty", "/contractor-htc", 0, "", null },
                    { 242, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_CONTRACT_FORM", "Xuất dữ liệu PDF mẫu hợp đồng", "/contract-form", 0, "", null },
                    { 234, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CONTRACTOR_HTC", "Chỉnh sửa công ty", "/contractor-htc", 0, "", null },
                    { 232, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_INCONTRACT_FEE_REVENUE", "Xem báo cáo Hợp đồng dầu vào và chi phí, phân chia hoa hồng", "/report/report-incontract-fee-and-sharing", 0, "", null },
                    { 231, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_DEBT", "Xem báo cáo dịch vụ công nợ", "/report/service-debt", 0, "", null },
                    { 230, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_EQUIPMENT", "Xem báo cáo Tổng số thiết bị", "/report/equipment-total", 0, "", null },
                    { 229, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_MASTER_CUSTOMER", "Xem báo cáo Master khách hàng", "/report/master-customer-nationwide-business", 0, "", null },
                    { 228, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_REPORT_CUSTOMER", "Xem báo cáo tổng hợp khách hàng", "/report/contractor-total", 0, "", null },
                    { 227, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "BAD_DEBT_CONFIRMATION", "Xác nhận nợ xấu", "/debt-management/receipt-voucher", 0, "", null },
                    { 233, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_CONTRACTOR_HTC", "Thêm mới công ty", "/contractor-htc", 0, "", null },
                    { 138, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_PARTNER", "Xuất pdf danh sách tài khoản đại lý đối tác", "/partner-management", 0, "", null },
                    { 147, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_MARKET_AREA", "Xuất excel danh  sách vùng thị trường", "/market-area", 0, "", null }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "PermissionCode", "PermissionName", "PermissionPage", "PermissionSetId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 136, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_PARTNER", "Xóa tài khoản đại lý đối tác", "/partner-management", 0, "", null },
                    { 36, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_TERMINATE_OUT_CONTRACT", "Duyệt hủy hợp đồng đầu ra", "/transaction-management", 0, "", null },
                    { 37, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_PAYMENT_VOUCHER", "Thêm mới phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 38, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_PAYMENT_VOUCHER", "Chỉnh sửa phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 39, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CANCEL_PAYMENT_VOUCHER", "Hủy phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 40, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "PRINT_PAYMENT_VOUCHER", "In phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 41, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_PAYMENT_VOUCHER", "Xuất pdf phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 42, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_PAYMENT_VOUCHER", "Xuất excel phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 43, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_PAYMENT_VOUCHER", "Hiển thị danh sách phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 44, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "IMPORT_PAYMENT_VOUCHER", "Import data phiếu đề nghị thanh toán", "/debt-management/payment-voucher", 0, "", null },
                    { 45, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_DEBT", "Chỉnh sửa công nợ", "/debt-management/receipt-voucher", 0, "", null },
                    { 46, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "PRINT_RECEIPT_VOUCHER", "In phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 47, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CANCEL_RECEIPT_VOUCHER", "Hủy phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 48, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "FEE_REDUCTION", "Giảm trừ cước", "/debt-management/receipt-voucher", 0, "", null },
                    { 49, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_RECEIPT_VOUCHER", "Xuất pdf phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 50, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_RECEIPT_VOUCHER", "Xuất excel phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 51, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_RECEIPT_VOUCHER", "Hiển thị danh sách phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 52, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_RECEIPT_VOUCHER", "Thêm mới phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 66, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_GROUP_SERVICE", "Chỉnh sửa nhóm dịch vụ", "/service-groups-management", 0, "", null },
                    { 65, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_GROUP_SERVICE", "Xuất pdf nhóm dịch vụ", "/service-groups-management", 0, "", null },
                    { 64, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_GROUP_SERVICE", "Xuất excel nhóm dịch vụ", "/service-groups-management", 0, "", null },
                    { 63, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_GROUP_SERVICE", "Xóa nhóm dịch vụ", "/service-groups-management", 0, "", null },
                    { 62, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_GROUP_SERVICE", "Thêm mới nhóm dịch vụ", "/service-groups-management", 0, "", null },
                    { 61, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "IMPORT_CLEARING_DEBT", "Import data phiếu phụ lục bù trừ", "/debt-management/clearing-debt", 0, "", null },
                    { 35, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_RECLAIM_EQUIPMENT_OUT_CONTRACT", "Duyệt thu hồi thiết bị", "/transaction-management", 0, "", null },
                    { 60, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_CLEARING_DEBT", "Hiển thị danh sách phiếu bù trừ công nợ", "/debt-management/clearing-debt", 0, "", null },
                    { 58, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVE_CLEARING_DEBT", "Duyệt phiếu bù trừ", "/debt-management/clearing-debt", 0, "", null },
                    { 57, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CLEARING_DEBT", "Chỉnh sửa phiếu phụ lục bù trừ", "/debt-management/clearing-debt", 0, "", null },
                    { 56, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_CLEARING_DEBT", "Thêm mới phiếu phụ lục bù trừ", "/debt-management/clearing-debt", 0, "", null },
                    { 55, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "IMPORT_RECEIPT_VOUCHER", "Nhập excel phiếu chi", "/debt-management/receipt-voucher", 0, "", null },
                    { 54, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_DETAIL_OUT_CONTRACT", "Chi tiết hợp đồng đầu ra", "/debt-management/receipt-voucher", 0, "", null },
                    { 53, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_OTHER_RECEIPT_VOUCHER", "Thêm mới phiếu thu khác", "/debt-management/receipt-voucher", 0, "", null },
                    { 137, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_PARTNER", "Xuất excel danh  sách tài khoản đại lý đối tác", "/partner-management", 0, "", null },
                    { 67, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_GROUP_SERVICE", "Hiển thị danh sách nhóm dịch vụ", "/service-groups-management", 0, "", null },
                    { 34, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_CHANGE_EQUIPMENT_OUT_CONTRACT", "Duyệt thay đổi thiết bị", "/transaction-management", 0, "", null },
                    { 32, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_CHANGE_SERVICE_PACKAGE_OUT_CONTRACT", "Duyệt thay đổi dịch vụ/gói cước", "/transaction-management", 0, "", null },
                    { 1, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_OUT_CONTRACT", "Chi tiết hợp đồng đầu ra", "/contract-output-management", 0, "", null },
                    { 2, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_OUT_CONTRACT", "Thêm mới hợp đồng đầu ra", "/contract-output-management", 0, "", null },
                    { 3, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_OUT_CONTRACT", "Chỉnh sửa hợp đồng đầu ra", "/contract-output-management", 0, "", null },
                    { 4, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "UPGRADE_EQUIPMENTS_OUT_CONTRACT", "Nâng cấp thiết bị", "/contract-output-management", 0, "", null },
                    { 5, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "UPGRADE_BANDWIDTH_OUT_CONTRACT", "Nâng cấp dịch vụ/gói cước", "/contract-output-management", 0, "", null },
                    { 6, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "SUSPEND_SERVICE_PACKAGE_OUT_CONTRACT", "Tạm ngưng dịch vụ", "/contract-output-management", 0, "", null },
                    { 7, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "TERMINATE_SERVICE_PACKAGE_OUT_CONTRACT", "Hủy dịch vụ", "/contract-output-management", 0, "", null },
                    { 8, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "RESTORE_SERVICE_PACKAGE_OUT_CONTRACT", "Khôi phục dịch vụ", "/contract-output-management", 0, "", null },
                    { 9, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_SERVICE_PACKAGE_OUT_CONTRACT", "Thêm mới dịch vụ/gói cước", "/contract-output-management", 0, "", null },
                    { 10, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CHANGE_SERVICE_PACKAGE_OUT_CONTRACT", "Thay đổi dịch vụ/gói cước", "/contract-output-management", 0, "", null },
                    { 11, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CHANGE_LOCATION_OUT_CONTRACT", "Dịch chuyển địa điểm", "/contract-output-management", 0, "", null },
                    { 12, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CHANGE_EQUIPMENT_OUT_CONTRACT", "Thay đổi thiết bị", "/contract-output-management", 0, "", null },
                    { 13, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "RECLAIM_EQUIPMENT_OUT_CONTRACT", "Thu hồi thiết bị", "/contract-output-management", 0, "", null },
                    { 14, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "TERMINATE_OUT_CONTRACT", "Hủy hợp đồng đầu ra", "/contract-output-management", 0, "", null },
                    { 15, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_OUT_CONTRACT", "Xuất dữ liệu PDF đầu ra", "/contract-output-management", 0, "", null },
                    { 16, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_OUT_CONTRACT", "Xuất dữ liệu Excel đầu ra", "/contract-output-management", 0, "", null },
                    { 17, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_IN_CONTRACT", "Chi tiết hợp đồng đầu vào", "/contract-input-management", 0, "", null },
                    { 31, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_ADD_NEW_SERVICE_PACKAGE_OUT_CONTRACT", "Duyệt thêm mới dịch vụ/gói cước", "/transaction-management", 0, "", null },
                    { 30, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_RESTORE_SERVICE_PACKAGE_OUT_CONTRACT", "Duyệt khôi phục dịch vụ", "/transaction-management", 0, "", null },
                    { 29, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_TERMINATE_SERVICE_PACKAGE_OUT_CONTRACT", "Duyệt hủy dịch vụ", "/transaction-management", 0, "", null },
                    { 28, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_SUSPEND_SERVICE_PACKAGE_OUT_CONTRACT", "Duyệt tạm ngưng dịch vụ", "/transaction-management", 0, "", null },
                    { 27, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_UPGRADE_BANDWIDTH_OUT_CONTRACT", "Duyệt nâng cấp dịch vụ/gói cước", "/transaction-management", 0, "", null },
                    { 26, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_UPGRADE_EQUIPMENTS_OUT_CONTRACT", "Duyệt nâng cấp thiết bị", "/transaction-management", 0, "", null },
                    { 33, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_CHANGE_LOCATION_OUT_CONTRACT", "Duyệt dịch chuyển địa điểm", "/transaction-management", 0, "", null },
                    { 25, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CANCELLED_TRANSACTION", "Từ chối phụ lục", "/transaction-management", 0, "", null },
                    { 23, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEW_TRANSACTION", "Chi tiết phụ lục", "/transaction-management", 0, "", null },
                    { 22, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXCEL_IN_CONTRACT", "Xuất dữ liệu Excel đầu vào", "/contract-input-management", 0, "", null },
                    { 21, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_IN_CONTRACT", "Xuất dữ liệu PDF đầu vào", "/contract-input-management", 0, "", null },
                    { 20, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "TERMINATE_IN_CONTRACT", "Hủy hợp đồng đầu vào", "/contract-input-management", 0, "", null },
                    { 19, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_IN_CONTRACT", "Chỉnh sửa hợp đồng đầu vào", "/contract-input-management", 0, "", null },
                    { 18, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_IN_CONTRACT", "Thêm mới hợp đồng đầu vào", "/contract-input-management", 0, "", null },
                    { 24, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "APPROVED_TRANSACTION", "Phê duyệt phụ lục", "/transaction-management", 0, "", null },
                    { 68, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_SERVICE", "Thêm mới dịch vụ", "/services-management", 0, "", null },
                    { 59, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "CANCEL_CLEARING_DEBT", "Hủy phiếu bù trừ", "/debt-management/clearing-debt", 0, "", null },
                    { 70, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_SERVICE", "Xuất excel dịch vụ", "/services-management", 0, "", null },
                    { 106, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_CUSTOMER_CLASSES", "Xuất pdf danh sách hạng khách hàng", "/customer-classes", 0, "", null },
                    { 107, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_CUSTOMER_CLASSES", "Xóa hạng khách hàng", "/customer-classes", 0, "", null },
                    { 108, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CUSTOMER_CLASSES", "Chỉnh sửa thông tin hạng khách hàng", "/customer-classes", 0, "", null },
                    { 109, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_CUSTOMER_CLASSES", "Hiển thị danh sách hạng khách hàng", "/customer-classes", 0, "", null },
                    { 110, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_CUSTOMER_GROUP", "Thêm mới nhóm khách hàng", "/customer-group", 0, "", null },
                    { 111, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_CUSTOMER_GROUP", "Xuất excel danh sách nhóm khách hàng", "/customer-group", 0, "", null },
                    { 112, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_CUSTOMER_GROUP", "Xuất pdf danh sách nhóm khách hàng", "/customer-group", 0, "", null },
                    { 69, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_SERVICE", "Xóa dịch vụ", "/services-management", 0, "", null },
                    { 114, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CUSTOMER_GROUP", "Chỉnh sửa thông tin nhóm khách hàng", "/customer-group", 0, "", null },
                    { 115, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_CUSTOMER_GROUP", "Hiển thị danh sách nhóm khách hàng", "/customer-group", 0, "", null },
                    { 116, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "OPEN_CUSTOMER_IN_GROUP", "Mở các tài khoản thuộc nhóm", "/customer-group", 0, "", null },
                    { 117, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_CUSTOMER_STRUCTURE", "Thêm mới cơ cấu khách hàng", "/customer-structure", 0, "", null },
                    { 118, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_CUSTOMER_STRUCTURE", "Xuất excel danh sách cơ cấu khách hàng", "/customer-structure", 0, "", null },
                    { 119, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_CUSTOMER_STRUCTURE", "Xuất pdf danh sách cơ cấu khách hàng", "/customer-structure", 0, "", null },
                    { 105, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_CUSTOMER_CLASSES", "Xuất excel danh sách hạng khách hàng", "/customer-classes", 0, "", null },
                    { 120, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_CUSTOMER_STRUCTURE", "Xóa cơ cấu khách hàng", "/customer-structure", 0, "", null },
                    { 122, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_CUSTOMER_STRUCTURE", "Hiển thị danh sách cơ cấu khách hàng", "/customer-structure", 0, "", null },
                    { 123, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_CUSTOMER_TYPE", "Thêm mới kiểu khách hàng", "/customer-type", 0, "", null },
                    { 124, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_CUSTOMER_TYPE", "Xóa kiểu khách hàng", "/customer-type", 0, "", null },
                    { 125, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CUSTOMER_TYPE", "Chỉnh sửa kiểu khách hàng", "/customer-type", 0, "", null },
                    { 126, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_CUSTOMER_TYPE", "Hiển thị danh sách kiểu khách hàng", "/customer-type", 0, "", null },
                    { 127, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_CUSTOMER_CATEGORY", "Thêm mới danh mục khách hàng", "/customer-category", 0, "", null },
                    { 128, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_CUSTOMER_CATEGORY", "Xóa danh mục khách hàng", "/customer-category", 0, "", null },
                    { 129, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CUSTOMER_CATEGORY", "Chỉnh sửa danh mục khách hàng", "/customer-category", 0, "", null },
                    { 130, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_CUSTOMER_CATEGORY", "Hiển thị danh sách danh mục khách hàng", "/customer-category", 0, "", null },
                    { 131, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_TRANSACTION", "Xuất excel danh sách phụ lục", "/transaction-management", 0, "", null },
                    { 132, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_TRANSACTION", "Xuất pdf danh sách phụ lục", "/transaction-management", 0, "", null },
                    { 133, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "IMPORT_EXCEL_RECEIPT", "Nhập dữ liệu phiếu thu", "/debt-management/receipt-voucher", 0, "", null },
                    { 134, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_PARTNER", "Hiển thị danh sách tài khoản đại lý và đối tác", "/partner-management", 0, "", null },
                    { 135, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_PARTNER", "Thêm mới tài khoản đại lý đối tác", "/partner-management", 0, "", null },
                    { 121, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CUSTOMER_STRUCTURE", "Chỉnh sửa cơ cấu khách hàng", "/customer-structure", 0, "", null },
                    { 104, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_CUSTOMER_CLASSES", "Thêm mới hạng khách hàng", "/customer-classes", 0, "", null },
                    { 113, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_CUSTOMER_GROUP", "Xóa nhóm khách hàng", "/customer-group", 0, "", null },
                    { 102, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_CUSTOMER", "Chỉnh sửa tài khoản khách hàng", "/customer-management", 0, "", null },
                    { 71, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_SERVICE", "Xuất pdf dịch vụ", "/services-management", 0, "", null },
                    { 72, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_SERVICE", "Chỉnh sửa dịch vụ", "/services-management", 0, "", null },
                    { 73, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_SERVICE", "Hiển thị danh sách dịch vụ", "/services-management", 0, "", null },
                    { 74, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_SERVICE_PACKAGE", "Thêm mới gói cước", "/service-packages-management", 0, "", null },
                    { 75, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_SERVICE_PACKAGE", "Xóa gói cước", "/service-packages-management", 0, "", null },
                    { 76, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_SERVICE_PACKAGE", "Xuất excel gói cước", "/service-packages-management", 0, "", null },
                    { 103, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_CUSTOMER", "Hiển thị danh sách tài khoản khách hàng", "/customer-management", 0, "", null },
                    { 78, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_SERVICE_PACKAGE", "Chỉnh sửa gói cước", "/service-packages-management", 0, "", null },
                    { 79, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_SERVICE_PACKAGE", "Hiển thị danh sách gói cước", "/service-packages-management", 0, "", null },
                    { 80, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_EQUIPMENT_TYPE", "Thêm mới thiết bị vật tư", "/equipment", 0, "", null },
                    { 81, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_EQUIPMENT_TYPE", "Xóa thiết bị vật tư", "/equipment", 0, "", null },
                    { 82, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_EQUIPMENT_TYPE", "Xuất excel thiết bị vật tư", "/equipment", 0, "", null },
                    { 83, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_EQUIPMENT_TYPE", "Xuất pdf thiết bị vật tư", "/equipment", 0, "", null },
                    { 84, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_EQUIPMENT_TYPE", "Chỉnh sửa thông tin thiết bị vật tư", "/equipment", 0, "", null },
                    { 85, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_EQUIPMENT_TYPE", "Hiển thị danh sách thiết bị vật tư", "/equipment", 0, "", null },
                    { 77, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_SERVICE_PACKAGE", "Xuất pdf gói cước", "/service-packages-management", 0, "", null },
                    { 87, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_SERVICE_CHANNEL_INPUT", "Xóa kênh truyền đầu vào", "/service-channel-input", 0, "", null },
                    { 86, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_SERVICE_CHANNEL_INPUT", "Thêm mới kênh truyền đầu vào", "/service-channel-input", 0, "", null },
                    { 101, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_CUSTOMER", "Xóa tài khoản khách hàng", "/customer-management", 0, "", null },
                    { 100, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_CUSTOMER", "Xuất pdf danh sách khách hàng", "/customer-management", 0, "", null },
                    { 99, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_CUSTOMER", "Xuất excel danh sách khách hàng", "/customer-management", 0, "", null },
                    { 98, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_CUSTOMER", "Thêm mới khách hàng", "/customer-management", 0, "", null },
                    { 97, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_SERVICE_CHANNEL_OUTPUT", "Hiển thị danh sách kênh truyền đầu ra", "/service-channel-output", 0, "", null },
                    { 96, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_SERVICE_CHANNEL_OUTPUT", "Chỉnh sửa thông tin kênh truyền đầu ra", "/service-channel-output", 0, "", null },
                    { 95, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_SERVICE_CHANNEL_OUTPUT", "Xuất pdf kênh truyền đầu ra", "/service-channel-output", 0, "", null },
                    { 94, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_SERVICE_CHANNEL_OUTPUT", "Xuất excel kênh truyền đầu ra", "/service-channel-output", 0, "", null },
                    { 93, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "DELETE_SERVICE_CHANNEL_OUTPUT", "Xóa kênh truyền đầu ra", "/service-channel-output", 0, "", null },
                    { 92, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "ADD_NEW_SERVICE_CHANNEL_OUTPUT", "Thêm mới kênh truyền đầu ra", "/service-channel-output", 0, "", null },
                    { 91, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "VIEWLIST_SERVICE_CHANNEL_INPUT", "Hiển thị danh sách kênh truyền đầu vào", "/service-channel-input", 0, "", null },
                    { 90, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EDIT_SERVICE_CHANNEL_INPUT", "Chỉnh sửa thông tin kênh truyền đầu vào", "/service-channel-input", 0, "", null },
                    { 89, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_PDF_SERVICE_CHANNEL_INPUT", "Xuất pdf kênh truyền đầu vào", "/service-channel-input", 0, "", null },
                    { 88, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", "", 0, true, false, "", "EXPORT_EXECEL_SERVICE_CHANNEL_INPUT", "Xuất excel kênh truyền đầu vào", "/service-channel-input", 0, "", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "RoleCode", "RoleDescription", "RoleName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2020, 8, 31, 12, 34, 19, 0, DateTimeKind.Unspecified), "", 0, true, false, "", "AGENT", "", "Đại lý phân phối", "", new DateTime(2020, 9, 1, 1, 37, 37, 0, DateTimeKind.Unspecified) },
                    { 2, "", new DateTime(2020, 8, 31, 12, 34, 53, 0, DateTimeKind.Unspecified), "", 0, true, false, "", "CUSTOMERCARESTAFF", "", "Nhân viên CSKH", "", new DateTime(2020, 9, 1, 1, 37, 18, 0, DateTimeKind.Unspecified) },
                    { 3, "", new DateTime(2020, 8, 31, 12, 35, 52, 0, DateTimeKind.Unspecified), "", 0, true, false, "", "SALESMANS", "", "Nhân viên kinh doanh", "", new DateTime(2020, 9, 1, 1, 36, 38, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "RoleCode", "RoleDescription", "RoleName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 999, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0, true, false, "", "ADMIN", "", "adminstrator", "", null },
                    { 6, "", new DateTime(2020, 9, 9, 8, 58, 5, 0, DateTimeKind.Unspecified), "", 0, true, false, "", "ACCOUNTANT", "", "Kế toán", "", null },
                    { 5, "", new DateTime(2020, 9, 9, 8, 4, 43, 0, DateTimeKind.Unspecified), "", 0, true, false, "", "SUPPORTER", "", "Kỹ thuật", "", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountingCustomerCode", "Address", "ApplicationUserIdentityGuid", "AvatarId", "BrcDateOfIssue", "BrcIssuedBy", "BusinessRegCertificate", "Code", "CreatedBy", "CreatedDate", "Culture", "DateOfIssueID", "DisplayOrder", "Email", "FaxNo", "FirstName", "FullName", "Gender", "IdNo", "IdentityGuid", "IsActive", "IsCustomer", "IsCustomerInternational", "IsDeleted", "IsEnterprise", "IsLock", "IsPartner", "JobPosition", "JobTitle", "LastIpAddress", "LastName", "MobilePhoneNo", "Note", "OrganizationPath", "OrganizationUnitId", "Password", "PlaceOfIssueID", "RepresentativePersonName", "RpDateOfBirth", "RpJobPosition", "RpPhoneNo", "SecurityStamp", "ShortName", "Status", "TaxIdNo", "TradingAddress", "UpdatedBy", "UpdatedDate", "UserName" },
                values: new object[,]
                {
                    { 7, "", "Sơn Tây", "", null, null, "", "", "", "", new DateTime(2020, 9, 3, 2, 43, 9, 0, DateTimeKind.Unspecified), "", null, 0, "phuongttn@gmail.com", "", "Ngọc Phượng", "Ngọc Phượng Trịnh", 2, "017235865", "1207b062-c891-4b5f-a266-0768c0c9ca82", true, false, false, false, false, false, false, "Phòng kĩ thuật", "Phòng kĩ thuật", null, "Trịnh", "0912868868", "", "", null, "", "", "", null, "", "", "4HSUUJD2URVT7HHKZ7LKIVABHMIKKB3P", "", null, "", "", "", new DateTime(2020, 9, 3, 9, 24, 32, 0, DateTimeKind.Unspecified), "PHUONGTTN" },
                    { 8, "", "Duy Tân", "", null, null, "", "", "DLA`", "", new DateTime(2020, 9, 9, 6, 24, 50, 0, DateTimeKind.Unspecified), "", null, 0, "", "", "", "Đại lý A", null, "017245454", "36355c27-d6aa-4fc2-af40-357788aefd2d", true, false, false, false, false, false, true, "", "", null, "", "0221545548", "", "", null, "", "", "", null, "", "", "UGIY5JYMH53D7J2QFHRYEUMJRBBC7YQT", "", null, "", "", "", null, "" },
                    { 9, "", "Duy Tân", "", null, null, "", "", "VNP", "", new DateTime(2020, 9, 9, 6, 53, 25, 0, DateTimeKind.Unspecified), "", null, 0, "", "", "", "Vinaphone", null, "", "ab861fa1-d5e9-4afa-be6f-a9e6d7df1030", true, false, false, false, true, false, true, "", "", null, "", "0254522356", "", "", null, "", "", "Nguyễn Văn A", null, "", "0352553888", "6KMK7E2CLVQYLQ6XAVHKEER726NJPIS2", "", null, "010235481212132", "", "", null, "" },
                    { 10, "", "Cầu Giấy", "", null, null, "", "", "DN_01", "", new DateTime(2020, 9, 9, 7, 13, 27, 0, DateTimeKind.Unspecified), "", null, 0, "dl01_01@gmail.com", "", "", "Doanh nghiệp 01", null, "", "60fcc75b-4a35-42f3-b83a-349dafff5383", true, false, false, true, true, false, true, "", "", null, "", "0123456789", "", "", null, "", "", "Nguyễn Văn A", null, "Phó giám đốc", "0987466454", "VH7O452ABAG3DN27NIJWREJE3SWZCXYM", "", null, "0918376350", "", "", new DateTime(2020, 9, 9, 7, 13, 51, 0, DateTimeKind.Unspecified), "" },
                    { 11, "", "Cầu Giấy", "", null, null, "", "", "DLA", "", new DateTime(2020, 9, 9, 7, 18, 47, 0, DateTimeKind.Unspecified), "", null, 0, "dailya@gmail.com", "", "", "Đại Lý B", null, "142023746", "658c6e95-27d9-4514-ae53-744d86944bb9", true, false, false, false, false, false, true, "", "", null, "", "0968726736", "", "", null, "", "", "", null, "", "", "IG5KFWYA5OZSTSARKKUQGWXFD3GBQPGU", "", null, "", "", "", new DateTime(2020, 9, 9, 10, 25, 44, 0, DateTimeKind.Unspecified), "" },
                    { 12, "", "Hà Nội", "", null, null, "", "", "", "", new DateTime(2020, 9, 9, 8, 5, 46, 0, DateTimeKind.Unspecified), "", null, 0, "", "", "Kỹ thuật", "Kỹ thuật KT", null, "", "6eed446f-26c6-48ed-8626-65f550b880f1", true, false, false, false, false, false, false, "", "", null, "KT", "0342589765", "", "", null, "AQAAAAEAACcQAAAAEIosCRKDQmmGbwxjBZYYvVBxF4OHAtZS6IhL5hLCW/cMsyjY/NxVeKGiHBSOAnwGPw==", "", "", null, "", "", "TANP6I4W62HXJDAU24V6IJM2Y7SEAI6M", "", null, "", "", "", null, "KYTHUAT_01" },
                    { 13, "", "Hà Nội", "", null, null, "", "", "", "", new DateTime(2020, 9, 9, 8, 44, 1, 0, DateTimeKind.Unspecified), "", null, 0, "", "", "Kinh doanh", "Kinh doanh Kinh", null, "01835486", "bd32df6a-e09c-4479-96bb-a00c788d896e", true, false, false, false, false, false, false, "", "", null, "Kinh", "0352587467", "", "", null, "AQAAAAEAACcQAAAAEHykEnCT5sTOKW1MweGIfWYB/x2skFWRsZS0W9Ug71exTFI8D7CYlPVlyuuPlwJysQ==", "", "", null, "", "", "S7V2D2FIVU5NKJFAISDA4JNFAFVWXGEA", "", null, "", "", "", null, "KINHDOANH_01" },
                    { 14, "", "Hà Nội", "", null, null, "", "", "", "", new DateTime(2020, 9, 9, 8, 49, 7, 0, DateTimeKind.Unspecified), "", null, 0, "", "", "Chăm sóc khách hàng 1", "Chăm sóc khách hàng 1 CS", null, "", "1979a999-af32-4a8e-8bd9-76d5f2ab7cd2", true, false, false, false, false, false, false, "", "", null, "CS", "0352587855", "", "", null, "AQAAAAEAACcQAAAAEKqmci7ugpzfkxJhtean77LKvlvQLZHAys1SM5I18wpVaoEqu4FRKWcW79c8Za7Ekg==", "", "", null, "", "", "QH5QEAY2LCEQWKITEWFRGO5FIXMCTJVL", "", null, "", "", "", null, "CSKH_01" },
                    { 15, "", "Hà Nội", "", null, null, "", "", "", "", new DateTime(2020, 9, 9, 9, 2, 28, 0, DateTimeKind.Unspecified), "", null, 0, "", "", "Kế toán", "Kế toán KT", null, "0145148535", "cdbbd7d4-488a-4215-8095-5d8058d79d0d", true, false, false, false, false, false, false, "", "", null, "KT", "0352587562", "", "", null, "AQAAAAEAACcQAAAAEE3FpnUuBSbnPhTW2SrCdkqDlg6XOdeOMjgtcnVS+usXPt0OdhIsrAGdjDPC6w9Zyg==", "", "", null, "", "", "FA7FDILAJ767LGWXGZFVWCBE5BCUV6RT", "", null, "", "", "", null, "KETOAN_01" },
                    { 5, "", "Cầu Giấy, Hà Nội", "", null, null, "", "", "", "", new DateTime(2020, 9, 1, 1, 32, 46, 0, DateTimeKind.Unspecified), "", null, 0, "nvkd001@htc-itc.com.vn", "", "Anh", "Anh Nguyễn Văn", 1, "012944822", "665d324f-863b-4530-a60b-e09bee7a5b35", true, false, false, false, false, false, false, "", "", null, "Nguyễn Văn", "0989805399", "", "", null, "AQAAAAEAACcQAAAAEGHA71lzmKyR65FA5Mx2WhFo7a1tLQxol8TN3xdXnCh7nOBO05AlzLz79sB8pp6jGw==", "", "", null, "", "", "7UKP6TFGC3SGG4L63YD3JXZPBX6YM7E7", "", null, "", "", "", null, "NVKD001" },
                    { 4, "", "Cầu Giấy, Hà Nội", "", null, null, "", "", "", "", new DateTime(2020, 9, 1, 1, 30, 59, 0, DateTimeKind.Unspecified), "", null, 0, "nvkd01@htc-itc.com.vn", "", "Anh", "Anh Nguyễn Văn", null, "01233922", "4336c0b9-3293-41f2-a05b-38d795013eb6", true, false, false, true, false, false, false, "", "", null, "Nguyễn Văn", "0998989222", "", "", null, "AQAAAAEAACcQAAAAEMG7lLonmmZRagUy6XJUa5VGtJdxzernrq2l60+befqZ7p7LMbjEVZChN1wIjEZlQQ==", "", "", null, "", "", "MAFWDQPQCHDE74JJBFCYYPQNXMIEPWKM", "", null, "", "", "", new DateTime(2020, 9, 1, 1, 31, 37, 0, DateTimeKind.Unspecified), "NVKD01" },
                    { 1, "", "administrator@gmail.com", "00000000-0000-0000-0000-000000000000", null, null, "", "", "ADMINISTRATOR", "", new DateTime(2020, 9, 10, 4, 9, 30, 0, DateTimeKind.Unspecified), "", null, 0, "", "", "ADMIN", "ADMIN", 1, "", "47d3bbde-bc70-4af4-9afc-cb197cea37f0", true, false, false, false, false, false, false, "", "", null, "ADMIN", "0972998868", "", "", null, "AQAAAAEAACcQAAAAEPUj7VKlsHGKJ7KespHZxw5HLr65bH/XCKGJYWOdOptRQ/VXW3ouj3TxP1WGF1Y0Ng==", "", "", null, "", "", "ZBKVRES4PZNBEB3NMFPTPFAPVVAKSXKS", "", null, "", "", "", null, "ADMINISTRATOR" },
                    { 6, "", "Hà Nội", "", null, null, "", "", "", "", new DateTime(2020, 9, 1, 2, 42, 3, 0, DateTimeKind.Unspecified), "", null, 0, "nam@htc-itc.com.vn", "", "Nguyễn Đăng", "Nguyễn Đăng Nam", null, "0129448822", "afb91c64-e66c-4780-b099-38791cc73630", true, false, false, false, false, false, false, "", "", null, "Nam", "0339848822", "", "", null, "AQAAAAEAACcQAAAAEAQXuLBMt8t3oY3ObIfkvLAa1Vs2U9cMV6LTQWrKAZz6Tit/AYY2bk2s2+ZXiRicMA==", "", "", null, "", "", "NHRXIWAXIKIT4SHX7SI5FX4W6Z444JD2", "", null, "", "", "", null, "NVKD0001" },
                    { 3, "", "Thủy Triều - Thủy Nguyên - Hải Phòng", "", null, null, "", "", "BauTN", "", new DateTime(2020, 8, 31, 14, 0, 4, 0, DateTimeKind.Unspecified), "", null, 0, "bautran1911@gmail.com", "", "", "Báu Trần", null, "031843257", "e5df9668-38e2-4a65-a549-6bfb5273e8c3", true, false, false, false, false, false, true, "", "", null, "", "0582251486", "", "", null, "", "", "", null, "", "", "ZLDOCF25W3GQP5N2EFCOVOUAK5JAK624", "", null, "", "", "", null, "" }
                });

            migrationBuilder.InsertData(
                table: "ConfigurationPersonalAccounts",
                columns: new[] { "Id", "AllowSendEmail", "AllowSendNotification", "AllowSendSMS", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 1 },
                    { 15, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 15 },
                    { 14, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 14 },
                    { 13, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 13 },
                    { 11, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 11 },
                    { 10, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 10 },
                    { 9, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 9 },
                    { 12, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 12 },
                    { 7, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 7 },
                    { 6, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 6 },
                    { 5, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 5 },
                    { 4, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 4 },
                    { 3, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 3 },
                    { 8, true, true, true, "system", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", "", null, 8 }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Deny", "DisplayOrder", "Grant", "IsActive", "IsDeleted", "OrganizationPath", "PermissionId", "RoleId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 514, "", new DateTime(2020, 9, 11, 2, 33, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 194, 2, "", null },
                    { 509, "", new DateTime(2020, 9, 11, 2, 32, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 150, 2, "", null },
                    { 510, "", new DateTime(2020, 9, 11, 2, 32, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 151, 2, "", null },
                    { 511, "", new DateTime(2020, 9, 11, 2, 33, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 156, 2, "", null },
                    { 512, "", new DateTime(2020, 9, 11, 2, 33, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 157, 2, "", null },
                    { 513, "", new DateTime(2020, 9, 11, 2, 33, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 162, 2, "", null },
                    { 515, "", new DateTime(2020, 9, 11, 2, 33, 50, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 200, 2, "", null },
                    { 521, "", new DateTime(2020, 9, 11, 2, 37, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 175, 3, "", null },
                    { 517, "", new DateTime(2020, 9, 11, 2, 33, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 204, 2, "", null },
                    { 518, "", new DateTime(2020, 9, 11, 2, 34, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 206, 2, "", null },
                    { 519, "", new DateTime(2020, 9, 11, 2, 34, 11, 0, DateTimeKind.Unspecified), "", false, 0, false, false, false, "", 210, 2, "", new DateTime(2020, 9, 11, 2, 34, 15, 0, DateTimeKind.Unspecified) },
                    { 681, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 440, 3, "", null },
                    { 520, "", new DateTime(2020, 9, 11, 2, 37, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 174, 3, "", null },
                    { 508, "", new DateTime(2020, 9, 11, 2, 32, 45, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 149, 2, "", null },
                    { 516, "", new DateTime(2020, 9, 11, 2, 33, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 203, 2, "", null },
                    { 507, "", new DateTime(2020, 9, 11, 2, 32, 44, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 148, 2, "", null },
                    { 500, "", new DateTime(2020, 9, 11, 2, 32, 33, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 140, 2, "", null },
                    { 505, "", new DateTime(2020, 9, 11, 2, 32, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 145, 2, "", null },
                    { 490, "", new DateTime(2020, 9, 11, 2, 32, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 41, 2, "", null },
                    { 491, "", new DateTime(2020, 9, 11, 2, 32, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 42, 2, "", null },
                    { 492, "", new DateTime(2020, 9, 11, 2, 32, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 43, 2, "", null },
                    { 493, "", new DateTime(2020, 9, 11, 2, 32, 11, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 44, 2, "", null },
                    { 494, "", new DateTime(2020, 9, 11, 2, 32, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 56, 2, "", null },
                    { 495, "", new DateTime(2020, 9, 11, 2, 32, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 57, 2, "", null },
                    { 496, "", new DateTime(2020, 9, 11, 2, 32, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 58, 2, "", null },
                    { 497, "", new DateTime(2020, 9, 11, 2, 32, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 59, 2, "", null },
                    { 498, "", new DateTime(2020, 9, 11, 2, 32, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 60, 2, "", null },
                    { 499, "", new DateTime(2020, 9, 11, 2, 32, 26, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 61, 2, "", null },
                    { 522, "", new DateTime(2020, 9, 11, 2, 37, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 168, 3, "", null },
                    { 501, "", new DateTime(2020, 9, 11, 2, 32, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 141, 2, "", null },
                    { 502, "", new DateTime(2020, 9, 11, 2, 32, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 142, 2, "", null },
                    { 503, "", new DateTime(2020, 9, 11, 2, 32, 35, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 143, 2, "", null },
                    { 504, "", new DateTime(2020, 9, 11, 2, 32, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 144, 2, "", null },
                    { 506, "", new DateTime(2020, 9, 11, 2, 32, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 147, 2, "", null },
                    { 523, "", new DateTime(2020, 9, 11, 2, 37, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 169, 3, "", null },
                    { 530, "", new DateTime(2020, 9, 11, 2, 37, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 1, 3, "", null },
                    { 525, "", new DateTime(2020, 9, 11, 2, 37, 41, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 18, 3, "", null },
                    { 545, "", new DateTime(2020, 9, 11, 2, 38, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 16, 3, "", null },
                    { 546, "", new DateTime(2020, 9, 11, 2, 38, 27, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 23, 3, "", null },
                    { 547, "", new DateTime(2020, 9, 11, 2, 38, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 24, 3, "", null },
                    { 548, "", new DateTime(2020, 9, 11, 2, 38, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 25, 3, "", null },
                    { 549, "", new DateTime(2020, 9, 11, 2, 38, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 26, 3, "", null },
                    { 550, "", new DateTime(2020, 9, 11, 2, 38, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 27, 3, "", null },
                    { 544, "", new DateTime(2020, 9, 11, 2, 38, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 15, 3, "", null },
                    { 551, "", new DateTime(2020, 9, 11, 2, 38, 33, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 28, 3, "", null },
                    { 553, "", new DateTime(2020, 9, 11, 2, 38, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 30, 3, "", null },
                    { 554, "", new DateTime(2020, 9, 11, 2, 38, 35, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 31, 3, "", null },
                    { 555, "", new DateTime(2020, 9, 11, 2, 38, 36, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 32, 3, "", null },
                    { 556, "", new DateTime(2020, 9, 11, 2, 38, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 62, 3, "", null },
                    { 557, "", new DateTime(2020, 9, 11, 2, 38, 47, 0, DateTimeKind.Unspecified), "", false, 0, false, false, false, "", 64, 3, "", new DateTime(2020, 9, 11, 2, 38, 48, 0, DateTimeKind.Unspecified) },
                    { 558, "", new DateTime(2020, 9, 11, 2, 38, 49, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 66, 3, "", null },
                    { 552, "", new DateTime(2020, 9, 11, 2, 38, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 29, 3, "", null },
                    { 543, "", new DateTime(2020, 9, 11, 2, 38, 9, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 14, 3, "", null },
                    { 542, "", new DateTime(2020, 9, 11, 2, 38, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 13, 3, "", null },
                    { 541, "", new DateTime(2020, 9, 11, 2, 38, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 12, 3, "", null },
                    { 526, "", new DateTime(2020, 9, 11, 2, 37, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 19, 3, "", null },
                    { 527, "", new DateTime(2020, 9, 11, 2, 37, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 20, 3, "", null },
                    { 528, "", new DateTime(2020, 9, 11, 2, 37, 44, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 21, 3, "", null },
                    { 529, "", new DateTime(2020, 9, 11, 2, 37, 44, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 22, 3, "", null },
                    { 489, "", new DateTime(2020, 9, 11, 2, 32, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 40, 2, "", null },
                    { 531, "", new DateTime(2020, 9, 11, 2, 37, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 2, 3, "", null },
                    { 532, "", new DateTime(2020, 9, 11, 2, 37, 58, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 3, 3, "", null },
                    { 533, "", new DateTime(2020, 9, 11, 2, 37, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 4, 3, "", null },
                    { 534, "", new DateTime(2020, 9, 11, 2, 38, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 5, 3, "", null },
                    { 535, "", new DateTime(2020, 9, 11, 2, 38, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 6, 3, "", null },
                    { 536, "", new DateTime(2020, 9, 11, 2, 38, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 7, 3, "", null },
                    { 537, "", new DateTime(2020, 9, 11, 2, 38, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 8, 3, "", null },
                    { 538, "", new DateTime(2020, 9, 11, 2, 38, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 9, 3, "", null },
                    { 539, "", new DateTime(2020, 9, 11, 2, 38, 5, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 10, 3, "", null },
                    { 540, "", new DateTime(2020, 9, 11, 2, 38, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 11, 3, "", null },
                    { 524, "", new DateTime(2020, 9, 11, 2, 37, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 17, 3, "", null },
                    { 488, "", new DateTime(2020, 9, 11, 2, 32, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 39, 2, "", null },
                    { 481, "", new DateTime(2020, 9, 11, 2, 31, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 51, 2, "", null },
                    { 486, "", new DateTime(2020, 9, 11, 2, 32, 5, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 37, 2, "", null },
                    { 435, "", new DateTime(2020, 9, 11, 2, 28, 13, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 86, 2, "", null },
                    { 436, "", new DateTime(2020, 9, 11, 2, 28, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 90, 2, "", null },
                    { 437, "", new DateTime(2020, 9, 11, 2, 28, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 88, 2, "", null },
                    { 438, "", new DateTime(2020, 9, 11, 2, 28, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 89, 2, "", null },
                    { 439, "", new DateTime(2020, 9, 11, 2, 28, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 92, 2, "", null },
                    { 440, "", new DateTime(2020, 9, 11, 2, 28, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 94, 2, "", null },
                    { 434, "", new DateTime(2020, 9, 11, 2, 27, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 84, 2, "", null },
                    { 441, "", new DateTime(2020, 9, 11, 2, 28, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 95, 2, "", null },
                    { 443, "", new DateTime(2020, 9, 11, 2, 28, 33, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 97, 2, "", null },
                    { 444, "", new DateTime(2020, 9, 11, 2, 28, 36, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 91, 2, "", null },
                    { 445, "", new DateTime(2020, 9, 11, 2, 28, 41, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 98, 2, "", null },
                    { 446, "", new DateTime(2020, 9, 11, 2, 28, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 99, 2, "", null },
                    { 447, "", new DateTime(2020, 9, 11, 2, 28, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 100, 2, "", null },
                    { 448, "", new DateTime(2020, 9, 11, 2, 28, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 102, 2, "", null },
                    { 442, "", new DateTime(2020, 9, 11, 2, 28, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 96, 2, "", null },
                    { 433, "", new DateTime(2020, 9, 11, 2, 27, 44, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 80, 2, "", null },
                    { 432, "", new DateTime(2020, 9, 11, 2, 27, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 79, 2, "", null },
                    { 431, "", new DateTime(2020, 9, 11, 2, 27, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 73, 2, "", null },
                    { 416, "", new DateTime(2020, 9, 11, 2, 26, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 13, 2, "", null },
                    { 417, "", new DateTime(2020, 9, 11, 2, 26, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 14, 2, "", null },
                    { 418, "", new DateTime(2020, 9, 11, 2, 26, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 15, 2, "", null },
                    { 419, "", new DateTime(2020, 9, 11, 2, 26, 26, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 16, 2, "", null },
                    { 420, "", new DateTime(2020, 9, 11, 2, 26, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 23, 2, "", null },
                    { 421, "", new DateTime(2020, 9, 11, 2, 26, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 24, 2, "", null },
                    { 422, "", new DateTime(2020, 9, 11, 2, 26, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 25, 2, "", null },
                    { 423, "", new DateTime(2020, 9, 11, 2, 26, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 26, 2, "", null },
                    { 424, "", new DateTime(2020, 9, 11, 2, 26, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 27, 2, "", null },
                    { 425, "", new DateTime(2020, 9, 11, 2, 26, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 28, 2, "", null },
                    { 426, "", new DateTime(2020, 9, 11, 2, 26, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 29, 2, "", null },
                    { 427, "", new DateTime(2020, 9, 11, 2, 26, 58, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 30, 2, "", null },
                    { 428, "", new DateTime(2020, 9, 11, 2, 26, 58, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 31, 2, "", null },
                    { 429, "", new DateTime(2020, 9, 11, 2, 26, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 32, 2, "", null },
                    { 430, "", new DateTime(2020, 9, 11, 2, 27, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 67, 2, "", null },
                    { 449, "", new DateTime(2020, 9, 11, 2, 28, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 103, 2, "", null },
                    { 487, "", new DateTime(2020, 9, 11, 2, 32, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 38, 2, "", null },
                    { 450, "", new DateTime(2020, 9, 11, 2, 28, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 105, 2, "", null },
                    { 452, "", new DateTime(2020, 9, 11, 2, 28, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 108, 2, "", null },
                    { 472, "", new DateTime(2020, 9, 11, 2, 31, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 135, 2, "", null },
                    { 473, "", new DateTime(2020, 9, 11, 2, 31, 35, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 137, 2, "", null },
                    { 474, "", new DateTime(2020, 9, 11, 2, 31, 36, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 138, 2, "", null },
                    { 475, "", new DateTime(2020, 9, 11, 2, 31, 37, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 139, 2, "", null },
                    { 476, "", new DateTime(2020, 9, 11, 2, 31, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 46, 2, "", null },
                    { 477, "", new DateTime(2020, 9, 11, 2, 31, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 47, 2, "", null },
                    { 471, "", new DateTime(2020, 9, 11, 2, 31, 33, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 134, 2, "", null },
                    { 478, "", new DateTime(2020, 9, 11, 2, 31, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 48, 2, "", null },
                    { 480, "", new DateTime(2020, 9, 11, 2, 31, 50, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 50, 2, "", null },
                    { 559, "", new DateTime(2020, 9, 11, 2, 38, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 67, 3, "", null },
                    { 482, "", new DateTime(2020, 9, 11, 2, 31, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 52, 2, "", null },
                    { 483, "", new DateTime(2020, 9, 11, 2, 31, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 53, 2, "", null },
                    { 484, "", new DateTime(2020, 9, 11, 2, 31, 53, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 54, 2, "", null },
                    { 485, "", new DateTime(2020, 9, 11, 2, 31, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 45, 2, "", null },
                    { 479, "", new DateTime(2020, 9, 11, 2, 31, 49, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 49, 2, "", null },
                    { 470, "", new DateTime(2020, 9, 11, 2, 31, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 129, 2, "", null },
                    { 469, "", new DateTime(2020, 9, 11, 2, 31, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 130, 2, "", null },
                    { 468, "", new DateTime(2020, 9, 11, 2, 31, 21, 0, DateTimeKind.Unspecified), "", false, 0, false, false, false, "", 128, 2, "", new DateTime(2020, 9, 11, 2, 31, 23, 0, DateTimeKind.Unspecified) },
                    { 453, "", new DateTime(2020, 9, 11, 2, 29, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 109, 2, "", null },
                    { 454, "", new DateTime(2020, 9, 11, 2, 30, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 110, 2, "", null },
                    { 455, "", new DateTime(2020, 9, 11, 2, 30, 41, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 111, 2, "", null },
                    { 456, "", new DateTime(2020, 9, 11, 2, 30, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 112, 2, "", null },
                    { 457, "", new DateTime(2020, 9, 11, 2, 30, 45, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 114, 2, "", null },
                    { 458, "", new DateTime(2020, 9, 11, 2, 30, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 115, 2, "", null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Deny", "DisplayOrder", "Grant", "IsActive", "IsDeleted", "OrganizationPath", "PermissionId", "RoleId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 459, "", new DateTime(2020, 9, 11, 2, 30, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 117, 2, "", null },
                    { 460, "", new DateTime(2020, 9, 11, 2, 30, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 118, 2, "", null },
                    { 461, "", new DateTime(2020, 9, 11, 2, 30, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 119, 2, "", null },
                    { 462, "", new DateTime(2020, 9, 11, 2, 30, 58, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 121, 2, "", null },
                    { 463, "", new DateTime(2020, 9, 11, 2, 30, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 122, 2, "", null },
                    { 464, "", new DateTime(2020, 9, 11, 2, 31, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 123, 2, "", null },
                    { 465, "", new DateTime(2020, 9, 11, 2, 31, 9, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 125, 2, "", null },
                    { 466, "", new DateTime(2020, 9, 11, 2, 31, 9, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 126, 2, "", null },
                    { 467, "", new DateTime(2020, 9, 11, 2, 31, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 127, 2, "", null },
                    { 451, "", new DateTime(2020, 9, 11, 2, 28, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 106, 2, "", null },
                    { 560, "", new DateTime(2020, 9, 11, 2, 39, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 68, 3, "", null },
                    { 567, "", new DateTime(2020, 9, 11, 2, 39, 44, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 77, 3, "", null },
                    { 562, "", new DateTime(2020, 9, 11, 2, 39, 15, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 71, 3, "", null },
                    { 343, "", new DateTime(2020, 9, 11, 2, 13, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 1, 6, "", null },
                    { 344, "", new DateTime(2020, 9, 11, 2, 13, 37, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 12, 6, "", null },
                    { 345, "", new DateTime(2020, 9, 11, 2, 13, 38, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 13, 6, "", null },
                    { 346, "", new DateTime(2020, 9, 11, 2, 13, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 17, 6, "", null },
                    { 347, "", new DateTime(2020, 9, 11, 2, 14, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 21, 6, "", null },
                    { 348, "", new DateTime(2020, 9, 11, 2, 14, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 22, 6, "", null },
                    { 324, "", new DateTime(2020, 9, 11, 1, 36, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 174, 6, "", new DateTime(2020, 9, 11, 2, 24, 2, 0, DateTimeKind.Unspecified) },
                    { 349, "", new DateTime(2020, 9, 11, 2, 16, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 23, 6, "", null },
                    { 351, "", new DateTime(2020, 9, 11, 2, 16, 26, 0, DateTimeKind.Unspecified), "", false, 0, false, false, false, "", 64, 6, "", new DateTime(2020, 9, 11, 2, 16, 27, 0, DateTimeKind.Unspecified) },
                    { 352, "", new DateTime(2020, 9, 11, 2, 16, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 73, 6, "", null },
                    { 353, "", new DateTime(2020, 9, 11, 2, 16, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 79, 6, "", null },
                    { 354, "", new DateTime(2020, 9, 11, 2, 16, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 85, 6, "", null },
                    { 355, "", new DateTime(2020, 9, 11, 2, 16, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 91, 6, "", null },
                    { 356, "", new DateTime(2020, 9, 11, 2, 16, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 97, 6, "", null },
                    { 350, "", new DateTime(2020, 9, 11, 2, 16, 11, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 67, 6, "", null },
                    { 342, "", new DateTime(2020, 9, 11, 2, 5, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 194, 5, "", null },
                    { 341, "", new DateTime(2020, 9, 11, 2, 5, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 206, 5, "", null },
                    { 340, "", new DateTime(2020, 9, 11, 2, 5, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 175, 5, "", null },
                    { 325, "", new DateTime(2020, 9, 11, 1, 55, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 1, 5, "", null },
                    { 326, "", new DateTime(2020, 9, 11, 1, 55, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 12, 5, "", null },
                    { 327, "", new DateTime(2020, 9, 11, 1, 55, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 13, 5, "", null },
                    { 328, "", new DateTime(2020, 9, 11, 1, 56, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 17, 5, "", null },
                    { 329, "", new DateTime(2020, 9, 11, 1, 57, 35, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 23, 5, "", null },
                    { 330, "", new DateTime(2020, 9, 11, 2, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 80, 5, "", null },
                    { 331, "", new DateTime(2020, 9, 11, 2, 0, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 84, 5, "", null },
                    { 332, "", new DateTime(2020, 9, 11, 2, 0, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 85, 5, "", null },
                    { 333, "", new DateTime(2020, 9, 11, 2, 1, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 140, 5, "", null },
                    { 334, "", new DateTime(2020, 9, 11, 2, 2, 37, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 144, 5, "", null },
                    { 335, "", new DateTime(2020, 9, 11, 2, 3, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 150, 5, "", null },
                    { 336, "", new DateTime(2020, 9, 11, 2, 3, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 162, 5, "", null },
                    { 337, "", new DateTime(2020, 9, 11, 2, 5, 15, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 168, 5, "", null },
                    { 338, "", new DateTime(2020, 9, 11, 2, 5, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 169, 5, "", null },
                    { 339, "", new DateTime(2020, 9, 11, 2, 5, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 174, 5, "", null },
                    { 357, "", new DateTime(2020, 9, 11, 2, 17, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 103, 6, "", null },
                    { 674, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 435, 5, "", null },
                    { 358, "", new DateTime(2020, 9, 11, 2, 17, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 99, 6, "", null },
                    { 360, "", new DateTime(2020, 9, 11, 2, 17, 28, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 109, 6, "", null },
                    { 380, "", new DateTime(2020, 9, 11, 2, 21, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 57, 6, "", null },
                    { 381, "", new DateTime(2020, 9, 11, 2, 22, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 60, 6, "", null },
                    { 382, "", new DateTime(2020, 9, 11, 2, 22, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 140, 6, "", null },
                    { 383, "", new DateTime(2020, 9, 11, 2, 22, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 144, 6, "", null },
                    { 384, "", new DateTime(2020, 9, 11, 2, 22, 38, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 150, 6, "", null },
                    { 385, "", new DateTime(2020, 9, 11, 2, 22, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 156, 6, "", null },
                    { 379, "", new DateTime(2020, 9, 11, 2, 21, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 56, 6, "", null },
                    { 386, "", new DateTime(2020, 9, 11, 2, 22, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 157, 6, "", null },
                    { 388, "", new DateTime(2020, 9, 11, 2, 23, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 194, 6, "", null },
                    { 389, "", new DateTime(2020, 9, 11, 2, 23, 37, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 200, 6, "", null },
                    { 390, "", new DateTime(2020, 9, 11, 2, 23, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 206, 6, "", null },
                    { 391, "", new DateTime(2020, 9, 11, 2, 23, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 168, 6, "", null },
                    { 392, "", new DateTime(2020, 9, 11, 2, 23, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 169, 6, "", null },
                    { 393, "", new DateTime(2020, 9, 11, 2, 24, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 175, 6, "", null },
                    { 387, "", new DateTime(2020, 9, 11, 2, 23, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 162, 6, "", null },
                    { 378, "", new DateTime(2020, 9, 11, 2, 21, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 43, 6, "", null },
                    { 377, "", new DateTime(2020, 9, 11, 2, 21, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 42, 6, "", null },
                    { 376, "", new DateTime(2020, 9, 11, 2, 21, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 41, 6, "", null },
                    { 361, "", new DateTime(2020, 9, 11, 2, 17, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 105, 6, "", null },
                    { 362, "", new DateTime(2020, 9, 11, 2, 17, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 106, 6, "", null },
                    { 363, "", new DateTime(2020, 9, 11, 2, 17, 37, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 111, 6, "", null },
                    { 364, "", new DateTime(2020, 9, 11, 2, 17, 38, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 112, 6, "", null },
                    { 365, "", new DateTime(2020, 9, 11, 2, 17, 41, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 115, 6, "", null },
                    { 366, "", new DateTime(2020, 9, 11, 2, 17, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 122, 6, "", null },
                    { 367, "", new DateTime(2020, 9, 11, 2, 17, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 118, 6, "", null },
                    { 368, "", new DateTime(2020, 9, 11, 2, 17, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 119, 6, "", null },
                    { 369, "", new DateTime(2020, 9, 11, 2, 19, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 126, 6, "", null },
                    { 370, "", new DateTime(2020, 9, 11, 2, 19, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 130, 6, "", null },
                    { 371, "", new DateTime(2020, 9, 11, 2, 20, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 46, 6, "", null },
                    { 372, "", new DateTime(2020, 9, 11, 2, 20, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 49, 6, "", null },
                    { 373, "", new DateTime(2020, 9, 11, 2, 20, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 50, 6, "", null },
                    { 374, "", new DateTime(2020, 9, 11, 2, 20, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 51, 6, "", null },
                    { 375, "", new DateTime(2020, 9, 11, 2, 21, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 40, 6, "", null },
                    { 359, "", new DateTime(2020, 9, 11, 2, 17, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 100, 6, "", null },
                    { 649, "", new DateTime(2020, 10, 28, 2, 5, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 338, 3, "", null },
                    { 635, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 206, 3, "", null },
                    { 634, "", new DateTime(2020, 9, 11, 2, 44, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 204, 3, "", null },
                    { 582, "", new DateTime(2020, 9, 11, 2, 40, 28, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 95, 3, "", null },
                    { 583, "", new DateTime(2020, 9, 11, 2, 40, 28, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 96, 3, "", null },
                    { 584, "", new DateTime(2020, 9, 11, 2, 40, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 97, 3, "", null },
                    { 585, "", new DateTime(2020, 9, 11, 2, 40, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 98, 3, "", null },
                    { 586, "", new DateTime(2020, 9, 11, 2, 40, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 99, 3, "", null },
                    { 587, "", new DateTime(2020, 9, 11, 2, 40, 50, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 100, 3, "", null },
                    { 581, "", new DateTime(2020, 9, 11, 2, 40, 27, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 94, 3, "", null },
                    { 588, "", new DateTime(2020, 9, 11, 2, 40, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 102, 3, "", null },
                    { 590, "", new DateTime(2020, 9, 11, 2, 41, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 109, 3, "", null },
                    { 591, "", new DateTime(2020, 9, 11, 2, 41, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 108, 3, "", null },
                    { 592, "", new DateTime(2020, 9, 11, 2, 41, 9, 0, DateTimeKind.Unspecified), "", false, 0, false, false, false, "", 107, 3, "", new DateTime(2020, 9, 11, 2, 41, 9, 0, DateTimeKind.Unspecified) },
                    { 593, "", new DateTime(2020, 9, 11, 2, 41, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 106, 3, "", null },
                    { 594, "", new DateTime(2020, 9, 11, 2, 41, 11, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 105, 3, "", null },
                    { 595, "", new DateTime(2020, 9, 11, 2, 41, 28, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 110, 3, "", null },
                    { 589, "", new DateTime(2020, 9, 11, 2, 40, 53, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 103, 3, "", null },
                    { 580, "", new DateTime(2020, 9, 11, 2, 40, 26, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 92, 3, "", null },
                    { 579, "", new DateTime(2020, 9, 11, 2, 40, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 91, 3, "", null },
                    { 578, "", new DateTime(2020, 9, 11, 2, 40, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 90, 3, "", null },
                    { 563, "", new DateTime(2020, 9, 11, 2, 39, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 73, 3, "", null },
                    { 564, "", new DateTime(2020, 9, 11, 2, 39, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 72, 3, "", null },
                    { 565, "", new DateTime(2020, 9, 11, 2, 39, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 74, 3, "", null },
                    { 566, "", new DateTime(2020, 9, 11, 2, 39, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 76, 3, "", null },
                    { 415, "", new DateTime(2020, 9, 11, 2, 26, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 12, 2, "", null },
                    { 568, "", new DateTime(2020, 9, 11, 2, 39, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 78, 3, "", null },
                    { 569, "", new DateTime(2020, 9, 11, 2, 39, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 79, 3, "", null },
                    { 570, "", new DateTime(2020, 9, 11, 2, 39, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 80, 3, "", null },
                    { 571, "", new DateTime(2020, 9, 11, 2, 39, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 82, 3, "", null },
                    { 572, "", new DateTime(2020, 9, 11, 2, 40, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 83, 3, "", null },
                    { 573, "", new DateTime(2020, 9, 11, 2, 40, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 84, 3, "", null },
                    { 574, "", new DateTime(2020, 9, 11, 2, 40, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 85, 3, "", null },
                    { 575, "", new DateTime(2020, 9, 11, 2, 40, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 86, 3, "", null },
                    { 576, "", new DateTime(2020, 9, 11, 2, 40, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 88, 3, "", null },
                    { 577, "", new DateTime(2020, 9, 11, 2, 40, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 89, 3, "", null },
                    { 596, "", new DateTime(2020, 9, 11, 2, 41, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 111, 3, "", null },
                    { 597, "", new DateTime(2020, 9, 11, 2, 41, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 112, 3, "", null },
                    { 598, "", new DateTime(2020, 9, 11, 2, 41, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 114, 3, "", null },
                    { 599, "", new DateTime(2020, 9, 11, 2, 41, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 115, 3, "", null },
                    { 619, "", new DateTime(2020, 9, 11, 2, 43, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 141, 3, "", null },
                    { 620, "", new DateTime(2020, 9, 11, 2, 43, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 142, 3, "", null },
                    { 621, "", new DateTime(2020, 9, 11, 2, 43, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 143, 3, "", null },
                    { 622, "", new DateTime(2020, 9, 11, 2, 43, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 144, 3, "", null },
                    { 623, "", new DateTime(2020, 9, 11, 2, 43, 27, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 145, 3, "", null },
                    { 624, "", new DateTime(2020, 9, 11, 2, 43, 28, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 147, 3, "", null },
                    { 625, "", new DateTime(2020, 9, 11, 2, 43, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 148, 3, "", null },
                    { 626, "", new DateTime(2020, 9, 11, 2, 43, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 149, 3, "", null },
                    { 627, "", new DateTime(2020, 9, 11, 2, 43, 41, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 150, 3, "", null },
                    { 628, "", new DateTime(2020, 9, 11, 2, 43, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 151, 3, "", null },
                    { 629, "", new DateTime(2020, 9, 11, 2, 43, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 156, 3, "", null },
                    { 630, "", new DateTime(2020, 9, 11, 2, 44, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 162, 3, "", null },
                    { 631, "", new DateTime(2020, 9, 11, 2, 44, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 194, 3, "", null },
                    { 632, "", new DateTime(2020, 9, 11, 2, 44, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 200, 3, "", null },
                    { 633, "", new DateTime(2020, 9, 11, 2, 44, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 203, 3, "", null },
                    { 618, "", new DateTime(2020, 9, 11, 2, 43, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 140, 3, "", null },
                    { 561, "", new DateTime(2020, 9, 11, 2, 39, 14, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 70, 3, "", null },
                    { 617, "", new DateTime(2020, 9, 11, 2, 42, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 43, 3, "", null },
                    { 615, "", new DateTime(2020, 9, 11, 2, 42, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 139, 3, "", null },
                    { 600, "", new DateTime(2020, 9, 11, 2, 41, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 117, 3, "", null },
                    { 601, "", new DateTime(2020, 9, 11, 2, 41, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 118, 3, "", null },
                    { 602, "", new DateTime(2020, 9, 11, 2, 41, 44, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 119, 3, "", null },
                    { 603, "", new DateTime(2020, 9, 11, 2, 41, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 121, 3, "", null },
                    { 604, "", new DateTime(2020, 9, 11, 2, 41, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 122, 3, "", null },
                    { 605, "", new DateTime(2020, 9, 11, 2, 41, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 123, 3, "", null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Deny", "DisplayOrder", "Grant", "IsActive", "IsDeleted", "OrganizationPath", "PermissionId", "RoleId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 606, "", new DateTime(2020, 9, 11, 2, 41, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 125, 3, "", null },
                    { 607, "", new DateTime(2020, 9, 11, 2, 41, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 126, 3, "", null },
                    { 608, "", new DateTime(2020, 9, 11, 2, 42, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 127, 3, "", null },
                    { 609, "", new DateTime(2020, 9, 11, 2, 42, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 129, 3, "", null },
                    { 610, "", new DateTime(2020, 9, 11, 2, 42, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 130, 3, "", null },
                    { 611, "", new DateTime(2020, 9, 11, 2, 42, 13, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 134, 3, "", null },
                    { 612, "", new DateTime(2020, 9, 11, 2, 42, 13, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 135, 3, "", null },
                    { 613, "", new DateTime(2020, 9, 11, 2, 42, 15, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 137, 3, "", null },
                    { 614, "", new DateTime(2020, 9, 11, 2, 42, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 138, 3, "", null },
                    { 616, "", new DateTime(2020, 9, 11, 2, 42, 37, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 51, 3, "", null },
                    { 414, "", new DateTime(2020, 9, 11, 2, 26, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 11, 2, "", null },
                    { 409, "", new DateTime(2020, 9, 11, 2, 26, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 6, 2, "", null },
                    { 412, "", new DateTime(2020, 9, 11, 2, 26, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 9, 2, "", null },
                    { 95, "", new DateTime(2020, 9, 10, 4, 16, 53, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 150, 999, "", null },
                    { 96, "", new DateTime(2020, 9, 10, 4, 16, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 151, 999, "", null },
                    { 97, "", new DateTime(2020, 9, 10, 4, 16, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 152, 999, "", null },
                    { 98, "", new DateTime(2020, 9, 10, 4, 16, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 153, 999, "", null },
                    { 99, "", new DateTime(2020, 9, 10, 4, 16, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 154, 999, "", null },
                    { 100, "", new DateTime(2020, 9, 10, 4, 16, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 155, 999, "", null },
                    { 94, "", new DateTime(2020, 9, 10, 4, 16, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 143, 999, "", null },
                    { 101, "", new DateTime(2020, 9, 10, 4, 17, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 134, 999, "", null },
                    { 103, "", new DateTime(2020, 9, 10, 4, 17, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 136, 999, "", null },
                    { 104, "", new DateTime(2020, 9, 10, 4, 17, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 137, 999, "", null },
                    { 105, "", new DateTime(2020, 9, 10, 4, 17, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 138, 999, "", null },
                    { 106, "", new DateTime(2020, 9, 10, 4, 17, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 139, 999, "", null },
                    { 107, "", new DateTime(2020, 9, 10, 4, 17, 5, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 200, 999, "", null },
                    { 108, "", new DateTime(2020, 9, 10, 4, 17, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 201, 999, "", null },
                    { 102, "", new DateTime(2020, 9, 10, 4, 17, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 135, 999, "", null },
                    { 93, "", new DateTime(2020, 9, 10, 4, 16, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 142, 999, "", null },
                    { 92, "", new DateTime(2020, 9, 10, 4, 16, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 141, 999, "", null },
                    { 91, "", new DateTime(2020, 9, 10, 4, 16, 50, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 140, 999, "", null },
                    { 76, "", new DateTime(2020, 9, 10, 4, 16, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 65, 999, "", null },
                    { 77, "", new DateTime(2020, 9, 10, 4, 16, 33, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 66, 999, "", null },
                    { 78, "", new DateTime(2020, 9, 10, 4, 16, 38, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 67, 999, "", null },
                    { 79, "", new DateTime(2020, 9, 10, 4, 16, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 92, 999, "", null },
                    { 80, "", new DateTime(2020, 9, 10, 4, 16, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 93, 999, "", null },
                    { 81, "", new DateTime(2020, 9, 10, 4, 16, 41, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 94, 999, "", null },
                    { 82, "", new DateTime(2020, 9, 10, 4, 16, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 95, 999, "", null },
                    { 83, "", new DateTime(2020, 9, 10, 4, 16, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 96, 999, "", null },
                    { 84, "", new DateTime(2020, 9, 10, 4, 16, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 97, 999, "", null },
                    { 85, "", new DateTime(2020, 9, 10, 4, 16, 45, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 86, 999, "", null },
                    { 86, "", new DateTime(2020, 9, 10, 4, 16, 45, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 87, 999, "", null },
                    { 87, "", new DateTime(2020, 9, 10, 4, 16, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 88, 999, "", null },
                    { 88, "", new DateTime(2020, 9, 10, 4, 16, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 89, 999, "", null },
                    { 89, "", new DateTime(2020, 9, 10, 4, 16, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 90, 999, "", null },
                    { 90, "", new DateTime(2020, 9, 10, 4, 16, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 91, 999, "", null },
                    { 109, "", new DateTime(2020, 9, 10, 4, 17, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 202, 999, "", null },
                    { 75, "", new DateTime(2020, 9, 10, 4, 16, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 64, 999, "", null },
                    { 110, "", new DateTime(2020, 9, 10, 4, 17, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 203, 999, "", null },
                    { 112, "", new DateTime(2020, 9, 10, 4, 17, 9, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 205, 999, "", null },
                    { 132, "", new DateTime(2020, 9, 10, 4, 17, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 46, 999, "", null },
                    { 133, "", new DateTime(2020, 9, 10, 4, 17, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 47, 999, "", null },
                    { 134, "", new DateTime(2020, 9, 10, 4, 17, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 48, 999, "", null },
                    { 135, "", new DateTime(2020, 9, 10, 4, 17, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 49, 999, "", null },
                    { 136, "", new DateTime(2020, 9, 10, 4, 17, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 50, 999, "", null },
                    { 137, "", new DateTime(2020, 9, 10, 4, 17, 43, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 51, 999, "", null },
                    { 131, "", new DateTime(2020, 9, 10, 4, 17, 27, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 45, 999, "", null },
                    { 138, "", new DateTime(2020, 9, 10, 4, 17, 44, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 52, 999, "", null },
                    { 140, "", new DateTime(2020, 9, 10, 4, 17, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 54, 999, "", null },
                    { 141, "", new DateTime(2020, 9, 10, 4, 17, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 55, 999, "", null },
                    { 142, "", new DateTime(2020, 9, 10, 4, 17, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 133, 999, "", null },
                    { 143, "", new DateTime(2020, 9, 10, 4, 17, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 37, 999, "", null },
                    { 144, "", new DateTime(2020, 9, 10, 4, 17, 49, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 38, 999, "", null },
                    { 145, "", new DateTime(2020, 9, 10, 4, 17, 50, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 39, 999, "", null },
                    { 139, "", new DateTime(2020, 9, 10, 4, 17, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 53, 999, "", null },
                    { 130, "", new DateTime(2020, 9, 10, 4, 17, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 85, 999, "", null },
                    { 129, "", new DateTime(2020, 9, 10, 4, 17, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 84, 999, "", null },
                    { 128, "", new DateTime(2020, 9, 10, 4, 17, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 83, 999, "", null },
                    { 113, "", new DateTime(2020, 9, 10, 4, 17, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 144, 999, "", null },
                    { 114, "", new DateTime(2020, 9, 10, 4, 17, 12, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 145, 999, "", null },
                    { 115, "", new DateTime(2020, 9, 10, 4, 17, 12, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 146, 999, "", null },
                    { 116, "", new DateTime(2020, 9, 10, 4, 17, 13, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 147, 999, "", null },
                    { 117, "", new DateTime(2020, 9, 10, 4, 17, 14, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 148, 999, "", null },
                    { 118, "", new DateTime(2020, 9, 10, 4, 17, 14, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 149, 999, "", null },
                    { 119, "", new DateTime(2020, 9, 10, 4, 17, 15, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 194, 999, "", null },
                    { 120, "", new DateTime(2020, 9, 10, 4, 17, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 195, 999, "", null },
                    { 121, "", new DateTime(2020, 9, 10, 4, 17, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 196, 999, "", null },
                    { 122, "", new DateTime(2020, 9, 10, 4, 17, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 197, 999, "", null },
                    { 123, "", new DateTime(2020, 9, 10, 4, 17, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 198, 999, "", null },
                    { 124, "", new DateTime(2020, 9, 10, 4, 17, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 199, 999, "", null },
                    { 125, "", new DateTime(2020, 9, 10, 4, 17, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 80, 999, "", null },
                    { 126, "", new DateTime(2020, 9, 10, 4, 17, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 81, 999, "", null },
                    { 127, "", new DateTime(2020, 9, 10, 4, 17, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 82, 999, "", null },
                    { 111, "", new DateTime(2020, 9, 10, 4, 17, 9, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 204, 999, "", null },
                    { 74, "", new DateTime(2020, 9, 10, 4, 16, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 63, 999, "", null },
                    { 73, "", new DateTime(2020, 9, 10, 4, 16, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 62, 999, "", null },
                    { 72, "", new DateTime(2020, 9, 10, 4, 16, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 79, 999, "", null },
                    { 20, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 211, 999, "", null },
                    { 21, "", new DateTime(2020, 9, 10, 4, 14, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 174, 999, "", null },
                    { 22, "", new DateTime(2020, 9, 10, 4, 14, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 175, 999, "", null },
                    { 23, "", new DateTime(2020, 9, 10, 4, 15, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 176, 999, "", new DateTime(2020, 9, 10, 4, 15, 1, 0, DateTimeKind.Unspecified) },
                    { 24, "", new DateTime(2020, 9, 10, 4, 15, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 177, 999, "", null },
                    { 25, "", new DateTime(2020, 9, 10, 4, 15, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 178, 999, "", null },
                    { 19, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 210, 999, "", null },
                    { 26, "", new DateTime(2020, 9, 10, 4, 15, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 179, 999, "", null },
                    { 28, "", new DateTime(2020, 9, 10, 4, 15, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 169, 999, "", null },
                    { 29, "", new DateTime(2020, 9, 10, 4, 15, 5, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 170, 999, "", null },
                    { 30, "", new DateTime(2020, 9, 10, 4, 15, 5, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 171, 999, "", null },
                    { 31, "", new DateTime(2020, 9, 10, 4, 15, 38, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 162, 999, "", null },
                    { 32, "", new DateTime(2020, 9, 10, 4, 15, 39, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 163, 999, "", null },
                    { 33, "", new DateTime(2020, 9, 10, 4, 15, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 213, 999, "", null },
                    { 27, "", new DateTime(2020, 9, 10, 4, 15, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 168, 999, "", null },
                    { 18, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 209, 999, "", null },
                    { 17, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 208, 999, "", null },
                    { 16, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 207, 999, "", null },
                    { 1, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 186, 999, "", null },
                    { 2, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 187, 999, "", null },
                    { 3, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 188, 999, "", null },
                    { 4, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 189, 999, "", null },
                    { 5, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 190, 999, "", null },
                    { 6, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 191, 999, "", null },
                    { 7, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 192, 999, "", null },
                    { 8, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 193, 999, "", null },
                    { 9, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 180, 999, "", null },
                    { 10, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 181, 999, "", null },
                    { 11, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 182, 999, "", null },
                    { 12, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 183, 999, "", null },
                    { 13, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 184, 999, "", null },
                    { 14, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 185, 999, "", null },
                    { 15, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, true, false, "", 206, 999, "", null },
                    { 34, "", new DateTime(2020, 9, 10, 4, 15, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 104, 999, "", null },
                    { 35, "", new DateTime(2020, 9, 10, 4, 15, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 164, 999, "", null },
                    { 36, "", new DateTime(2020, 9, 10, 4, 15, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 165, 999, "", null },
                    { 37, "", new DateTime(2020, 9, 10, 4, 15, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 166, 999, "", null },
                    { 57, "", new DateTime(2020, 9, 10, 4, 16, 15, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 158, 999, "", null },
                    { 58, "", new DateTime(2020, 9, 10, 4, 16, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 159, 999, "", null },
                    { 59, "", new DateTime(2020, 9, 10, 4, 16, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 160, 999, "", null },
                    { 60, "", new DateTime(2020, 9, 10, 4, 16, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 161, 999, "", null },
                    { 61, "", new DateTime(2020, 9, 10, 4, 16, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 68, 999, "", null },
                    { 62, "", new DateTime(2020, 9, 10, 4, 16, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 69, 999, "", null },
                    { 63, "", new DateTime(2020, 9, 10, 4, 16, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 70, 999, "", null },
                    { 64, "", new DateTime(2020, 9, 10, 4, 16, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 71, 999, "", null },
                    { 65, "", new DateTime(2020, 9, 10, 4, 16, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 72, 999, "", null },
                    { 66, "", new DateTime(2020, 9, 10, 4, 16, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 73, 999, "", null },
                    { 67, "", new DateTime(2020, 9, 10, 4, 16, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 74, 999, "", null },
                    { 68, "", new DateTime(2020, 9, 10, 4, 16, 26, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 75, 999, "", null },
                    { 69, "", new DateTime(2020, 9, 10, 4, 16, 26, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 76, 999, "", null },
                    { 70, "", new DateTime(2020, 9, 10, 4, 16, 27, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 77, 999, "", null },
                    { 71, "", new DateTime(2020, 9, 10, 4, 16, 27, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 78, 999, "", null },
                    { 56, "", new DateTime(2020, 9, 10, 4, 16, 15, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 157, 999, "", null },
                    { 413, "", new DateTime(2020, 9, 11, 2, 26, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 10, 2, "", null },
                    { 55, "", new DateTime(2020, 9, 10, 4, 16, 14, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 156, 999, "", null },
                    { 53, "", new DateTime(2020, 9, 10, 4, 16, 11, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 131, 999, "", null },
                    { 38, "", new DateTime(2020, 9, 10, 4, 15, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 167, 999, "", null },
                    { 39, "", new DateTime(2020, 9, 10, 4, 16, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 23, 999, "", null },
                    { 40, "", new DateTime(2020, 9, 10, 4, 16, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 24, 999, "", null },
                    { 41, "", new DateTime(2020, 9, 10, 4, 16, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 25, 999, "", null },
                    { 42, "", new DateTime(2020, 9, 10, 4, 16, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 26, 999, "", null },
                    { 43, "", new DateTime(2020, 9, 10, 4, 16, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 27, 999, "", null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Deny", "DisplayOrder", "Grant", "IsActive", "IsDeleted", "OrganizationPath", "PermissionId", "RoleId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 44, "", new DateTime(2020, 9, 10, 4, 16, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 28, 999, "", null },
                    { 45, "", new DateTime(2020, 9, 10, 4, 16, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 29, 999, "", null },
                    { 46, "", new DateTime(2020, 9, 10, 4, 16, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 30, 999, "", null },
                    { 47, "", new DateTime(2020, 9, 10, 4, 16, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 31, 999, "", null },
                    { 48, "", new DateTime(2020, 9, 10, 4, 16, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 32, 999, "", null },
                    { 49, "", new DateTime(2020, 9, 10, 4, 16, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 33, 999, "", null },
                    { 50, "", new DateTime(2020, 9, 10, 4, 16, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 34, 999, "", null },
                    { 51, "", new DateTime(2020, 9, 10, 4, 16, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 35, 999, "", null },
                    { 52, "", new DateTime(2020, 9, 10, 4, 16, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 36, 999, "", null },
                    { 54, "", new DateTime(2020, 9, 10, 4, 16, 11, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 132, 999, "", new DateTime(2020, 9, 10, 4, 16, 13, 0, DateTimeKind.Unspecified) },
                    { 147, "", new DateTime(2020, 9, 10, 4, 17, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 41, 999, "", null },
                    { 146, "", new DateTime(2020, 9, 10, 4, 17, 50, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 40, 999, "", null },
                    { 149, "", new DateTime(2020, 9, 10, 4, 17, 53, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 43, 999, "", null },
                    { 676, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 436, 999, "", null },
                    { 677, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 437, 999, "", null },
                    { 678, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 438, 999, "", null },
                    { 679, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 439, 999, "", null },
                    { 680, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 440, 999, "", null },
                    { 666, "", new DateTime(2020, 9, 11, 2, 43, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 228, 999, "", null },
                    { 665, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 247, 999, "", null },
                    { 667, "", new DateTime(2020, 9, 11, 2, 44, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 230, 999, "", null },
                    { 669, "", new DateTime(2020, 9, 11, 2, 44, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 232, 999, "", null },
                    { 670, "", new DateTime(2020, 9, 11, 2, 44, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 231, 999, "", null },
                    { 671, "", new DateTime(2020, 9, 11, 2, 44, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 433, 999, "", null },
                    { 672, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 434, 999, "", null },
                    { 639, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 322, 999, "", null },
                    { 638, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 323, 999, "", null },
                    { 668, "", new DateTime(2020, 9, 11, 2, 44, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 229, 999, "", null },
                    { 664, "", new DateTime(2020, 9, 11, 2, 44, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 246, 999, "", null },
                    { 663, "", new DateTime(2020, 9, 11, 2, 44, 31, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 245, 999, "", null },
                    { 662, "", new DateTime(2020, 9, 11, 2, 44, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 244, 999, "", null },
                    { 223, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 223, 999, "", null },
                    { 224, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 224, 999, "", null },
                    { 225, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 225, 999, "", null },
                    { 226, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 226, 999, "", null },
                    { 233, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 233, 999, "", null },
                    { 234, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 234, 999, "", null },
                    { 235, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 235, 999, "", null },
                    { 236, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 236, 999, "", null },
                    { 237, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 237, 999, "", null },
                    { 238, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 238, 999, "", null },
                    { 239, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 239, 999, "", null },
                    { 240, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 240, 999, "", null },
                    { 241, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 241, 999, "", null },
                    { 242, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 242, 999, "", null },
                    { 661, "", new DateTime(2020, 9, 11, 2, 44, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 243, 999, "", null },
                    { 637, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 324, 999, "", null },
                    { 148, "", new DateTime(2020, 9, 10, 4, 17, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 42, 999, "", null },
                    { 636, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 325, 999, "", null },
                    { 642, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 327, 999, "", null },
                    { 398, "", new DateTime(2020, 9, 11, 2, 26, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 17, 2, "", null },
                    { 399, "", new DateTime(2020, 9, 11, 2, 26, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 18, 2, "", null },
                    { 400, "", new DateTime(2020, 9, 11, 2, 26, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 19, 2, "", null },
                    { 401, "", new DateTime(2020, 9, 11, 2, 26, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 20, 2, "", null },
                    { 402, "", new DateTime(2020, 9, 11, 2, 26, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 21, 2, "", null },
                    { 403, "", new DateTime(2020, 9, 11, 2, 26, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 22, 2, "", null },
                    { 397, "", new DateTime(2020, 9, 11, 2, 25, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 169, 2, "", null },
                    { 404, "", new DateTime(2020, 9, 11, 2, 26, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 1, 2, "", null },
                    { 406, "", new DateTime(2020, 9, 11, 2, 26, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 3, 2, "", null },
                    { 407, "", new DateTime(2020, 9, 11, 2, 26, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 4, 2, "", null },
                    { 408, "", new DateTime(2020, 9, 11, 2, 26, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 5, 2, "", null },
                    { 655, "", new DateTime(2020, 10, 29, 8, 35, 58, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 339, 6, "", null },
                    { 410, "", new DateTime(2020, 9, 11, 2, 26, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 7, 2, "", null },
                    { 411, "", new DateTime(2020, 9, 11, 2, 26, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 8, 2, "", null },
                    { 405, "", new DateTime(2020, 9, 11, 2, 26, 16, 0, DateTimeKind.Unspecified), "", false, 0, false, false, false, "", 2, 2, "", new DateTime(2020, 9, 11, 2, 26, 34, 0, DateTimeKind.Unspecified) },
                    { 396, "", new DateTime(2020, 9, 11, 2, 25, 49, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 168, 2, "", null },
                    { 395, "", new DateTime(2020, 9, 11, 2, 25, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 175, 2, "", null },
                    { 394, "", new DateTime(2020, 9, 11, 2, 25, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 174, 2, "", null },
                    { 641, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 328, 999, "", null },
                    { 640, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 329, 999, "", null },
                    { 645, "", new DateTime(2020, 10, 14, 8, 31, 41, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 330, 999, "", null },
                    { 644, "", new DateTime(2020, 10, 14, 8, 31, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 331, 999, "", null },
                    { 646, "", new DateTime(2020, 10, 14, 8, 31, 45, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 332, 999, "", null },
                    { 648, "", new DateTime(2020, 10, 22, 8, 47, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 332, 999, "", null },
                    { 647, "", new DateTime(2020, 10, 14, 8, 31, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 333, 999, "", null },
                    { 653, "", new DateTime(2020, 10, 29, 1, 50, 8, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 335, 999, "", null },
                    { 652, "", new DateTime(2020, 10, 29, 1, 50, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 336, 999, "", null },
                    { 650, "", new DateTime(2020, 10, 29, 1, 50, 5, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 337, 999, "", null },
                    { 651, "", new DateTime(2020, 10, 29, 1, 50, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 338, 999, "", null },
                    { 654, "", new DateTime(2020, 10, 29, 2, 59, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 339, 999, "", null },
                    { 656, "", new DateTime(2020, 10, 30, 8, 20, 13, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 431, 999, "", null },
                    { 660, "", new DateTime(2020, 10, 30, 8, 20, 13, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 432, 999, "", null },
                    { 682, "", new DateTime(2020, 9, 11, 2, 44, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 440, 2, "", null },
                    { 643, "", new DateTime(2020, 10, 9, 9, 11, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 326, 999, "", null },
                    { 221, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 221, 999, "", null },
                    { 222, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 222, 999, "", null },
                    { 219, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 219, 999, "", null },
                    { 169, "", new DateTime(2020, 9, 10, 4, 18, 12, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 99, 999, "", null },
                    { 170, "", new DateTime(2020, 9, 10, 4, 18, 13, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 100, 999, "", null },
                    { 171, "", new DateTime(2020, 9, 10, 4, 18, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 101, 999, "", null },
                    { 172, "", new DateTime(2020, 9, 10, 4, 18, 16, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 102, 999, "", null },
                    { 173, "", new DateTime(2020, 9, 10, 4, 18, 17, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 103, 999, "", null },
                    { 174, "", new DateTime(2020, 9, 10, 4, 18, 18, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 110, 999, "", null },
                    { 175, "", new DateTime(2020, 9, 10, 4, 18, 19, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 111, 999, "", null },
                    { 176, "", new DateTime(2020, 9, 10, 4, 18, 20, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 112, 999, "", null },
                    { 177, "", new DateTime(2020, 9, 10, 4, 18, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 113, 999, "", null },
                    { 178, "", new DateTime(2020, 9, 10, 4, 18, 21, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 114, 999, "", null },
                    { 179, "", new DateTime(2020, 9, 10, 4, 18, 22, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 115, 999, "", null },
                    { 180, "", new DateTime(2020, 9, 10, 4, 18, 23, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 116, 999, "", null },
                    { 181, "", new DateTime(2020, 9, 10, 4, 18, 24, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 105, 999, "", null },
                    { 182, "", new DateTime(2020, 9, 10, 4, 18, 25, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 106, 999, "", null },
                    { 183, "", new DateTime(2020, 9, 10, 4, 18, 26, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 107, 999, "", null },
                    { 168, "", new DateTime(2020, 9, 10, 4, 18, 12, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 98, 999, "", null },
                    { 184, "", new DateTime(2020, 9, 10, 4, 18, 27, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 108, 999, "", null },
                    { 167, "", new DateTime(2020, 9, 10, 4, 18, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 122, 999, "", null },
                    { 165, "", new DateTime(2020, 9, 10, 4, 18, 9, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 120, 999, "", null },
                    { 150, "", new DateTime(2020, 9, 10, 4, 17, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 44, 999, "", null },
                    { 151, "", new DateTime(2020, 9, 10, 4, 17, 55, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 56, 999, "", null },
                    { 152, "", new DateTime(2020, 9, 10, 4, 17, 56, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 57, 999, "", null },
                    { 153, "", new DateTime(2020, 9, 10, 4, 17, 57, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 58, 999, "", null },
                    { 154, "", new DateTime(2020, 9, 10, 4, 17, 58, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 59, 999, "", null },
                    { 155, "", new DateTime(2020, 9, 10, 4, 17, 58, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 60, 999, "", null },
                    { 156, "", new DateTime(2020, 9, 10, 4, 17, 59, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 61, 999, "", null },
                    { 157, "", new DateTime(2020, 9, 10, 4, 18, 0, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 212, 999, "", null },
                    { 158, "", new DateTime(2020, 9, 10, 4, 18, 1, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 123, 999, "", null },
                    { 159, "", new DateTime(2020, 9, 10, 4, 18, 2, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 124, 999, "", null },
                    { 160, "", new DateTime(2020, 9, 10, 4, 18, 3, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 125, 999, "", null },
                    { 161, "", new DateTime(2020, 9, 10, 4, 18, 4, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 126, 999, "", null },
                    { 162, "", new DateTime(2020, 9, 10, 4, 18, 5, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 117, 999, "", null },
                    { 163, "", new DateTime(2020, 9, 10, 4, 18, 6, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 118, 999, "", null },
                    { 164, "", new DateTime(2020, 9, 10, 4, 18, 7, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 119, 999, "", null },
                    { 166, "", new DateTime(2020, 9, 10, 4, 18, 10, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 121, 999, "", null },
                    { 220, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 220, 999, "", null },
                    { 185, "", new DateTime(2020, 9, 10, 4, 18, 28, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 109, 999, "", null },
                    { 187, "", new DateTime(2020, 9, 10, 4, 18, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 128, 999, "", null },
                    { 204, "", new DateTime(2020, 9, 10, 4, 18, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 15, 999, "", null },
                    { 205, "", new DateTime(2020, 9, 10, 4, 18, 47, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 16, 999, "", null },
                    { 206, "", new DateTime(2020, 9, 10, 4, 18, 48, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 17, 999, "", null },
                    { 207, "", new DateTime(2020, 9, 10, 4, 18, 49, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 18, 999, "", null },
                    { 208, "", new DateTime(2020, 9, 10, 4, 18, 49, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 19, 999, "", null },
                    { 209, "", new DateTime(2020, 9, 10, 4, 18, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 20, 999, "", null },
                    { 210, "", new DateTime(2020, 9, 10, 4, 18, 51, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 21, 999, "", null },
                    { 211, "", new DateTime(2020, 9, 10, 4, 18, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 22, 999, "", null },
                    { 212, "", new DateTime(2020, 9, 10, 4, 18, 53, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 172, 999, "", null },
                    { 213, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 173, 999, "", null },
                    { 214, "", new DateTime(2020, 9, 10, 4, 18, 52, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 214, 999, "", null },
                    { 215, "", new DateTime(2020, 9, 10, 4, 18, 53, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 215, 999, "", null },
                    { 216, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 216, 999, "", null },
                    { 217, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 217, 999, "", null },
                    { 218, "", new DateTime(2020, 9, 10, 4, 18, 54, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 218, 999, "", null },
                    { 203, "", new DateTime(2020, 9, 10, 4, 18, 46, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 14, 999, "", null },
                    { 186, "", new DateTime(2020, 9, 10, 4, 18, 29, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 127, 999, "", null },
                    { 202, "", new DateTime(2020, 9, 10, 4, 18, 45, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 13, 999, "", null },
                    { 193, "", new DateTime(2020, 9, 10, 4, 18, 35, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 4, 999, "", null },
                    { 200, "", new DateTime(2020, 9, 10, 4, 18, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 11, 999, "", null },
                    { 199, "", new DateTime(2020, 9, 10, 4, 18, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 10, 999, "", null },
                    { 198, "", new DateTime(2020, 9, 10, 4, 18, 40, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 9, 999, "", null },
                    { 197, "", new DateTime(2020, 9, 10, 4, 18, 39, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 8, 999, "", null },
                    { 196, "", new DateTime(2020, 9, 10, 4, 18, 39, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 7, 999, "", null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Deny", "DisplayOrder", "Grant", "IsActive", "IsDeleted", "OrganizationPath", "PermissionId", "RoleId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 195, "", new DateTime(2020, 9, 10, 4, 18, 37, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 6, 999, "", null },
                    { 194, "", new DateTime(2020, 9, 10, 4, 18, 36, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 5, 999, "", null },
                    { 201, "", new DateTime(2020, 9, 10, 4, 18, 42, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 12, 999, "", null },
                    { 192, "", new DateTime(2020, 9, 10, 4, 18, 35, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 3, 999, "", null },
                    { 191, "", new DateTime(2020, 9, 10, 4, 18, 34, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 2, 999, "", null },
                    { 190, "", new DateTime(2020, 9, 10, 4, 18, 32, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 1, 999, "", null },
                    { 188, "", new DateTime(2020, 9, 10, 4, 18, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 129, 999, "", null },
                    { 189, "", new DateTime(2020, 9, 10, 4, 18, 30, 0, DateTimeKind.Unspecified), "", false, 0, true, false, false, "", 130, 999, "", null }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "RoleId", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 8, "", new DateTime(2020, 9, 9, 8, 44, 12, 0, DateTimeKind.Unspecified), "", 0, true, false, "", 3, "", null, 13 },
                    { 1, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vn", 0, true, false, "", 999, "system", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 7, "", new DateTime(2020, 9, 9, 8, 30, 48, 0, DateTimeKind.Unspecified), "", 0, true, false, "", 5, "", null, 12 },
                    { 9, "", new DateTime(2020, 9, 9, 8, 49, 33, 0, DateTimeKind.Unspecified), "", 0, true, false, "", 2, "", null, 14 },
                    { 10, "", new DateTime(2020, 9, 9, 9, 2, 43, 0, DateTimeKind.Unspecified), "", 0, true, false, "", 6, "", null, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationPersonalAccounts_UserId",
                table: "ConfigurationPersonalAccounts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_UserId",
                table: "ContactInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnits_ManagementUserId",
                table: "OrganizationUnits",
                column: "ManagementUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBankAccounts_UserId",
                table: "UserBankAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarId",
                table: "Users",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationUnitId",
                table: "Users",
                column: "OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OrganizationUnits_OrganizationUnitId",
                table: "Users",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUnits_Users_ManagementUserId",
                table: "OrganizationUnits");

            migrationBuilder.DropTable(
                name: "ConfigurationPersonalAccounts");

            migrationBuilder.DropTable(
                name: "ConfigurationSystemParameters");

            migrationBuilder.DropTable(
                name: "ContactInfos");

            migrationBuilder.DropTable(
                name: "FCMTokens");

            migrationBuilder.DropTable(
                name: "Otps");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserBankAccounts");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "OrganizationUnits");
        }
    }
}
