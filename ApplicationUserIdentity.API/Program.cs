using System;
using System.IO;
using System.Net;
using ApplicationUserIdentity.API.Infrastructure;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using IntegrationEventLogEF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace ApplicationUserIdentity.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildHostBuilder(configuration, args);

                //Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                //host.MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var environment = services.GetRequiredService<IWebHostEnvironment>();

                        var logDbContext = services.GetRequiredService<IntegrationEventLogDbContext>();
                        logDbContext.Database.Migrate();

                        var contractDbContext = services.GetRequiredService<ApplicationUserDbContext>();
                        contractDbContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(ex, "An error occurred creating/updating the DB. ({ApplicationContext})!", AppName);
                    }
                }

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                Console.WriteLine(ex);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHost BuildHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(configuration);

                    webBuilder.ConfigureKestrel(options =>
                    {
                        var ports = GetDefinedPorts(configuration);
                        options.Listen(IPAddress.Any, ports.httpPort,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });

                        options.Listen(IPAddress.Any, ports.grpcPort,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });

                    });

                    webBuilder.UseSerilog();
                    webBuilder.UseStartup<Startup>();
                }).Build();

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var elasticServerUri = configuration["Serilog:SeqServerUrl"];

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticServerUri))
                {
                    AutoRegisterTemplate = true
                })
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == Environments.Development)
            {
                builder
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            }
            else
            {
                builder
                    .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
            }


            return builder.Build();
        }

        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 81);
            var port = config.GetValue("PORT", 80);
            return (port, grpcPort);
        }
    }
}
