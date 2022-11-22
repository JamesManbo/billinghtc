using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using CsvHelper;
using CsvHelper.Configuration;
using Location.API.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoClusterRepository;
using MongoDB.Driver;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Location.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace;
        public static readonly string SeedDataFileName = "Localtion_Seed_Data";

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildHostBuilder(configuration, args);

                //Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                //host.MigrateDbContext<BillingDbContext>((_, __) => { })
                //    .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);

                using (var scope = host.Services.CreateScope())
                {
                    var provider = scope.ServiceProvider;
                    try
                    {
                        var mongoDbContext = provider.GetRequiredService<ILocationkMongoDbContext>();
                        var collectionCluster = mongoDbContext.GetCollection<LocationModel>();
                        SeedData(collectionCluster);
                    }
                    catch (Exception e)
                    {
                        throw;
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build();

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

        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 81);
            var port = config.GetValue("PORT", 80);
            return (port, grpcPort);
        }

        private static void SeedData(IMongoClusterCollection<LocationModel> collectionCluster)
        {
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            var pathOfFile = Path.Combine(dirPath, "SeedData", SeedDataFileName + ".csv");

            if (!File.Exists(pathOfFile))
            {
                throw new ArgumentException($"The file {SeedDataFileName} does not exist(path: {pathOfFile}");
            }

            using (var reader = new StreamReader(pathOfFile))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound = null,
                    HasHeaderRecord = true
                }))
                {
                    var seedData = new List<InsertOneModel<LocationModel>>();
                    csv.Read();
                    csv.ReadHeader();
                    var records = csv.GetRecords<LocationModel>();
                    foreach (var record in records)
                    {
                        record.Code = ResolveLocationCode(record.Code);
                        seedData.Add(new InsertOneModel<LocationModel>(record));
                    }

                    foreach (var collection in collectionCluster.GetCollections())
                    {
                        if (collection.EstimatedDocumentCount() == 0)
                        {
                            collection.BulkWrite(seedData);
                        }
                    }
                }
            }
        }

        private static string ResolveLocationCode(string source)
        {
            source = Regex.Replace(source, @"\s+", "");
            if (source.StartsWith("tinh"))
            {
                source = source.Replace("tinh", string.Empty);
            }
            if (source.StartsWith("thanhpho"))
            {
                source = source.Replace("thanhpho", string.Empty);
            }
            return source;
        }
    }
}
