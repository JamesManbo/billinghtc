using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate;
using OrganizationUnit.Domain.RoleAggregate;

namespace ContractManagement.Infrastructure.Seed
{
    public static class OrganizationUnitContextRuntimeSeed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            //Seed data
            var seedPermissions = ReadDataFromCSV<Permission>("permissions");
            modelBuilder.Entity<Permission>().HasData(seedPermissions);

            var seedRoles = ReadDataFromCSV<Role>("roles");
            modelBuilder.Entity<Role>().HasData(seedRoles);

            var seedRolePermissions = ReadDataFromCSV<RolePermission>("rolepermissions");
            modelBuilder.Entity<RolePermission>().HasData(seedRolePermissions);

            var seedUser = ReadDataFromCSV<User>("user");
            modelBuilder.Entity<User>().HasData(seedUser);

            var seedUserRoles = ReadDataFromCSV<UserRole>("userroles");
            modelBuilder.Entity<UserRole>().HasData(seedUserRoles);

            //var organizationUnits = ReadDataFromCSV<OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit>("organization_units");
            //modelBuilder
            //    .Entity<OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit>()
            //    .HasData(organizationUnits);
            

            // Mock data
            var mockConfigurationPersonalParamasters = ReadDataFromCSV<ConfigurationSystemParameter>("configurationSystemParameters");
            modelBuilder.Entity<ConfigurationSystemParameter>().HasData(mockConfigurationPersonalParamasters);

            var mockConfigurationPersonalAccounts = ReadDataFromCSV<ConfigurationPersonalAccount>("configurationPersonalAccounts");
            modelBuilder.Entity<ConfigurationPersonalAccount>().HasData(mockConfigurationPersonalAccounts);

            //Demo data

            //var demoOrgs = ReadDataFromCSV<OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit>("demo-organizationunits");
            //modelBuilder
            //    .Entity<OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit>()
            //    .HasData(demoOrgs);

            var demoRoles = ReadDataFromCSV<Role>("demo-roles");
            modelBuilder.Entity<Role>().HasData(demoRoles);
            var demoUsers = ReadDataFromCSV<User>("demo-users");
            modelBuilder.Entity<User>().HasData(demoUsers);
            var demoRolePermissions = ReadDataFromCSV<RolePermission>("demo-rolepermissions");
            modelBuilder.Entity<RolePermission>().HasData(demoRolePermissions);
            var demoUserRoles = ReadDataFromCSV<UserRole>("demo-userroles");
            modelBuilder.Entity<UserRole>().HasData(demoUserRoles);

            var radiusBrasAndDebtPermissions = ReadDataFromCSV<Permission>("bras_radius_and_debt_permissions");
            modelBuilder.Entity<Permission>().HasData(radiusBrasAndDebtPermissions);
            var adminRadiusBrasAndDebtRolePermissions = ReadDataFromCSV<RolePermission>("bras_radius_and_debt_administrator_assignations");
            modelBuilder.Entity<RolePermission>().HasData(adminRadiusBrasAndDebtRolePermissions);

        }

        private static IEnumerable<TEntity> ReadDataFromCSV<TEntity>(string fileName) where TEntity : new()
        {
            var result = new List<TEntity>();
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            var pathOfFile = Path.Combine(dirPath, "Seed", "Data", fileName + ".csv");

            using (var reader = new StreamReader(pathOfFile))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    csv.ValidateHeader(typeof(TEntity));
                    var records = csv.GetRecords<TEntity>();
                    foreach (var record in records)
                    {
                        result.Add(record);
                    }
                }
            }

            return result;
        }
    }
}
