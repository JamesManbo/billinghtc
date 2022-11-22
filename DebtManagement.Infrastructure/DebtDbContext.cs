using DebtManagement.Infrastructure.EntityConfigurations;
using DebtManagement.Infrastructure.Seed;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;

namespace DebtManagement.Infrastructure
{
    public partial class DebtDbContext : DbContext
    {
        public virtual DbSet<TemporaryGeneratingVoucher> TemporaryGeneratingVouchers { get; set; }
        public virtual DbSet<ReceiptVoucherType> PaymentVoucherTypes { get; set; }
        public virtual DbSet<PaymentVoucher> PaymentVouchers { get; set; }
        public virtual DbSet<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }
        public virtual DbSet<PaymentMethodType> PaymentMethodTypes { get; set; }
        public virtual DbSet<PaymentVoucherStatus> PaymentVoucherStatuses { get; set; }
        public virtual DbSet<ReceiptVoucher> ReceiptVouchers { get; set; }
        public virtual DbSet<ReceiptVoucherDebtHistory> ReceiptVoucherDebtHistories { get; set; }
        public virtual DbSet<ReceiptVoucherPaymentDetail> ReceiptVoucherPaymentDetails { get; set; }
        public virtual DbSet<ReceiptVoucherDetail> ReceiptVoucherDetails { get; set; }
        public virtual DbSet<ReceiptVoucherStatus> ReceiptVoucherStatuses { get; set; }
        public virtual DbSet<ReceiptVoucherType> ReceiptVoucherTypes { get; set; }
        public virtual DbSet<ClearingStatus> ClearingStatuses { get; set; }
        public virtual DbSet<Clearing> Clearing { get; set; }
        public virtual DbSet<VoucherTarget> VoucherTargets { get; set; }
        public virtual DbSet<VoucherTargetProperty> VoucherTargetProperties { get; set; }
        public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public virtual DbSet<VoucherPaymentMethod> VoucherPaymentMethods { get; set; }
        public virtual DbSet<PromotionForReceiptVoucher> PromotionForReceiptVouchers { get; set; }
        public virtual DbSet<CurrencyUnit> CurrencyUnits { get; set; }
        public virtual DbSet<ReceiptVoucherDetailReduction> ReceiptVoucherDetailReductions { get; set; }
        public virtual DbSet<VoucherAutoGenerateHistory> VoucherAutoGenerateHistories { get; set; }
        public virtual DbSet<ReceiptVoucherInPaymentVoucher> ReceiptVoucherInPaymentVouchers { get; set; }
        public virtual DbSet<ChannelPriceBusTable> ChannelPriceBusTables { get; set; }
        public virtual DbSet<BusTablePricingCalculator> BusTablePricingCalculators { get; set; }

        private readonly IMediator _mediator;

        private IDbContextTransaction _currentTransaction;

        public DebtDbContext(DbContextOptions<DebtDbContext> options) : base(options)
        {
        }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public DebtDbContext(DbContextOptions<DebtDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ReceiptVoucherEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptVoucherDetailEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentVoucherEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentVoucherDetailEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ClearingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherTargetEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentFileEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptVoucherDebtHistoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TemporaryGeneratingVoucherEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelPriceBusTableEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BusTablePricingCalculatorEntityConfiguration());

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
