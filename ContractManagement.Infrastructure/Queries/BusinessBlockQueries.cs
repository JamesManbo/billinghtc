using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IBusinessBlockQueries : IQueryRepository
    {
        IEnumerable<SelectionItem> GetSelectionList();
        BusinessBlockDTO Find(int id);
        bool CheckExistName(int id, string name);
    }

    public class BusinessBlockQueries : QueryRepository<ManagementBusinessBlock, int>, IBusinessBlockQueries
    {
        private readonly IMapper _mapper;

        public BusinessBlockQueries(ContractDbContext contractDbContext, IMapper mapper) : base(contractDbContext)
        {
            _mapper = mapper;
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.BusinessBlockName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");
            return dapperExecution.ExecuteQuery();
        }

        public BusinessBlockDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<BusinessBlockDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<BusinessBlockDTO>();
            dapperExecution.SqlBuilder.Where("t1.BusinessBlockName = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

    }
}
