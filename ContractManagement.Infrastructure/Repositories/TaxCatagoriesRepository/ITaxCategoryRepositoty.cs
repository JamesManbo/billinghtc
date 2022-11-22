using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using GenericRepository;

namespace ContractManagement.Infrastructure.Repositories.TaxCatagoriesRepository
{
    public interface ITaxCategoryRepositoty : ICrudRepository<TaxCategory, int>
    {
        bool CheckExistTaxName(string nameTax, int id);
        bool CheckExistTaxCode(string codeTax, int id);
    }
}
