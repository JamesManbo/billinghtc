using System.IO;
using System.Net;
using Global.Configs.ResourceConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace StaticResource.API
{
    public class Program
    {
        public static string RootDirectory = "/resource/";
        public static void Main(string[] args)
        {
            if (!File.Exists($"{RootDirectory}{ResourceGlobalConfigs.ResourceTempFolder}"))
            {
                Directory.CreateDirectory($"{RootDirectory}{ResourceGlobalConfigs.ResourceTempFolder}");
            }

            var configuration = GetConfiguration();
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
                })
                .Build();

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

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
