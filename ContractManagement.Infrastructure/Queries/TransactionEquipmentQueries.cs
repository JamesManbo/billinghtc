using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface ITransactionEquipmentQueries : IQueryRepository
    {
        IPagedList<TransactionEquipmentDTO> GetList(RequestFilterModel requestFilterModel);
        TransactionEquipmentDTO Find(int id);

    }
    public class TransactionEquipmentQueries : QueryRepository<TransactionEquipment, int>, ITransactionEquipmentQueries
    {
        public TransactionEquipmentQueries(ContractDbContext context) : base(context)
        {
        }
        public TransactionEquipmentDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<TransactionEquipmentDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IPagedList<TransactionEquipmentDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<TransactionEquipmentDTO>(requestFilterModel);
            return dapperExecution.ExecutePaginateQuery();
        }
    }
}
