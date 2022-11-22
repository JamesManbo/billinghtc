using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Remove_CreatorId_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCategories_ApplicationUsers_CreatedUId",
                table: "CustomerCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerStructure_ApplicationUsers_CreatedUId",
                table: "CustomerStructure");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTypes_ApplicationUsers_CreatedUId",
                table: "CustomerTypes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTypes_CreatedUId",
                table: "CustomerTypes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerStructure_CreatedUId",
                table: "CustomerStructure");

            migrationBuilder.DropIndex(
                name: "IX_CustomerCategories_CreatedUId",
                table: "CustomerCategories");

            migrationBuilder.DropColumn(
                name: "CreatedUId",
                table: "CustomerTypes");

            migrationBuilder.DropColumn(
                name: "CreatedUId",
                table: "CustomerStructure");

            migrationBuilder.DropColumn(
                name: "CreatedUId",
                table: "CustomerCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedUId",
                table: "CustomerTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedUId",
                table: "CustomerStructure",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedUId",
                table: "CustomerCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypes_CreatedUId",
                table: "CustomerTypes",
                column: "CreatedUId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStructure_CreatedUId",
                table: "CustomerStructure",
                column: "CreatedUId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCategories_CreatedUId",
                table: "CustomerCategories",
                column: "CreatedUId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCategories_ApplicationUsers_CreatedUId",
                table: "CustomerCategories",
                column: "CreatedUId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerStructure_ApplicationUsers_CreatedUId",
                table: "CustomerStructure",
                column: "CreatedUId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTypes_ApplicationUsers_CreatedUId",
                table: "CustomerTypes",
                column: "CreatedUId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
