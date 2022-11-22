using DebtManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.Factories
{
    public class DebtDbContextFactory : IDesignTimeDbContextFactory<DebtDbContext>
    {
        public DebtDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DebtDbContext>();

            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), x => x
                    .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                    .MigrationsAssembly(typeof(DebtDbContext).GetTypeInfo().Assembly.GetName().Name)
                    
                    );

            return new DebtDbContext(optionsBuilder.Options);
        }
    }
}
