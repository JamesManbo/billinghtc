using OrganizationUnit.Domain.Models.User;
using System.Threading.Tasks;
using SystemUserIdentity.API.Models;

namespace SystemUserIdentity.API.Infrastructure.Services
{
    public interface IAuthenticationService<T>
    {
        string GenerateJwtToken(string userName);
        Task<T> FindByUserNameAsync(string name);
        UserDTO FindById(string id);
        string EncryptPassword(string rawPassword);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task<UserIdentityResult> CreateAsync(T user);
        Task<UserIdentityResult> CreateAsync(T user, string password);
        Task<UserIdentityResult> ChangePassword(string userName, string oldPassword, string newPassword);
        Task<UserIdentityResult> ChangePasswordUser(string userName, string newPassword);
        Task<bool> LoginByAD(string userName, string password);
    }
}
