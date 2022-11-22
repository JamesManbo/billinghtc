using System;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.SalesmanAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregateModels.PictureAggregate;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using ContractManagement.Infrastructure.Seed;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate;

namespace ContractManagement.Infrastructure
{
    public partial class ContractDbContext : DbContext
    {
        public virtual DbSet<MarketArea> MarketAreas { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ManagementBusinessBlock> ManagementBusinessBlocks { get; set; }
        public virtual DbSet<ProjectTechnician> ProjectTechnicians { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<PromotionDetail> PromotionDetails { get; set; }
        public virtual DbSet<PromotionType> PromotionTypes { get; set; }
        public virtual DbSet<TaxCategory> TaxCategories { get; set; }
        public virtual DbSet<CardType> CardTypes { get; set; }
        public virtual DbSet<Salesman> Salesmen { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<ServiceGroup> ServiceGroups { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServicePackage> ServicePackages { get; set; }
        public virtual DbSet<ServicePackagePrice> ServicePackagePrice { get; set; }
        public virtual DbSet<PointType> PointTypes { get; set; }
        public virtual DbSet<InContractType> InContractTypes { get; set; }
        public virtual DbSet<InContract> InContracts { get; set; }
        //public virtual DbSet<InContractService> InContractServices { get; set; }
        public virtual DbSet<OutContractType> OutContractTypes { get; set; }
        public virtual DbSet<OutContract> OutContracts { get; set; }
        public virtual DbSet<OutContractServicePackage> OutContractServicePackages { get; set; }
        public virtual DbSet<ContractEquipment> OutContractEquipments { get; set; }
        public virtual DbSet<EquipmentStatus> EquipmentStatuses { get; set; }
        public virtual DbSet<ContractStatus> ContractStatuses { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<ContractSharingRevenueLine> ContractSharingRevenues { get; set; }
        public virtual DbSet<SharingRevenueLineDetail> SharingRevenueLineDetails { get; set; }
        public virtual DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public virtual DbSet<Transaction> TransactionSTransactionServicePackages { get; set; }
        public virtual DbSet<TransactionServicePackage> TransactionServicePackages { get; set; }
        public virtual DbSet<TransactionChannelPoint> TransactionChannelPoints { get; set; }
        public virtual DbSet<TransactionEquipment> TransactionEquipments { get; set; }
        public virtual DbSet<TransactionChannelTax> TransactionChannelTaxes { get; set; }
        public virtual DbSet<TransactionServiceLevelAgreement> TransactionServiceLevelAgreements { get; set; }
        public virtual DbSet<TransactionPromotionForContract> TransactionPromotionForContracts { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
        public virtual DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public virtual DbSet<TransactionReason> ReasonSuspensions { get; set; }
        public virtual DbSet<EquipmentPicture> EquipmentPictures { get; set; }
        public virtual DbSet<InContractTax> InContractTaxes { get; set; }
        public virtual DbSet<OutContractServicePackageTax> OutContractServicePackageTaxes { get; set; }
        public virtual DbSet<ServiceLevelAgreement> ServiceLevelAgreements { get; set; }
        public virtual DbSet<PromotionProduct> PromotionProducts { get; set; }
        public virtual DbSet<PromotionForContract> PromotionForContracts { get; set; }
        public virtual DbSet<ServicePackageSuspensionTime> ServicePackageSuspensionTimes { get; set; }
        public virtual DbSet<OutContractServicePackageClearing> OutContractServicePackageClearings { get; set; }
        public virtual DbSet<OutContractServicePackageStatus> OutContractServicePackageStatuses { get; set; }
        public virtual DbSet<PromotionValueType> PromotionValueTypes { get; set; }
        public virtual DbSet<RadiusServerInformation> RadiusServerInformation { get; set; }
        public virtual DbSet<BrasInformation> BrasInformation { get; set; }
        public virtual DbSet<ServicePackageRadiusService> ServicePackageRadiusServices { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<TemporaryPayingContract> TemporaryPayingContracts { get; set; }
        public virtual DbSet<CurrencyUnit> CurrencyUnits { get; set; }
        public virtual DbSet<ContractForm> ContractForms { get; set; }
        public virtual DbSet<ContractContent> ContractContents { get; set; }
        public virtual DbSet<OutputChannelPoint> OutputChannelPoints { get; set; }
        public virtual DbSet<ChannelGroups> ChannelGroups { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<ChannelPriceBusTable> ChannelPriceTables { get; set; }
        public virtual DbSet<TransactionPriceBusTable> TransactionPriceBusTables { get; set; }
        //public virtual DbSet<PackagePriceBusTable> PackagePriceTables { get; set; }
        public virtual DbSet<ContractorProperties> ContractorProperties { get; set; }

        private readonly IMediator _mediator;

        private IDbContextTransaction _currentTransaction;
        public ContractDbContext(DbContextOptions<ContractDbContext> options) : base(options)
        {
        }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public ContractDbContext(DbContextOptions<ContractDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContractorEntityConfiguration());

            modelBuilder.ApplyConfiguration(new InContractEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new InContractServiceEntityConfiguration());

            modelBuilder.ApplyConfiguration(new OutContractEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ContractEquipmentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ContractServicePackageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ContractStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ServicePackageSuspensionTimeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OutContractServicePackageClearingEntityConfiguration());

            modelBuilder.ApplyConfiguration(new CardTypeEntityConfiguration());

            modelBuilder.ApplyConfiguration(new SalesmanEntityConfiguration());

            modelBuilder.ApplyConfiguration(new ServicesEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ServicePackagesEntityConfiguration());

            modelBuilder.ApplyConfiguration(new TaxCategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTechnicianEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ServicePackagePriceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UnitOfMeasurementEntityConfiguation());
            modelBuilder.ApplyConfiguration(new EquipmentPictureEntityConfiguation());

            modelBuilder.ApplyConfiguration(new PictureEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentFileEntityConfiguration());

            modelBuilder.ApplyConfiguration(new InContractTaxEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OutContractServicePackageTaxEntityConfiguration());

            modelBuilder.ApplyConfiguration(new PromotionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionDetailEntityConfiguration());

            modelBuilder.ApplyConfiguration(new ServiceLevelAgreementConfiguration());

            modelBuilder.ApplyConfiguration(new ContractSharingRevenueEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionServicePackageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionChannelPointEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionEquipmentEntityConfigurations());
            modelBuilder.ApplyConfiguration(new TransactionChannelTaxEntityConfiguration());

            modelBuilder.ApplyConfiguration(new BrasInformationEntityConfigurations());
            modelBuilder.ApplyConfiguration(new RadiusServerInformationEntityConfigurations());

            modelBuilder.ApplyConfiguration(new CurrencyUnitEntityConfiguration());

            modelBuilder.ApplyConfiguration(new ContractFormEntityConfiguration());

            modelBuilder.ApplyConfiguration(new OutputChannelPointEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PackagePriceTableEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionPriceBusTableEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ContractorPropertiesEntityConfiguation());

            //modelBuilder.Entity<Equipment>()
            //    .HasOne<Picture>()
            //    .WithMany()
            //    .HasForeignKey(e => e.EquipmentPictureId);

            modelBuilder.Entity<TemporaryPayingContract>()
                .HasKey(c => new { c.OutContractId, c.ServicePackageId });

            modelBuilder.SeedData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
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