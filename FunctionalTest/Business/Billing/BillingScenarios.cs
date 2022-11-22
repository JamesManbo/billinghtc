using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContractManagement.API.Application.Commands;
using ContractManagement.Domain.Commands.OutContractCommand;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Solution.FunctionalTests.Business.Billing;
using Xunit;
using Xunit.Abstractions;

namespace Solution.FunctionalTests.Business
{
    public class BillingScenarios : BillingScenariosBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BillingScenarios(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task CreateBilling()
        {
            using var billingApiServer = CreateServer();

            var createBillingCommand = new CreateContractCommand()
            {
                ContractStatusId = 1,
                ContractorId = null,
                Description = "Created at testing scenario",
            };

            var response = await billingApiServer.CreateClient().PostAsync(BillingUrlBase,
                new StringContent(JsonConvert.SerializeObject(createBillingCommand), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseResult = await response.Content.ReadAsStringAsync();
            _testOutputHelper.WriteLine(responseResult);
        }
    }
}
