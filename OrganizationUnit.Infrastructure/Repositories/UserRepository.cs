using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public class UserRepository : CrudRepository<User, int>, IUserRepository, IDisposable
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;
        private bool _disposed;

        public UserRepository(OrganizationUnitDbContext organizationUnitDbContext,
            IWrappedConfigAndMapper configAndMapper)
            : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        public override Task<User> GetByIdAsync(object id)
        {
            return DbSet.Include(e => e.UserBankAccounts).Include(e => e.ContactInfos)
                .Include(e => e.ConfigurationAccount)
                .Include(e => e.OrganizationUnitUsers)
                .Where(e => e.Id == (int)id).FirstOrDefaultAsync();
        }

        public async Task<User> FindByIdentityGuidAsync(string identityGuid)
        {
            var user = await _organizationUnitDbContext.Users.FirstOrDefaultAsync(e =>
                e.IdentityGuid.Equals(identityGuid) && !e.IsDeleted);
            return user;
        }

        public async Task<User> FindByApplicationUserIdentityGuidAsync(string applicationUserIdentityGuid)
        {
            var user = await _organizationUnitDbContext.Users.FirstOrDefaultAsync(e =>
                e.ApplicationUserIdentityGuid.Equals(applicationUserIdentityGuid) && !e.IsDeleted);
            return user;
        }

        public async Task<User> FindByUserNameAsync(string userName)
        {
            var user = await _organizationUnitDbContext.Users.FirstOrDefaultAsync(e =>
                e.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && !e.IsDeleted);
            return user;
        }

        /// <summary>
        /// Sets the provided security <paramref name="stamp"/> for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <param name="stamp">The security stamp to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual Task SetSecurityStampAsync(User user, string stamp,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (stamp == null)
            {
                throw new ArgumentNullException(nameof(stamp));
            }

            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sets the password hash for a user.
        /// </summary>
        /// <param name="user">The user to set the password hash for.</param>
        /// <param name="passwordHash">The password hash to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetPasswordHashAsync(User user, string passwordHash,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public bool CheckExitUserName(string userName, int id)
        {
            return !_organizationUnitDbContext.Users
                .Any(x => x.UserName.Equals(userName.Trim(), StringComparison.InvariantCultureIgnoreCase)
                          && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExitPhoneNumber(string phoneNumber, int id)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

            var mobilePhones = phoneNumber.Split(",");
            return mobilePhones.Any(a => _organizationUnitDbContext.Users.Any(x => x.MobilePhoneNo.Contains(a) && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0))));
        }

        public bool CheckExitEmail(string email, int id)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var emails = email.Split(",");
            return emails.Any(a => _organizationUnitDbContext.Users.Any(x => x.Email.Contains(a) && x.IsDeleted == false && x.Id != id && ((x.Id != id && id != 0) || (id == 0))));
        }

        public bool CheckExitIdNumber(string idNumber, int id)
        {
            return !_organizationUnitDbContext.Users.Any(x =>
                x.IdNo == idNumber.Trim() && x.IsDeleted == false && x.Id != id);
        }
        public bool CheckExitIdTaxNumber(string idNumber, int id)
        {
            return !_organizationUnitDbContext.Users.Any(x =>
                x.TaxIdNo.Equals(idNumber.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                x.IsDeleted == false && x.Id != id);
        }
        public bool CheckExistedCode(string code, int id)
        {
            return !_organizationUnitDbContext.Users.Any(x =>
                x.Code.Equals(code.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                x.IsDeleted == false && x.Id != id);
        }
        public bool CheckExitCustomer(string applicationUserIdentityGuid, int id)
        {
            return !_organizationUnitDbContext.Users.Any(x =>
                x.ApplicationUserIdentityGuid.Equals(applicationUserIdentityGuid) &&
                x.IsDeleted == false && x.Id != id);
        }

        //Gán người dùng cho các thao tác thao tác cài đặt cấu hình
        public bool AddConfigUsers(int userId)
        {
            try
            {
                var cPA = new ConfigurationPersonalAccount(userId, true, true, true);
                //var cSP = new ConfigurationSystemParameter( 100, 100);
                _organizationUnitDbContext.ConfigurationPersonalAccounts.Add(cPA);
                //_organizationUnitDbContext.ConfigurationSystemParameters.Add(cSP);
                _organizationUnitDbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }
}