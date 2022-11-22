using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ReportsRepository
{
    public interface IReportsRepository : ICrudRepository<ContractEquipment, int>
    {

    }
}
