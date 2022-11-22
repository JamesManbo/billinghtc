using ContractManagement.RadiusDomain.Models;
using ContractManagement.RadiusDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TikForNet.CommandTest.Radius
{
    public class RadiusUserTest //: RadiusTestBase
    {
        //private readonly IRadiusManagementRepository _radiusManagementUserRepository;
        public RadiusUserTest()
        {
            //CreateServer();
            //_radiusManagementUserRepository = new RadiusManagementRepository(new RadiusContext());
        }

        [Fact]
        public async Task CreateRmUser()
        {
            //var radiusUser = new RmUsers()
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
            //};

            //await this._radiusManagementUserRepository.CreateRmUser(radiusUser);

            Assert.False(false);
        }


    }
}
