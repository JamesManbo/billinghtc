using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using CsvHelper;
using Microsoft.EntityFrameworkCore;

namespace ContractManagement.Infrastructure.Seed
{
    public static class ContractContextRuntimeSeed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contractor>().HasData(Contractor.HTCHeadQuarter);

            modelBuilder.Entity<ContractStatus>().HasData(ContractStatus.List());
            modelBuilder.Entity<EquipmentStatus>().HasData(EquipmentStatus.Seeds());
            modelBuilder.Entity<TaxCategory>().HasData(TaxCategory.Seeds());
            modelBuilder.Entity<UnitOfMeasurement>().HasData(UnitOfMeasurement.Seeds());
            modelBuilder.Entity<MarketArea>().HasData(MarketArea.Seeds());
            modelBuilder.Entity<TransactionType>().HasData(TransactionType.Seeds());
            modelBuilder.Entity<TransactionStatus>().HasData(TransactionStatus.List());
            modelBuilder.Entity<TransactionReason>().HasData(TransactionReason.Seeds());
            modelBuilder.Entity<ReasonTypeContractTermination>().HasData(ReasonTypeContractTermination.Seeds());
            modelBuilder.Entity<InContractType>().HasData(InContractType.List());
            modelBuilder.Entity<OutContractType>().HasData(OutContractType.List());
            modelBuilder.Entity<PointType>().HasData(PointType.List());
            modelBuilder.Entity<PromotionType>().HasData(PromotionType.Seeds());
            modelBuilder.Entity<PromotionValueType>().HasData(PromotionValueType.Seeds());

            //var mockServiceGroups = ReadDataFromCSV<ServiceGroup>("service_groups");
            //modelBuilder.Entity<ServiceGroup>().HasData(mockServiceGroups);

            //var mockServices = ReadDataFromCSV<Service>("services");
            //modelBuilder.Entity<Service>().HasData(mockServices);

            modelBuilder.Entity<CurrencyUnit>().HasData(CurrencyUnit.VND);
            modelBuilder.Entity<CurrencyUnit>().HasData(CurrencyUnit.USD);

            modelBuilder.Entity<FlexiblePricingType>().HasData(FlexiblePricingType.List());

            // Mock data
            //var mockEquipments = ReadDataFromCSV<EquipmentType>("equipments");
            //modelBuilder.Entity<EquipmentType>().HasData(mockEquipments);

            //var mockProjects = ReadDataFromCSV<Project>("projects");
            //modelBuilder.Entity<Project>().HasData(mockProjects);

            //var mockPackagePrices = ReadDataFromCSV<ServicePackagePrice>("service_package_prices");
            //modelBuilder.Entity<ServicePackagePrice>().HasData(mockPackagePrices);
        }

        private static IEnumerable<Contractor> GetPredefinedTestBuyer()
        {
            return new List<Contractor>()
            {
                new Contractor("9C34C4DC-0592-4723-8F5F-A3088A520C66", "Nguyễn Đăng Sơn") { Id = 1 },
                new Contractor("cd576124-5c88-4c58-a348-3c1bea5b2fdb", "Nguyễn Văn A") { Id = 2 }
            };
        }

        private static IEnumerable<TEntity> ReadDataFromCSV<TEntity>(string fileName) where TEntity : new()
        {
            var result = new List<TEntity>();
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            var pathOfFile = Path.Combine(dirPath, "Seed", "Data", fileName + ".csv");

            using (var reader = new StreamReader(pathOfFile))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    csv.ValidateHeader(typeof(TEntity));
                    var records = csv.GetRecords<TEntity>();
                    foreach (var record in records)
                    {
                        result.Add(record);
                    }
                }
            }

            return result;
        }
    }
}
