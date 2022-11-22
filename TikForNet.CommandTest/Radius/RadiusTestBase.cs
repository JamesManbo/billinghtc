using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ContractManagement.RadiusDomain.Models;
using Microsoft.EntityFrameworkCore;
using ContractManagement.RadiusDomain.Repositories;

namespace TikForNet.CommandTest.Radius
{
    public class RadiusTestBase
    {
        protected IServiceCollection Services { get; }
        public RadiusTestBase()
        {
            var services = new ServiceCollection();

            services.AddDbContext<RadiusContext>(options =>
            {
                options.UseMySql("server=192.168.1.247;database=radius;user=htc-itc;password=Ht@$2020Itc;persistsecurityinfo=True;allowloadlocalinfile=True;allowuservariables=True;Convert Zero Datetime=True;TreatTinyAsBoolean=False");
            });

            services.AddTransient<IRadiusManagementRepository, RadiusManagementRepository>();
        }

        //public TestServer CreateServer()
        //{
        //    var path = Assembly.GetAssembly(typeof(RadiusTestBase))
        //        .Location;

        //    var hostBuilder = new WebHostBuilder()
        //        .UseContentRoot(Path.GetDirectoryName(path))
        //        .ConfigureAppConfiguration(cb =>
        //        {
        //            cb.AddJsonFile("Radius/appsettings.json", optional: false)
        //                .AddEnvironmentVariables();
        //        }).UseStartup<Startup>();

        //    var testServer = new TestServer(hostBuilder);

        //    return testServer;
        //}
    }
}
