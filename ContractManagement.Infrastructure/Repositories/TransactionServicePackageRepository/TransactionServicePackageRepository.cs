using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories
{
    public interface ITransactionServicePackageRepository : ICrudRepository<TransactionServicePackage, int>
    {
    }
    public class TransactionServicePackageRepository : CrudRepository<TransactionServicePackage, int>, ITransactionServicePackageRepository
    {
        public TransactionServicePackageRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }
    }
}
