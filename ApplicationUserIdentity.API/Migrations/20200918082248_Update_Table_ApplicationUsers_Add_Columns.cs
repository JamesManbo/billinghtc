using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationUserIdentity.API.Migrations
{
    public partial class Update_Table_ApplicationUsers_Add_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPartner",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserIdentityGuid",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPartner",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "UserIdentityGuid",
                table: "ApplicationUsers");
        }
    }
}
