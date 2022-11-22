using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Add_Table_Industries_And_ApplicationIndustries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "Otps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "FCMTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "CustomerTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "CustomerStructure",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "CustomerCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "ApplicationUserUserGroups",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPromotion",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectIdentityGuid",
                table: "ApplicationUsers",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromotionTypeId",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SwiftCode",
                table: "ApplicationUsers",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TradingAddress",
                table: "ApplicationUsers",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "ApplicationUserGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPath",
                table: "ApplicationUserClasses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Industries",
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
                    OrganizationPath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 512, nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ApplicationUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Industries_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserIndustries",
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
                    OrganizationPath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    IndustryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserIndustries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserIndustries_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserIndustries_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserIndustries_IndustryId",
                table: "ApplicationUserIndustries",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserIndustries_UserId",
                table: "ApplicationUserIndustries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_ApplicationUserId",
                table: "Industries",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserIndustries");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "Otps");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "FCMTokens");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "CustomerTypes");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "CustomerStructure");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "CustomerCategories");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "ApplicationUserUserGroups");

            migrationBuilder.DropColumn(
                name: "IsPromotion",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ProjectIdentityGuid",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "PromotionTypeId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "SwiftCode",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "TradingAddress",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "ApplicationUserGroups");

            migrationBuilder.DropColumn(
                name: "OrganizationPath",
                table: "ApplicationUserClasses");
        }
    }
}
