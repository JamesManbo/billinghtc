using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using GenericRepository;
using Global.Models.StateChangedResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.MarketAreaRespository
{
    public interface IMarketAreaRepository : ICrudRepository<MarketArea, int>
    {
        bool CheckExistMarketAreaName(string name, int id);
        bool CheckExistMarketAreaCode(string code, int id);
        int GetMarketAreaId(string marketAreaName);
    }
}
