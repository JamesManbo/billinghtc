
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IPromotionTypeQueries : IQueryRepository
    {        
        IPagedList<PromotionType> GetList(RequestFilterModel filterModel);
        PromotionType GetPromotionById(int id);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
       
    }

    public class PromotionTypeQueries : QueryRepository<PromotionType, int>, IPromotionTypeQueries
    {
        public PromotionTypeQueries(ContractDbContext context) : base(context)
        {
            
        }

        public IPagedList<PromotionType> GetList(RequestFilterModel requestFilterModel)
        {

            var dapperExecution = BuildByTemplate<PromotionType>(requestFilterModel);            
            return dapperExecution.ExecutePaginateQuery();
        }
      
        public PromotionType GetPromotionById(int id)
        {
            var dapperExecution = BuildByTemplate<PromotionType>();            
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        /// <summary>
        /// for combobox
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.PromotionName AS Text");            
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            return dapperExecution.ExecuteQuery();
        }
      
    }
}
