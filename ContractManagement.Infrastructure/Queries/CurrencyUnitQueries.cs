using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Utility;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface ICurrencyUnitQueries : IQueryRepository
    {
        IEnumerable<CurrencyUnitDTO> Autocomplete(RequestFilterModel requestFilterModel);
        IPagedList<CurrencyUnitDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
        IEnumerable<CurrencyUnitDTO> GetAll(RequestFilterModel filterModel);
        CurrencyUnitDTO Find(int id);
        IEnumerable<CurrencyUnitModel> GetCurrencyList(string currencyUnitCode);
        IEnumerable<PriceByCurrencyUnitDTO> GetAllPriceByCurrencyUnit(RequestFilterModel filterModel);
    }
    public class CurrencyUnitQueries : QueryRepository<CurrencyUnit, int>, ICurrencyUnitQueries
    {
        public CurrencyUnitQueries(ContractDbContext contractDbContext) : base(contractDbContext)
        {
        }
        public IEnumerable<CurrencyUnitDTO> Autocomplete(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<CurrencyUnitDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("  CONCAT(t1.CurrencyUnitName,' (', t1.CurrencyUnitCode,')') AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.CurrencyUnitCode LIKE @name", new { name = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.CurrencyUnitName LIKE @name", new { name = $"%{requestFilterModel.Keywords}%" });
            }
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<CurrencyUnitDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<CurrencyUnitDTO>(filterModel);
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                   .OrWhere("t1.CurrencyUnitCode LIKE @name", new { name = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.CurrencyUnitName LIKE @name", new { name = $"%{filterModel.Keywords}%" });
            }
            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode AS Code");

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<CurrencyUnitDTO> GetAll(RequestFilterModel filterModel)
        {
            filterModel.Paging = false;
            var dapperExecution = BuildByTemplate<CurrencyUnitDTO>(filterModel);
            //dapperExecution.SqlBuilder.Select("CONCAT(t1.Label, '(', t2.Description, ')') as UnitOfMeasurementName");


            if (filterModel.PropertyFilterModels
                .Any(p => p.Field.Equals("Ids", StringComparison.OrdinalIgnoreCase)))
            {
                var ids =
                    filterModel.PropertyFilterModels.First(
                            p => p.Field.Equals("Ids", StringComparison.OrdinalIgnoreCase))
                        .FilterValue.ToString().SplitToInt(',');
                dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
            }

            return dapperExecution.ExecuteQuery();
        }

        public CurrencyUnitDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<CurrencyUnitDTO>();

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<CurrencyUnitModel> GetCurrencyList(string currencyUnitCode)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<CurrencyUnitModel>();
            //dapperExecution.SqlBuilder.Select("t1.Id AS Id");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitName AS Name");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode AS Value");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitName AS Label");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitSymbol AS Symbol");
            if (!string.IsNullOrEmpty(currencyUnitCode))
            {
                dapperExecution.SqlBuilder.Where("t1.CurrencyUnitCode <> @currencyUnitCode", new { currencyUnitCode });
            }

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<PriceByCurrencyUnitDTO> GetAllPriceByCurrencyUnit(RequestFilterModel filterModel)
        {
            filterModel.Paging = false;
            var dapperExecution = BuildByTemplate<PriceByCurrencyUnitDTO>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.Id as CurrencyUnitId");

            return dapperExecution.ExecuteQuery();
        }
    }
}
