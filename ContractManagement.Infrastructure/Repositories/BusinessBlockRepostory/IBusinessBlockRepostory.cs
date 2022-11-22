using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.BusinessBlockRepostory
{
    public interface IBusinessBlockRepostory : ICrudRepository<ManagementBusinessBlock, int>
    {
    }
}
