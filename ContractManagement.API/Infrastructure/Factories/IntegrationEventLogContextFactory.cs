using System;
using System.IO;
using System.Reflection;
using ContractManagement.Infrastructure;
using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace ContractManagement.API.Infrastructure.Factories
{
    public class IntegrationEventLogContextFactory : IDesignTimeDbContextFactory<IntegrationEventLogDbContext>
    {
        public IntegrationEventLogDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IntegrationEventLogDbContext>();

            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), x => x
                .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                .MigrationsAssembly(typeof(ContractDbContext).GetTypeInfo().Assembly.GetName().Name));

            return new IntegrationEventLogDbContext(optionsBuilder.Options);
        }
    }
}
