using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using DebtManagement.BackgroundTasks.Extensions;
using Serilog;
using System.IO;
using System.Reflection;
using AutoMapper;
using DebtManagement.BackgroundTasks.Services.Grpc;
using Microsoft.Extensions.DependencyInjection;
using DebtManagement.BackgroundTasks.Tasks;
using DebtManagement.Infrastructure;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using GenericRepository.Setups;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using DebtManagement.BackgroundTasks.Services.OutContracts;
using DebtManagement.BackgroundTasks.Services.Organizations;

namespace DebtManagement.BackgroundTasks
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
                    services.Configure<ConnectionSettings>(
                      hostContext.Configuration.GetSection("ConnectionStrings"));

                    services.AddAutoMapper(typeof(Startup));
                    // Add framework services.
                    services.AddCustomDbContext(hostContext.Configuration);

                    services.GenericRepositorySetup(typeof(Startup).Assembly);

                    services.InjectRepositories();

                    services.InjectServices();

                    services.InjectGRPCServices();

                    services.AddHostedService<ReceiptVoucherManagementTask>();
                    services.AddHostedService<BadDebtScannerTask>();
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
            //var resilientRetryCount = 5;
            //if (!string.IsNullOrEmpty(configuration["ResilientMySqlCount"]))
            //{
            //    resilientRetryCount = int.Parse(configuration["ResilientMySqlCount"]);
            //}
            services.AddDbContext<DebtDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    //x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(DebtDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IOutContractService, OutContractService>();
            return services;
        }

        public static IServiceCollection InjectRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPaymentVoucherQueries, PaymentVoucherQueries>();
            services.AddTransient<IReceiptVoucherQueries, ReceiptVoucherQueries>();
            services.AddTransient<IReceiptVoucherRepository, ReceiptVoucherRepository>();
            services.AddTransient<IReceiptVoucherDetailRepository, ReceiptVoucherDetailRepository>();
            services.AddTransient<IReceiptVoucherDebtHistoryRepository, ReceiptVoucherDebtHistoryRepository>();
            services.AddTransient<IVoucherTargetRepository, VoucherTargetRepository>();
            services.AddTransient<IVoucherTargetQueries, VoucherTargetQueries>();
            services.AddTransient<IExportInvoiceFileQueries, ExportInvoiceFileQueries>();
            services.AddTransient<IReportCustomerQueries, ReportCustomerQueries>();
            services.AddTransient<IReceiptVoucherTaxRepository, ReceiptVoucherTaxRepository>();
            services.AddTransient<IReceiptVoucherDetailQueries, ReceiptVoucherDetailQueries>();
            services.AddTransient<IOutDebtManagementQueries, OutDebtManagementQueries>();
            services.AddTransient<IPaymentVoucherRepository, PaymentVoucherRepository>();
            services.AddTransient<ITemporaryGeneratingVoucherRepository, TemporaryGeneratingVoucherRepository>();
            services.AddTransient<IPromotionForReceiptVoucherRepository, PromotionForReceiptVoucherRepositories>();
            services.AddTransient<IVoucherAutoGenerateHistoryRepository, VoucherAutoGenerateHistoryRepository>();
            services.AddTransient<IVoucherAutoGenereateQueries, VoucherAutoGenerateQueries>();

            return services;
        }

        public static IServiceCollection InjectGRPCServices(this IServiceCollection services)
        {
            //GRPC services
            services.AddTransient<ITaxCategoryGrpcClientService, TaxCategoryGrpcClientService>();
            services.AddTransient<INotificationGrpcService, NotificationGrpcService>();
            services.AddTransient<IOutContractGrpcService, OutContractGrpcService>();
            services.AddTransient<IProjectGrpcService, ProjectGrpcService>();
            services.AddTransient<IMarketAreaGrpcService, MarketAreaGrpcService>();
            services.AddTransient<ITelcoServiceGrpcService, TelcoServiceGrpcService>();
            services.AddTransient<ITelcoServicePackageGrpcService, TelcoSrvPackageGrpcClientService>();
            services.AddTransient<IConfigurationSystemParameterGrpcService, ConfigurationSystemParameterGrpcService>();
            services.AddTransient<IOrganizationUnitGrpcService, OrganizationUnitGrpcService>();
            services.AddTransient<IExchangeRateGrpcService, ExchangeRateGrpcService>();

            return services;
        }
    }
}
