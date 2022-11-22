using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.CurrencyUnitRepository
{
    public interface ICurrencyUnitRepository : ICrudRepository<CurrencyUnit, int>
    {
        bool CheckExistCurrencyUnitName(string name, int id);
        bool CheckExistCurrencyUnitCode(string code, int id);
    }
}
