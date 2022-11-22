using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace CMS.APIGateway
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace;

        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            CreateHostBuilder(args).Build().Run();
        }
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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureServices(s => s.AddSingleton(webBuilder))
                        .ConfigureAppConfiguration(ic =>
                        {
                            var configurationFilePath = Path.Combine("Configuration", "configuration.json");
                            ic.AddJsonFile(configurationFilePath);
                        });

                    webBuilder
                    .UseSerilog()
                    .UseStartup<Startup>();
                });
    }
}