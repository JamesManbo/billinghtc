using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.BackgroundTasks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            //services.AddCustomHealthCheck(this.Configuration)
            //    .Configure<BackgroundTaskSettings>(this.Configuration)
            //    .AddOptions()
            //    .AddHostedService<GracePeriodManagerService>()
            //    .AddEventBus(this.Configuration);
        }


        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
            //    {
            //        Predicate = _ => true,
            //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //    });

            //    endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
            //    {
            //        Predicate = r => r.Name.Contains("self")
            //    });
            //});
        }
    }
}
