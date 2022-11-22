using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.InOutContractTaxRepository
{
    public interface IInContractTaxRepository : ICrudRepository<InContractTax, int>
    {
        bool DeleteAllByInContractId(int inContractId);
    }
}
