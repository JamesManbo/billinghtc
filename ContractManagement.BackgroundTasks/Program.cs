
using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ContractManagement.BackgroundTasks.Extensions;
using ContractManagement.BackgroundTasks.Services.Grpc;
using ContractManagement.BackgroundTasks.Tasks;
using ContractManagement.Infrastructure;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ExchangeRateRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using GenericRepository.Setups;
using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Serilog;

namespace ContractManagement.BackgroundTasks
{
    public class Program
    {
        public static readonly string AppName = typeof(Program).Assembly.GetName().Name;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IHost CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAutoMapper(typeof(Startup));
                    // Add framework services.
                    services.AddCustomDbContext(hostContext.Configuration);
                    services.GenericRepositorySetup(typeof(Startup).Assembly);

                    // Query
                    services.AddTransient<IServicesQueries, ServicesQueries>();
                    services.AddTransient<IProjectQueries, ProjectQueries>();
                    services.AddTransient<IMarketAreaQueries, MarketAreaQueries>();
                    services.AddTransient<IOutContractQueries, OutContractQueries>();
                    services.AddTransient<IExchangeRateQueries, ExchangeRateQueries>();
                    services.AddTransient<IOutContractQueries, OutContractQueries>();

                    // Repository
                    services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
                    services.AddTransient<IOutContractRepository, OutContractRepository>();

                    // GRPC Service
                    services.AddTransient<INotificationGrpcService, NotificationGrpcService>();
                    services.AddTransient<ISystemConfigurationGrpcService, SystemConfigurationGrpcService>();

                    services.AddHostedService<ContractExpirationManagementTask>();
                    services.AddHostedService<ExchangeRateManagementTask>();
                    services.AddHostedService<NotifyChannelToRenewalPeriodTask>();
                })
                .ConfigureAppConfiguration((host, builder) =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json", optional: true);
                    builder.AddJsonFile($"appsettings.{environment}.json", optional: true);
                    builder.AddEnvironmentVariables();
                    builder.AddCommandLine(args);
                })
                .ConfigureLogging((host, builder) => builder.UseSerilog(host.Configuration).AddSerilog())
                .Build();
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            var resilientRetryCount = 5;
            if (!string.IsNullOrEmpty(configuration["ResilientMySqlCount"]))
            {
                resilientRetryCount = int.Parse(configuration["ResilientMySqlCount"]);
            }

            services.AddDbContext<ContractDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(ContractDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            return services;
        }
    }
}
