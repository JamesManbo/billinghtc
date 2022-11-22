using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;

namespace DebtManagement.Infrastructure.Seed
{
    public static class ContractContextRuntimeSeed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentMethodType>().HasData(PaymentMethodType.List());
            modelBuilder.Entity<PaymentVoucherStatus>().HasData(PaymentVoucherStatus.List());
            modelBuilder.Entity<ReceiptVoucherStatus>().HasData(ReceiptVoucherStatus.List());
            modelBuilder.Entity<ClearingStatus>().HasData(ClearingStatus.List());
            modelBuilder.Entity<ReceiptVoucherType>().HasData(ReceiptVoucherType.List());
            modelBuilder.Entity<CurrencyUnit>().HasData(CurrencyUnit.List());
        }

        //private static IEnumerable<Contractor> GetPredefinedTestBuyer()
        //{
        //    return new List<Contractor>()
        //    {
        //        new Contractor("9C34C4DC-0592-4723-8F5F-A3088A520C66", "Nguyễn Đăng Sơn") { Id = 1 },
        //        new Contractor("cd576124-5c88-4c58-a348-3c1bea5b2fdb", "Nguyễn Văn A") { Id = 2 }
        //    };
        //}
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
