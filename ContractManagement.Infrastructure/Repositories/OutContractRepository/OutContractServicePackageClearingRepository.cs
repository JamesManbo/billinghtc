using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.OutContractRepository
{
    public interface IOutContractServicePackageClearingRepository : ICrudRepository<OutContractServicePackageClearing, int>
    {
    }

    public class OutContractServicePackageClearingRepository : CrudRepository<OutContractServicePackageClearing, int>, IOutContractServicePackageClearingRepository
    {
        public OutContractServicePackageClearingRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }
    }
}
