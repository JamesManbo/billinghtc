using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using ContractManagement.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Solution.FunctionalTests.Business.Billing
{
    public class BillingScenariosBase
    {
        public static string BillingUrlBase => "http://localhost:5100/bl/billings";
        public BillingScenariosBase()
        {
        }
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(BillingScenariosBase))
                .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("Business/Billing/appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                }).UseStartup<Startup>();

            var testServer = new TestServer(hostBuilder);

            return testServer;
        }
    }
}
