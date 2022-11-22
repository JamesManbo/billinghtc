using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using GenericRepository;
using GenericRepository.BulkOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractPckTaxRepository
{
    public interface IContractPckTaxRepository : IBulkOperation<OutContractServicePackageTax>
    {
    }
}
