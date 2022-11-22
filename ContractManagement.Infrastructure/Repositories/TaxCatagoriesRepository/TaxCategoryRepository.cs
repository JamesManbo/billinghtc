using System.Linq;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using GenericRepository;
using GenericRepository.Configurations;

namespace ContractManagement.Infrastructure.Repositories.TaxCatagoriesRepository
{
    public class TaxCategoryRepository : CrudRepository<TaxCategory, int>, ITaxCategoryRepositoty
    {
        private readonly ContractDbContext _contractDbContext;
        public TaxCategoryRepository(ContractDbContext contractDbContext, IWrappedConfigAndMapper configAndMapper) : base(contractDbContext, configAndMapper)
        {
            _contractDbContext = contractDbContext;
        }

        public bool CheckExistTaxName(string nameTax, int id)
        {
            return _contractDbContext.TaxCategories.Any(x => x.TaxName == nameTax.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExistTaxCode(string codeTax, int id)
        {
            return _contractDbContext.TaxCategories.Any(x => x.TaxCode == codeTax.Trim() && x.IsDeleted == false && x.Id != id);
        }
    }
}
