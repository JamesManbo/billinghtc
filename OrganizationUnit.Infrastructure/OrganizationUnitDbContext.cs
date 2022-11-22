using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Infrastructure.Seed;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using OrganizationUnit.Domain.AggregateModels.FCMAggregate;
using OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate;
using OrganizationUnit.Domain.AggregateModels.OTPAggregate;
using OrganizationUnit.Domain.AggregateModels.PictureAggregate;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.RoleAggregate;
using OrganizationUnit.Infrastructure.EntityConfigurations;

namespace OrganizationUnit.Infrastructure
{
    public partial class OrganizationUnitDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit> OrganizationUnits { get; set; }
        //public virtual DbSet<OrganizationUnitJobPosition> OrganizationUnitJobPositions { get; set; }
        //public virtual DbSet<JobPosition> JobPositions { get; set; }
        //public virtual DbSet<JobTitle> JobTitles { get; set; }
        public virtual DbSet<ConfigurationPersonalAccount> ConfigurationPersonalAccounts { get; set; }

        public virtual DbSet<ConfigurationSystemParameter> ConfigurationSystemParameters { get; set; }
        public virtual DbSet<OrganizationUnit.Domain.AggregateModels.PictureAggregate.Picture> Pictures { get; set; }
        public virtual DbSet<FCMToken> FCMTokens { get; set; }
        public virtual DbSet<OtpEntity> Otps { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }

        private readonly IMediator _mediator;

        private IDbContextTransaction _currentTransaction;

        public OrganizationUnitDbContext(DbContextOptions<OrganizationUnitDbContext> options)
            : base(options)
        {
        }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigurationSystemEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new OrganizationUnitEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new UserBankAccountEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationUnitUsersEntityTypeConfiguration());
            modelBuilder.SeedData();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync();

            return _currentTransaction;
        }


        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
