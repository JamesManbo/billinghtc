using AutoMapper.Configuration;
using Dapper;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IReceiptVoucherDetailQueries : IQueryRepository
    {
        IEnumerable<int> GetOCSPIdsExistByStartBillingDate(int[] oCSPIds, DateTime startBillingDate);
    }
    public class ReceiptVoucherDetailQueries : QueryRepository<ReceiptVoucherDetail, int>, IReceiptVoucherDetailQueries
    {
        public ReceiptVoucherDetailQueries(DebtDbContext context) : base(context)
        {
        }

        public IEnumerable<int> GetOCSPIdsExistByStartBillingDate(int[] oCSPIds, DateTime startBillingDate)
        {
            return WithConnection(conn
                => conn.Query(
                    "SELECT TOP 1 OutContractServicePackageId" +
                    " FROM ReceiptVoucherDetails" +
                    " WHERE OutContractServicePackageId IN @oCSPIds AND StartBillingDate = @startBillingDate AND IsDeleted = FALSE",
                    new[]
                    {
                        typeof(int)
                    }, results =>
                {
                    return (int)results[0];
                },
                    new
                    {
                        oCSPIds,
                        startBillingDate
                    })).Distinct();
        }

    }
}
