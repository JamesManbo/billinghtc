using System;
using System.IO;
using System.Reflection;
using ContractManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace ContractManagement.API.Infrastructure.Factories
{
    public class ContractDbContextFactory : IDesignTimeDbContextFactory<ContractDbContext>
    {
        public ContractDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == Environments.Production)
            {
                configBuilder
                    .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
            }
            else if (environment == Environments.Staging)
            {
                configBuilder
                    .AddJsonFile("appsettings.Staging.json", optional: true, reloadOnChange: true);
            }
            else
            {
                configBuilder
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            }

            var config = configBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<ContractDbContext>();

            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), x => x
                    .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                    .MigrationsAssembly(typeof(ContractDbContext).GetTypeInfo().Assembly.GetName().Name));

            return new ContractDbContext(optionsBuilder.Options);
        }
    }
}
