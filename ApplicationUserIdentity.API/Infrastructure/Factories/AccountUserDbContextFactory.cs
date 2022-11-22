using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace ApplicationUserIdentity.API.Infrastructure.Factories
{
    public class AccountUserDbContextFactory : IDesignTimeDbContextFactory<ApplicationUserDbContext>
    {
        public ApplicationUserDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationUserDbContext>();

            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), x => x
                .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                .MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));

            return new ApplicationUserDbContext(optionsBuilder.Options);
        }
    }
}
