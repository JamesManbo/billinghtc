using ContractManagement.RadiusDomain.Models;
using ContractManagement.RadiusDomain.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikForNet.Objects;
using TikForNet.Objects.Ppp;
using Xunit;
using Xunit.Abstractions;

namespace TikForNet.CommandTest
{
    public class PppTest : TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IRadiusManagementRepository _radiusManagementUserRepository;

        public PppTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            RecreateConnection();

            _radiusManagementUserRepository = new RadiusManagementRepository(new RadiusContext());
        }

        [Fact]
        public void LoadPppActiveWilNotFail()
        {
            var result = new List<PppActive>();
            foreach (var conn in Connections)
            {
                result.AddRange(conn.LoadAll<PppActive>());
            }

            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void LoadPppSecretWilNotFail()
        {
            var result = Execute(conn => conn.LoadAll<PppSecret>());
            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void CreateAndDeletePppSecretWilNotFail()
        {
            var before = Execute(conn => conn.LoadAll<PppSecret>());
            var newSecret = new PppSecret()
            {
                Name = "[SONND]Test",
            };

            Execute(conn => conn.Save(newSecret));
            Execute(conn => conn.Delete(newSecret));
            var after = Execute(conn => conn.LoadAll<PppSecret>());
            Assert.Equal(before.Count(), after.Count());

            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(after));
        }

        [Fact]
        public void LoadPppProfileWilNotFail()
        {
            var result = Execute(conn => conn.LoadAll<PppProfile>());
            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void CreateAndDeletePppProfileWilNotFail()
        {
            var before = Execute(conn => conn.LoadAll<PppProfile>());
            var newProfile = new PppProfile()
            {
                Name = "ghd-profile",
            };

            Execute(conn => conn.Save(newProfile));
            // Connection.Delete(newProfile);
            var after = Execute(conn => conn.LoadAll<PppProfile>());
            // Assert.Equal(before.Count(), after.Count());
            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(after));
        }

        [Fact]
        public async Task DeletePppActiveProfileWillNotFail()
        {
            var targetUserName = "ghd-test";
            var targetPppActives = Execute(conn => conn.LoadSingle<PppActive>(
                conn.CreateParameter("name", targetUserName, TikCommandParameterFormat.Filter)));
            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(targetPppActives));

            Assert.NotNull(targetPppActives);
            Assert.NotEmpty(targetPppActives);

            Assert.True(await _radiusManagementUserRepository.DeactivateUserByUserName(targetUserName));

            Execute(conn =>
            {
                foreach (var target in targetPppActives)
                {
                    conn.Delete(target);
                }
            });

            Assert.Throws<TikNoSuchItemException>(() =>
            {
                Execute(conn =>
                    conn.LoadSingle<PppActive>(conn.CreateParameter("name", "billing1", TikCommandParameterFormat.Filter)));
            });
        }

        [Fact]
        public async Task ActiveAllRadiusUsers()
        {
            var targetUserNames = new string[] { "billing1", "billing2", "billing3", "ghd-test" };
            Assert.True(await _radiusManagementUserRepository.MultipleActivateUserByUserName(targetUserNames));
        }

        [Fact]
        public void LoadPppAaaWilNotFail()
        {
            var result = Execute(conn => conn.LoadSingle<PppAaa>());
            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task UpgradeServiceMultipleUsers()
        {
            var targetUserNames = new string[] { "billing1", "billing2", "billing3", "ghd-test" };
            Assert.True(await _radiusManagementUserRepository.MultipleUpgradeSrvByUserName(targetUserNames, 0));

            Execute(conn => {
                var allActives = conn.LoadAll<PppActive>();
                var targets = allActives.Where(a => targetUserNames.Contains(a.Name.ToLower()));
                foreach (var target in targets)
                {
                    conn.Delete(target);
                }
            });
        }
    }
}
