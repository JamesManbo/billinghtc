using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using GenericRepository;
using GenericRepository.BulkOperation;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractPckTaxRepository
{
    public class ContractPckTaxRepository : BaseBulkOperation<OutContractServicePackageTax>, IContractPckTaxRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public ContractPckTaxRepository(ContractDbContext contractDbContext) : base(contractDbContext)
        {
            _contractDbContext = contractDbContext;
        }
    }
}
