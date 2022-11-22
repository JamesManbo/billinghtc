using Global.Models.StateChangedResponse;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Infrastructure.Services
{
    public interface IUserService
    {
        Task<ActionResponse> CreateAsync(User user);
        Task<ActionResponse> CreateAsync(User user, string password);
        Task<ActionResponse> UpdateAsync(User user, string password);
        Task<ActionResponse> UpdateWithoutPasswordAsync<T>(T user) where T: class;
        IActionResponse DeleteAndSave(int id);
        Task<User> GetByIdAsync(int id);
        bool CheckExitUserName(string userName, int id);
        bool CheckExitPhoneNumber(string phoneNumber, int id);
        bool CheckExitEmail(string email, int id);
        bool CheckExitIdNumber(string idNumber, int id);
        bool CheckExitIdTaxNumber(string idNumber, int id);
        bool CheckExistedCode(string code, int id = 0);
        bool CheckExitCustomer(string applicationUserIdentityGuid, int id);
        bool IsExisted(int id);
        bool AddConfigUsers(int useId);
    }
}
