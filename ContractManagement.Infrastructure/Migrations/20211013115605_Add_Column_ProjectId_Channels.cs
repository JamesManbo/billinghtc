using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Add_Column_ProjectId_Channels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "TransactionServicePackages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "OutContractServicePackages",
                nullable: true);

            migrationBuilder.Sql("UPDATE OutContractServicePackages t1 " +
                "INNER JOIN OutContracts t2 ON t2.Id = t1.OutContractId " +
                "SET t1.ProjectId = t2.ProjectId "+
                "WHERE 1 = 1;");

            migrationBuilder.Sql("UPDATE OutContractServicePackages t1 " +
                "INNER JOIN InContracts t2 ON t2.Id = t1.InContractId " +
                "SET t1.ProjectId = t2.ProjectId " +
                "WHERE 1 = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TransactionServicePackages");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "OutContractServicePackages");
        }
    }
}
