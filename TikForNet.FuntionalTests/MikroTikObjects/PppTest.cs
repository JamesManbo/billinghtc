using ContractManagement.API.Application.Services.Radius;
using ContractManagement.Infrastructure;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.RadiusDomain.Models;
using ContractManagement.RadiusDomain.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TikForNet.FuntionalTests.MikroTikObjects;
using TikForNet.Objects;
using TikForNet.Objects.Ppp;
using Xunit;
using Xunit.Abstractions;

namespace TikForNet.FuntionalTests
{
    public class PppTest : MikroTickTestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IRadiusAndBrasManagementService _radiusManagementService;
        private readonly IRadiusManagementRepository _radiusManagementUserRepository;
        private readonly IRadiusServerInfoQueries _radiusServerInfoQueries;

        public PppTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var dbContextOpt = new DbContextOptionsBuilder<ContractDbContext>();
            dbContextOpt.UseMySql("server=192.168.1.247;database=ITC_FBM_Contracts;user=htc-itc;password=Ht@$2020Itc;persistsecurityinfo=True;allowloadlocalinfile=True;allowuservariables=True", x => x
                    .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                    .MigrationsAssembly(typeof(ContractDbContext).GetTypeInfo().Assembly.GetName().Name));

            _radiusServerInfoQueries = new RadiusServerInfoQueries(new ContractDbContext(dbContextOpt.Options));
            _radiusManagementUserRepository = new RadiusManagementRepository(new RadiusContext());

            //_radiusManagementService = new RadiusAndBrasManagementService(_radiusServerInfoQueries, _radiusManagementUserRepository);
            RecreateConnection();
        }

        [Fact]
        public void TestConnection()
        {
            RecreateConnection();
            Execute(conn =>
            {
                Assert.True(conn.IsOpened);
            });
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

            var deleteBrasSessions = await _radiusManagementUserRepository.DeactivateUserByUserName(targetUserName);
            Assert.True(deleteBrasSessions.IsSuccess);

            Execute(conn =>
            {
                foreach (var target in targetPppActives)
                {
                    try
                    {
                        conn.Delete(target);
                    }
                    catch (TikNoSuchItemException)
                    {
                        continue;
                    }
                }
            });

            Assert.Throws<TikNoSuchItemException>(() =>
            {
                Execute(conn =>
                    conn.LoadSingle<PppActive>(conn.CreateParameter("name", "billing1", TikCommandParameterFormat.Filter)));
            });
        }

        [Fact]
        public async Task ActiveAllTestUsers()
        {
            var allTestUsers = await _radiusManagementUserRepository.GetAllTestUsers();
            var targetUserNames = allTestUsers.Select(u => u.Username).ToArray();
            var activateBrasSessRsp = await _radiusManagementUserRepository.MultipleActivateUserByUserName(targetUserNames);
            Assert.True(activateBrasSessRsp.IsSuccess);
        }

        [Fact]
        public async Task DeactivateAllTestUsers()
        {
            //var allTestUsers = await _radiusManagementUserRepository.GetAllTestUsers();
            //var targetUserNames = allTestUsers.Select(u => u.Username).ToArray();
            var targetUserNames = new string[] { "Billingtest_100MB" };
            //Assert.True(await _radiusManagementUserRepository.MultipleDeactivateUserByUserNames(targetUserNames));

            var targetPppActives = Execute(conn =>
            {
                var allPppActives = conn.LoadAll<PppActive>();
                var results = allPppActives.Where(p => targetUserNames.Contains(p.Name));
                foreach (var pppActive in results)
                {
                    conn.Delete(pppActive);
                }
                return results;
            }).GroupBy(t => t.Name).Select(g => g.Last());

            this._testOutputHelper.WriteLine(JsonConvert.SerializeObject(targetPppActives));

            //Assert.NotNull(targetPppActives);
            //Assert.NotEmpty(targetPppActives);

            Execute(conn =>
            {
                foreach (var target in targetPppActives)
                {
                    try
                    {
                        conn.Delete(target);
                    }
                    catch (TikNoSuchItemException)
                    {
                        continue;
                    }
                }
            });
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
            var testUsers = await _radiusManagementUserRepository.GetAllTestUsers();
            var targetUserNames = testUsers.Select(y => y.Username).ToArray();

            // Upgrade services
            var upgradeBrasSessionsRsp = await _radiusManagementUserRepository.MultipleUpgradeSrvByUserNames(targetUserNames, 33);
            Assert.True(upgradeBrasSessionsRsp.IsSuccess);
            
            // Delete Bras's active sessions of testing users
            Execute(conn =>
            {
                var allActives = conn.LoadAll<PppActive>();
                var targets = allActives.Where(a => targetUserNames.Contains(a.Name.ToLower()));
                foreach (var target in targets)
                {
                    try
                    {
                        conn.Delete(target);
                    }
                    catch (TikNoSuchItemException)
                    {
                        continue;
                    }
                }
            });

            //var chunkedTargetUsers = targetUserNames.Chunk(50);
            //foreach (var batch in chunkedTargetUsers)
            //{
            //    // Re-active all testing users
            //    Assert.True(await _radiusManagementUserRepository.MultipleActivateUserByUserName(batch.ToArray()));
            //    await Task.Delay(2 * 1000);
            //}
        }

    }

    public static class PppTestExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> list, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("chunkSize must be greater than 0.");
            }

            while (list.Any())
            {
                yield return list.Take(chunkSize);
                list = list.Skip(chunkSize);
            }
        }
    }
}
