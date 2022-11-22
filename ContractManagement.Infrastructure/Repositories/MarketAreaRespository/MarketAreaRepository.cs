using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.Models.OutContracts;
using GenericRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.MarketAreaRespository
{
    public class MarketAreaRepository : CrudRepository<MarketArea, int>, IMarketAreaRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public MarketAreaRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public bool CheckExistMarketAreaName(string name, int id)
        {
            return DbSet.Any(x => name.Trim().Equals(x.MarketName, StringComparison.OrdinalIgnoreCase) && !x.IsDeleted && x.Id != id);
        }

        public bool CheckExistMarketAreaCode(string code, int id)
        {
            return DbSet.Any(x => code.Trim().Equals(x.MarketCode, StringComparison.OrdinalIgnoreCase)&& !x.IsDeleted && x.Id != id);
        }

        public int GetMarketAreaId(string marketAreaName)
        {
            try
            {
                return _contractDbContext.MarketAreas.Where(x => x.MarketName.Contains(marketAreaName)
                && x.IsDeleted == false && x.IsActive == true).Select(y => y.Id).FirstOrDefault();
            }
            catch(Exception e)
            {
                return 0;
            }
        }
    }
}
