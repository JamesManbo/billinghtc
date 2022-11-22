using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Infrastructure.Seed;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.Otp;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApplicationUserIdentity.API.Infrastructure
{
    public class ApplicationUserDbContext : DbContext
    {
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ApplicationUserClass> ApplicationUserClass { get; set; }
        public DbSet<Models.Picture> Pictures { get; set; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { get; set; }
        public DbSet<ApplicationUserUserGroup> ApplicationUserUserGroups { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<CustomerCategory> CustomerCategories { get; set; }
        public DbSet<CustomerStructure> CustomerStructures { get; set; }
        public DbSet<FCMToken> FCMTokens { get; set; }
        public DbSet<OtpEntity> Otps { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<ApplicationUserIndustry> ApplicationUserIndustries { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options)
        {
        }

        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<Models.Picture>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<ApplicationUserClass>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<ApplicationUserGroup>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<ApplicationUserUserGroup>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<CustomerCategory>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<CustomerStructure>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<CustomerType>().Ignore(e => e.DomainEvents);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<Models.Picture>()
                .WithMany()
                .HasForeignKey(e => e.AvatarId)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationUserGroup>()
                .HasMany<ApplicationUserUserGroup>()
                .WithOne()
                .HasForeignKey(sc => sc.GroupId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<ApplicationUserUserGroup>()
                .WithOne()
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<CustomerCategory>()
                .WithMany()
                .HasForeignKey(sc => sc.CustomerCategoryId)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<CustomerType>()
                .WithMany()
                .HasForeignKey(sc => sc.CustomerTypeId)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<CustomerStructure>()
                .WithMany()
                .HasForeignKey(sc => sc.CustomerStructureId)
                .IsRequired(false);

            modelBuilder.Entity<Industry>()
                .HasMany<ApplicationUserIndustry>()
                .WithOne()
                .HasForeignKey(sc => sc.IndustryId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<ApplicationUserIndustry>()
                .WithOne()
                .HasForeignKey(sc => sc.UserId);

            //modelBuilder.SeedData();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
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
            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

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
    }
}