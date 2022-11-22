using System;
using System.IO;
using System.Reflection;
using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace ApplicationUserIdentity.API.Infrastructure.Factories
{
    public class IntegrationEventLogDbContextFactory : IDesignTimeDbContextFactory<IntegrationEventLogDbContext>
    {
        public IntegrationEventLogDbContext CreateDbContext(string[] args)
        {
            var configs = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IntegrationEventLogDbContext>();

            optionsBuilder.UseMySql(configs.GetConnectionString("DefaultConnection"), x => x
                .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                .MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));

            return new IntegrationEventLogDbContext(optionsBuilder.Options);
        }
    }
}
