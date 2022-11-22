using ContractManagement.API.Application.Services.MikroTik;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ContractManagement.RadiusDomain.Models;
using ContractManagement.RadiusDomain.Repositories;
using System.Linq;

namespace TikForNet.FuntionalTests.RadiusManagement
{
    public class RadiusUserMngrTest : RadiusManagementTestBase
    {
        private readonly IRadiusManagementRepository _radiusManagementRepository;
        private readonly IMikroTikService _mikroTikService;
        public RadiusUserMngrTest()
        {
            _mikroTikService = ServiceProvider.GetService<IMikroTikService>();
            _radiusManagementRepository = ServiceProvider.GetService<IRadiusManagementRepository>();
        }

        [Fact]
        public async Task CreateRmUsers()
        {
            int numberOfUsers = 1600;
            for (int i = 0; i < numberOfUsers; i++)
            {
                var testRmUser = new RmUsers()
                {
                    Username = $"test{i}",
                    Password = $"test{i}",
                    Country = "Viet Nam",
                    Downlimit = 12000000,
                    Uplimit = 5000000,
                    Address = $"Số {i}, Ngõ {i} Duy Tân, Dịch Vọng Hậu, Cầu Giấy, Hà Nội",
                    Contractid = "290920-1/NORTH-DRCN/FAMILYPLUS/VTQT-NTN",
                    Firstname = $"Test{i}",
                    Lastname = "Nguyễn Văn",
                    Phone = $"0335905{i.ToString("D3")}",
                    Mobile = $"0335905{i.ToString("D3")}",
                    Verifymobile = $"0335905{i.ToString("D3")}",
                    Enableuser = 1,
                    Email = $"dangson{i}@gmail.com",
                    Actcode = "",
                    Acctype = 0,
                    Contractvalid = DateTime.Now.AddYears(100),
                    Alertemail = 0,
                    Alertsms = 0,
                    City = "Hanoi",
                    Lang = "English",
                    Ipmodecpe = 1,
                    Staticipcpe = "",
                    Srvid = 0
                };

                await _radiusManagementRepository.CreateRmUser(testRmUser);
                //await _radiusManagementRepository.RemoveRmUser(testRmUser.Username);

                //var checkExistAgain = _radiusManagementRepository.IsUserExisted(testRmUser.Username);
                //Assert.False(checkExistAgain);
            }

            var createdTestUser = await _radiusManagementRepository.GetAllTestUsers();

            Assert.True(createdTestUser != null && createdTestUser.Count() >= numberOfUsers);
        }

        [Fact]
        public async Task DeactiveRmUsres()
        {
            var allTestUsers = await _radiusManagementRepository.GetAllTestUsers();
            var targetUserNames = allTestUsers.Select(c => c.Username).ToArray();
            await _radiusManagementRepository.MultipleDeactivateUserByUserNames(targetUserNames);
        }

        [Fact]
        public async Task ActiveRmUsers()
        {
            var allTestUsers = await _radiusManagementRepository.GetAllTestUsers();
            var targetUserNames = allTestUsers.Select(c => c.Username).ToArray();
            await _radiusManagementRepository.MultipleActivateUserByUserName(targetUserNames);
        }

        [Fact]
        public async Task DeleteTestUsers()
        {
            var allTestUsers = await _radiusManagementRepository.GetAllTestUsers();
            await _radiusManagementRepository.PureDeleteUsers(allTestUsers);

        }
    }
}
