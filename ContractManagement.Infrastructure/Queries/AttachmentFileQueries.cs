using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IAttachmentFileQueries : IQueryRepository
    {
        IPagedList<AttachmentFileDTO> GetList(RequestFilterModel filterModel);
    }
    public class AttachmentFileQueries : QueryRepository<AttachmentFile, int>, IAttachmentFileQueries
    {
        public AttachmentFileQueries(ContractDbContext contractDbContext) : base(contractDbContext)
        {

        }

        public IPagedList<AttachmentFileDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<AttachmentFileDTO>(filterModel);
            //dapperExecution.SqlBuilder.OrderBy("CreatedDate DESC");
            return dapperExecution.ExecutePaginateQuery();
        }
    }
}
