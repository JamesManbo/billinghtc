using ContractManagement.Domain.AggregatesModel.BaseContract;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractFormRepository
{
    public interface IContractFormRepository : ICrudRepository<ContractForm, int>
    {
        bool CheckExits(int id);
        bool CheckExitsName(string name, int id = 0);
    }
}
