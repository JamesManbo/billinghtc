using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractManagement.API.Application.Services.MikroTik;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ContractManagement.RadiusDomain.Models;
using System.Threading.Tasks;
using ContractManagement.RadiusDomain.Repositories;
using ContractManagement.Infrastructure.Repositories.RadiusAndBrasRepository;

namespace ContractManagement.API.Application.Services.MikroTik.UnitTests
{
    [TestClass()]
    public class MikroTikServiceCreateBrasInfo
    {
        //private readonly IMikroTikService _mikroTikService;
        //private readonly IRadiusManagementRepository _radiusManagementUserRepository;
        //private readonly IRadiusServerInfoRepository _radiusServerInfoRepository;
        //private readonly IBrasInfoRepository _brasInfoRepository;

        public MikroTikServiceCreateBrasInfo()
        {
            //var services = new ServiceCollection();
            //services.AddTransient<IRadiusManagementRepository, RadiusManagementRepository>();
            //services.AddTransient<IRadiusServerInfoRepository, RadiusServerInfoRepository>();
            //services.AddTransient<IBrasInfoRepository, BrasInfoRepository>();
            //services.AddTransient<IMikroTikService, MikroTikService>();

            //var serviceProvider = services.BuildServiceProvider();
            //_mikroTikService = serviceProvider.GetService<IMikroTikService>();
            //_radiusManagementUserRepository = serviceProvider.GetService<IRadiusManagementRepository>();
            //_radiusServerInfoRepository = serviceProvider.GetService<IRadiusServerInfoRepository>();
            //_brasInfoRepository = serviceProvider.GetService<IBrasInfoRepository>();
        }

        [TestMethod()]
        public async Task CreateRadiusUserCreateBrasInfo()
        {
            //await _mikroTikService.CreateRadiusUser(new RmUsers()
            //{
            //    Username = "ghd-test02",
            //    Password = "123123",
            //    Country = "Viet Nam",
            //    Downlimit = 12000000,
            //    Uplimit = 5000000,
            //    Address = "Số 6, Ngõ 82 Duy Tân, Dịch Vọng Hậu, Cầu Giấy, Hà Nội",
            //    Contractid = "290920-1/NORTH-DRCN/FAMILYPLUS/VTQT-NTN",
            //    Firstname = "Sơn",
            //    Lastname = "Nguyễn Đăng",
            //    Phone = "0335905133",
            //    Mobile = "0335905133",
            //    Verifymobile = "0335905133",
            //    Enableuser = 1,
            //    Email = "dangson@gmail.com",
            //    Actcode = "",
            //    Acctype = 0,
            //    Contractvalid = DateTime.Now.AddYears(100),
            //    Alertemail = 0,
            //    Alertsms = 0,
            //    City = "Hanoi",
            //    Lang = "English",
            //    Ipmodecpe = 3,
            //    Staticipcpe = "171.244.9.248",
            //    Srvid = 2
            //});

            //var testUserExisted = _radiusManagementUserRepository.IsUserExisted("ghd-test02");

            //Assert.IsTrue(testUserExisted);
            Assert.Fail();
        }
    }
}