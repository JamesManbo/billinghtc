using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContractManagement.Infrastructure.Repositories.OutContractRepository
{
    public interface IContractTotalByCurrencyRepository: ICrudRepository<ContractTotalByCurrency, int>
    {

    }
    public class ContractTotalByCurrencyRepository : CrudRepository<ContractTotalByCurrency, int>, IContractTotalByCurrencyRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public ContractTotalByCurrencyRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }
    }
}