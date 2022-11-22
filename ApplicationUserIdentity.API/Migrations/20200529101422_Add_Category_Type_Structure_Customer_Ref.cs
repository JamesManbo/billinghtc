using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Add_Category_Type_Structure_Customer_Ref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerCategoryId",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerStructureId",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerTypeId",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_CustomerCategoryId",
                table: "ApplicationUsers",
                column: "CustomerCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_CustomerStructureId",
                table: "ApplicationUsers",
                column: "CustomerStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_CustomerTypeId",
                table: "ApplicationUsers",
                column: "CustomerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_CustomerCategories_CustomerCategoryId",
                table: "ApplicationUsers",
                column: "CustomerCategoryId",
                principalTable: "CustomerCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_CustomerStructure_CustomerStructureId",
                table: "ApplicationUsers",
                column: "CustomerStructureId",
                principalTable: "CustomerStructure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_CustomerTypes_CustomerTypeId",
                table: "ApplicationUsers",
                column: "CustomerTypeId",
                principalTable: "CustomerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_CustomerCategories_CustomerCategoryId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_CustomerStructure_CustomerStructureId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_CustomerTypes_CustomerTypeId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_CustomerCategoryId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_CustomerStructureId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_CustomerTypeId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CustomerCategoryId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CustomerStructureId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CustomerTypeId",
                table: "ApplicationUsers");
        }
    }
}
