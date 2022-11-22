using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IReasonTypeContractTerminationQueries : IQueryRepository
    {
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);

    }

    public class ReasonTypeContractTerminationQueries : QueryRepository<ReasonTypeContractTermination, int>, IReasonTypeContractTerminationQueries
    {
        public ReasonTypeContractTerminationQueries(ContractDbContext context) : base(context)
        {

        }
        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.Name AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            return dapperExecution.ExecuteQuery();
        }
    }
}
