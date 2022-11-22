using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.ServicePackages;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IServiceGroupsQueries : IQueryRepository
    {
        IEnumerable<ServiceGroupDTO> GetAll(RequestFilterModel requestFilterModel = null);
        IPagedList<ServiceGroupDTO> GetList(RequestFilterModel requestFilterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel);
        ServiceGroupDTO Find(int id);
        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);
    }

    public class ServiceGroupsQueries : QueryRepository<ServiceGroup, int>, IServiceGroupsQueries
    {
        public ServiceGroupsQueries(ContractDbContext context) : base(context)
        {
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<ServiceGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.GroupCode = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<ServiceGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.GroupName = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public ServiceGroupDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<ServiceGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<ServiceGroupDTO> GetAll(RequestFilterModel requestFilterModel = null)
        {
            var dapperExecution = BuildByTemplate<ServiceGroupDTO>(requestFilterModel);
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<ServiceGroupDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<ServiceGroupDTO>(requestFilterModel);
            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.GroupName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            return dapperExecution.ExecuteQuery();
        }

    }
}
