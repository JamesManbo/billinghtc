using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    ClassName = table.Column<string>(nullable: true),
                    ClassCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    GroupName = table.Column<string>(nullable: true),
                    GroupCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    FileName = table.Column<string>(maxLength: 256, nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Order = table.Column<int>(nullable: true),
                    PictureType = table.Column<int>(nullable: false),
                    Extension = table.Column<string>(nullable: true),
                    RedirectLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: true),
                    IdentityGuid = table.Column<Guid>(nullable: false),
                    AvatarId = table.Column<int>(nullable: true),
                    CustomerCode = table.Column<string>(maxLength: 256, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(maxLength: 256, nullable: true),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    MobilePhoneNo = table.Column<string>(maxLength: 1000, nullable: true),
                    FaxNo = table.Column<string>(maxLength: 1000, nullable: true),
                    Password = table.Column<string>(type: "LONGTEXT", nullable: true),
                    PasswordSalt = table.Column<string>(maxLength: 128, nullable: true),
                    SecurityStamp = table.Column<string>(maxLength: 68, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    IdNo = table.Column<string>(maxLength: 256, nullable: true),
                    IdDateOfIssue = table.Column<DateTime>(nullable: true),
                    IdIssuedBy = table.Column<string>(maxLength: 1000, nullable: true),
                    TaxIdNo = table.Column<string>(nullable: true),
                    RepresentativePersonName = table.Column<string>(maxLength: 256, nullable: true),
                    RpDateOfBirth = table.Column<DateTime>(nullable: true),
                    RpJobPosition = table.Column<string>(maxLength: 256, nullable: true),
                    BusinessRegCertificate = table.Column<string>(maxLength: 256, nullable: true),
                    BrcDateOfIssue = table.Column<DateTime>(nullable: true),
                    BrcIssuedBy = table.Column<string>(maxLength: 1000, nullable: true),
                    Address = table.Column<string>(maxLength: 1000, nullable: true),
                    Ward = table.Column<string>(maxLength: 256, nullable: true),
                    WardIdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    District = table.Column<string>(maxLength: 256, nullable: true),
                    DistrictIdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Province = table.Column<string>(maxLength: 256, nullable: true),
                    ProvinceIdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    IsEnterprise = table.Column<bool>(nullable: false),
                    IsEmailCertificated = table.Column<bool>(nullable: false),
                    IsPhoneNoCertificated = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Pictures_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserUserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserUserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserUserGroups_ApplicationUserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ApplicationUserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserUserGroups_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_AvatarId",
                table: "ApplicationUsers",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserUserGroups_GroupId",
                table: "ApplicationUserUserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserUserGroups_UserId",
                table: "ApplicationUserUserGroups",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserClasses");

            migrationBuilder.DropTable(
                name: "ApplicationUserUserGroups");

            migrationBuilder.DropTable(
                name: "ApplicationUserGroups");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Pictures");
        }
    }
}
