using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using GenericRepository;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IUserRepository : ICrudRepository<ApplicationUser, int>
    {
        Task<ApplicationUser> FindByIdentityGuidAsync(string identityGuid);
        Task<ApplicationUser> FindByUserIdentityGuidAsync(string userIdentityGuid);
        Task<ApplicationUser> FindByUserNameAsync(string name);
        string GenerateJwtToken(ApplicationUser user);
        bool CheckExitUserName(string userName, int id = 0);
        bool CheckExitUserCode(string userName, int id = 0);
        bool CheckExitMobile(string mobilePhone, int id = 0);
        bool CheckExitFaxNo(string faxNo, int id = 0);
        bool CheckExitEmail(string email, int id = 0);
        bool CheckExitIdNo(string idNo, int id = 0);
        bool CheckExitTaxIdNo(string taxIdNo, int id = 0);
        bool CheckExitBusinessRegCertificate(string businessRegCertificate, int id = 0);
        bool CheckExitPartner(string userIdentityGuid, int id = 0);
    }
}