using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Create_Table_VourcherTargetProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoucherTargetProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    StructureId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    GroupIds = table.Column<string>(nullable: true),
                    ClassId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    IndustryIds = table.Column<string>(nullable: true),
                    ApplicationUserIdentityGuid = table.Column<string>(nullable: true),
                    StructureName = table.Column<string>(nullable: true),
                    CategoryName = table.Column<string>(nullable: true),
                    GroupNames = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    IndustryNames = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherTargetProperties", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoucherTargetProperties");
        }
    }
}
