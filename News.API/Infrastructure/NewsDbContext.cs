using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using News.API.Models;

namespace News.API.Infrastructure
{
    public class NewsDbContext : DbContext
    {
        public DbSet<Models.Picture> Pictures { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<ArticleArticleCategory> ArticleArticleCategories { get; set; }
        public virtual DbSet<FileAttachment> FileAttachments { get; set; }

        private readonly IMediator _mediator;

        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<ArticleCategory>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<ArticleArticleCategory>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<Models.Picture>().Ignore(e => e.DomainEvents);

            modelBuilder.Entity<Article>()
               .HasOne<Models.Picture>()
               .WithMany()
               .HasForeignKey(e => e.AvatarId)
               .IsRequired(false);

            modelBuilder.Entity<ArticleCategory>()
                .HasMany<ArticleArticleCategory>()
                .WithOne()
                .HasForeignKey(sc => sc.ArticleCategoryId);

            modelBuilder.Entity<Article>()
                .HasMany<ArticleArticleCategory>()
                .WithOne()
                .HasForeignKey(sc => sc.ArticleId);
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
            //var result = await base.SaveChangesAsync(cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
            //return true;
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
