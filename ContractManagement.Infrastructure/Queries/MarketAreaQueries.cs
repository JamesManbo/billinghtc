
using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System.Collections.Generic;
using GenericRepository.Core;
using Dapper;
using CachingLayer.Interceptor;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IMarketAreaQueries : IQueryRepository
    {
        IPagedList<MarketAreaDTO> GetList(RequestFilterModel requestFilterModel);
        [Cache]
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel);
        [Cache]
        IEnumerable<MarketAreaDTO> GetAll(RequestFilterModel requestFilterModel = null);
        IEnumerable<HierarchicalItem> GetHierarchicalList(RequestFilterModel requestFilterModel);
        MarketAreaDTO Find(int id);
        string GetMarketAreaCode(int marketAreaId);
    }
    public class MarketAreaQueries : QueryRepository<MarketArea, int>, IMarketAreaQueries
    {
        public MarketAreaQueries(ContractDbContext context) : base(context)
        {
        }

        public MarketAreaDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<MarketAreaDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IPagedList<MarketAreaDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<MarketAreaDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.LeftJoin("MarketAreas t2 ON t1.ParentId = t2.Id");
            dapperExecution.SqlBuilder.Select("t2.MarketName as ParentName");
            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t1.MarketName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Select("t1.ParentId");
            dapperExecution.SqlBuilder.Select("t1.DisplayOrder");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<MarketAreaDTO> GetAll(RequestFilterModel requestFilterModel = null)
        {
            var dapperExecution = requestFilterModel != null
                ? BuildByTemplate<MarketAreaDTO>(requestFilterModel)
                : BuildByTemplate<MarketAreaDTO>();

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<HierarchicalItem> GetHierarchicalList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<HierarchicalItem>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t1.MarketName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.TreeLevel AS `TreeLevel`");
            dapperExecution.SqlBuilder.Select("t1.MarketCode AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.TreePath AS `TreePath`");
            dapperExecution.SqlBuilder.Select("(CASE " +
                              "         WHEN EXISTS(" +
                              "             SELECT NULL AS `EMPTY`" +
                              "             FROM MarketAreas AS t2" +
                              "             WHERE t2.ParentId = t1.Id AND t2.IsDeleted = 0" +
                              "             ) THEN 1" +
                              "         ELSE 0" +
                              "     END) AS HasChildren");
            return dapperExecution.ExecuteQuery();
        }

        public string GetMarketAreaCode(int marketAreaId)
        {
            return WithConnection((conn) =>
                conn.QueryFirstOrDefault<string>(
                    "SELECT MarketCode FROM MarketAreas WHERE Id = @marketAreaId AND IsDeleted = FALSE", new { marketAreaId = marketAreaId }));
        }
    }
}
