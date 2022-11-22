using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.BackgroundTasks.Extensions
{
    public static class CustomExtensionMethods
    {
        public static ILoggingBuilder UseSerilog(this ILoggingBuilder builder, IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", Program.AppName)
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(seqServerUrl))
                {
                    AutoRegisterTemplate = true
                })
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return builder;
        }
    }
}
