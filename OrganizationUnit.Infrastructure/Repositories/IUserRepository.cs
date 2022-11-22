using System;
using System.Threading;
using System.Threading.Tasks;
using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public interface IUserRepository: ICrudRepository<User, int>
    {
        Task SetSecurityStampAsync(User user, string stamp,
            CancellationToken cancellationToken = default(CancellationToken));

        Task SetPasswordHashAsync(User user, string passwordHash,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<User> FindByIdentityGuidAsync(string identityGuid);
        Task<User> FindByApplicationUserIdentityGuidAsync(string applicationUserIdentityGuid);
        Task<User> FindByUserNameAsync(string name);

        bool CheckExitUserName(string userName, int id);
        bool CheckExitPhoneNumber(string phoneNumber, int id);
        bool CheckExitEmail(string email, int id);
        bool CheckExitIdNumber(string idNumber, int id);
        bool CheckExitIdTaxNumber(string idNumber, int id);
        bool CheckExistedCode(string code, int id);
        bool CheckExitCustomer(string applicationUserIdentityGuid, int id);
        bool AddConfigUsers(int userId);
    }
}
