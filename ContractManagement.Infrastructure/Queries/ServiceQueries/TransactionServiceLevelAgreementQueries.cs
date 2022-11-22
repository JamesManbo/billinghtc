using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries.ServiceQueries
{
    public interface ITransactionServiceLevelAgreementQueries : IQueryRepository
    {
        public IEnumerable<TransactionSlaDTO> GetSLAByTransactionServicePackageId(int id);
    }

    public class TransactionServiceLevelAgreementQueries : QueryRepository<TransactionServiceLevelAgreement, int>,
        ITransactionServiceLevelAgreementQueries
    {
        public TransactionServiceLevelAgreementQueries(ContractDbContext context) : base(context)
        {
        }

        public IEnumerable<TransactionSlaDTO> GetSLAByTransactionServicePackageId(int id)
        {
            var dapperExecution = BuildByTemplate<TransactionSlaDTO>();
            dapperExecution.SqlBuilder.Where("t1.TransactionServicePackageId = @id", new { id });
            return dapperExecution.ExecuteQuery();
        }
    }
}
