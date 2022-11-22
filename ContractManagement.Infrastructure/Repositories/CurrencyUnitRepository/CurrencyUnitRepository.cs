using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.CurrencyUnitRepository
{
    public class CurrencyUnitRepository : CrudRepository<CurrencyUnit, int>, ICurrencyUnitRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public CurrencyUnitRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public bool CheckExistCurrencyUnitName(string name, int id)
        {
            var lstNameCurrencyUnit = _contractDbContext.CurrencyUnits.Where(x => x.CurrencyUnitName == name.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstNameCurrencyUnit == 0) // không tồn tại tên thiết bị
            {
                return true;
            }
            else if (id > 0 && lstNameCurrencyUnit == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CheckExistCurrencyUnitCode(string code, int id)
        {
            var lstCodeCurrencyUnit = _contractDbContext.CurrencyUnits.Where(x => x.CurrencyUnitCode == code.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstCodeCurrencyUnit == 0) // không tồn tại mã thiết bị
            {
                return true;
            }
            else if (id > 0 && lstCodeCurrencyUnit == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
