using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface ITransactionServicePackageQueries : IQueryRepository
    {
        IPagedList<TransactionServicePackageDTO> GetList(RequestFilterModel requestFilterModel);
        TransactionServicePackageDTO Find(int id);
        int GetChannelIndexOfCustomer(string customerShortName);

    }
    public class TransactionServicePackageQueries : QueryRepository<TransactionServicePackage, int>, ITransactionServicePackageQueries
    {
        public TransactionServicePackageQueries(ContractDbContext context) : base(context)
        {
        }
        public TransactionServicePackageDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<TransactionServicePackageDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IPagedList<TransactionServicePackageDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<TransactionServicePackageDTO>(requestFilterModel);
            return dapperExecution.ExecutePaginateQuery();
        }

        public int GetChannelIndexOfCustomer(string customerShortName)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<int>();
            dapperExecution.SqlBuilder.Select("COUNT(1)");
            dapperExecution.SqlBuilder.InnerJoin("Transactions t ON t.Id = t1.TransactionId");
            dapperExecution.SqlBuilder.InnerJoin("Contractors ct ON t.ContractorId = ct.Id");
            dapperExecution.SqlBuilder.Where("t.Type = @addNewChannelType AND t.StatusId IN @unapprovedStatuses",
                new
                {
                    addNewChannelType = TransactionType.AddNewServicePackage.Id,
                    unapprovedStatuses = new int[] { TransactionStatus.WaitAcceptanced.Id, TransactionStatus.Acceptanced.Id }
                });
            dapperExecution.SqlBuilder.Where("TRIM(ct.ContractorShortName) = @customerShortName", new { customerShortName = customerShortName?.Trim() });
            dapperExecution.SqlBuilder.Where("MONTH(t.CreatedDate) = MONTH(CURRENT_DATE())");
            return dapperExecution.ExecuteQuery().FirstOrDefault();
        }
    }
}
