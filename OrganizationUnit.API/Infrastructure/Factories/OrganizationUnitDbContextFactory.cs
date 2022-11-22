using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OrganizationUnit.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace OrganizationUnit.API.Infrastructure.Factories
{
    public class OrganizationUnitDbContextFactory : IDesignTimeDbContextFactory<OrganizationUnitDbContext>
    {
        public OrganizationUnitDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrganizationUnitDbContext>();

            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), x => x
                    .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                    .MigrationsAssembly(typeof(OrganizationUnitDbContext).GetTypeInfo().Assembly.GetName().Name));

            return new OrganizationUnitDbContext(optionsBuilder.Options);
        }
    }
}
