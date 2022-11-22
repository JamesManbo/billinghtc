using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;
using System.Net;

namespace SystemUserIdentity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            var host = BuildHostBuilder(configuration, args);
            host.Run();
        }

        public static IHost BuildHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(configuration);
                    webBuilder.ConfigureKestrel(options =>
                    {
                        var ports = GetDefinedPorts(configuration);
                        options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        });

                        options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });

                    });

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                })
                .Build();

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
            else if (environment == Environments.Staging)
            {
                builder
                    .AddJsonFile("appsettings.Staging.json", optional: true, reloadOnChange: true);
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
