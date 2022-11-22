using ContractManagement.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ContractManagement.Infrastructure.Repositories.RadiusAndBrasRepository;
using ContractManagement.RadiusDomain.Repositories;
using ContractManagement.API.Application.Services.MikroTik;
using ContractManagement.RadiusDomain.Models;
using Microsoft.EntityFrameworkCore;
using ContractManagement.Infrastructure;
using GenericRepository.Setups;

namespace TikForNet.FuntionalTests.RadiusManagement
{
    public class RadiusManagementTestBase
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }
        public RadiusManagementTestBase()
        {
            CreateServer();
            var services = new ServiceCollection();

            services.Configure<TikConnectionSettings>(
              Configuration.GetSection("TikConnectionSettings"));

            services.AddDbContext<ContractDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<RadiusContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("RadiusDbConnection"));
            });

            services.GenericRepositorySetup(typeof(Startup).Assembly);
            services.AddTransient<IRadiusManagementRepository, RadiusManagementRepository>();
            services.AddTransient<IRadiusServerInfoRepository, RadiusServerInfoRepository>();
            services.AddTransient<IBrasInfoRepository, BrasInfoRepository>();
            services.AddTransient<IMikroTikService, MikroTikService>();
            ServiceProvider = services.BuildServiceProvider();
        }

        public void CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(RadiusManagementTestBase))
                .Location;

            var configurationBuider = new ConfigurationBuilder()
                .AddJsonFile("RadiusManagement/appsettings.json", optional: false)
                .AddEnvironmentVariables();

            Configuration = configurationBuider.Build();

            //var hostBuilder = Host.CreateDefaultBuilder()
            //    .ConfigureWebHostDefaults(webBuilder =>
            //    {
            //        webBuilder.UseContentRoot(Path.GetDirectoryName(path))
            //        .ConfigureAppConfiguration(cb =>
            //        {
            //            configurationBuider.Build();
            //        })
            //        .UseStartup<Startup>();
            //    })
            //    .UseServiceProviderFactory(new AutofacServiceProviderFactory());

            //hostBuilder.Start();
        }
    }
}
