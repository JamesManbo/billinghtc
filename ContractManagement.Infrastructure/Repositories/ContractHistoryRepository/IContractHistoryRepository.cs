using ContractManagement.Domain.Models.ChangeHistories;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository
{
    public interface IContractHistoryRepository
    {
        Task<ContractHistory> Get(string id);
        Task Create(ContractHistory changeHistory);
        //void Update(string id, ContractHistory changeHistory);
        Task<IPagedList<ContractHistory>> GetList(RequestFilterModel filterModel);
    }
}
