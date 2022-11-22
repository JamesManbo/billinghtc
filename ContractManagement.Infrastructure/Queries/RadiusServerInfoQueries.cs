using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using ContractManagement.Domain.Models.RadiusAndBras;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IRadiusServerInfoQueries : IQueryRepository
    {
        RadiusServerInfoDTO Find(int id);
        IEnumerable<RadiusServerInfoDTO> GetAll();
        IEnumerable<SelectionItem> GetSelectionList();
        IPagedList<RadiusServerInfoDTO> GetList(RequestFilterModel filterModel);

    }
    public class RadiusServerInfoQueries : QueryRepository<RadiusServerInformation, int>, IRadiusServerInfoQueries
    {
        public RadiusServerInfoQueries(ContractDbContext context) : base(context)
        {
        }

        public IPagedList<RadiusServerInfoDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<RadiusServerInfoDTO>(filterModel);
            dapperExecution.SqlBuilder.Select("t2.MarketName AS `MarketAreaName`");
            dapperExecution.SqlBuilder.LeftJoin("MarketAreas t2 ON t1.MarketAreaId = t2.Id");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                   .OrWhere("t1.ServerName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.IP LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.Description LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });
            }
            return dapperExecution.ExecutePaginateQuery();
        }
                                
        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("CONCAT_WS('', t1.`ServerName`, ', IP ', t1.`IP`)  AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Select("t1.`IP` AS `Code`");
            return dapperExecution.ExecuteQuery();
        }

        public RadiusServerInfoDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<RadiusServerInfoDTO>();
            dapperExecution.SqlBuilder.Select("t2.MarketName AS `MarketAreaName`");
            dapperExecution.SqlBuilder.LeftJoin("MarketAreas t2 ON t1.MarketAreaId = t2.Id");

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<RadiusServerInfoDTO> GetAll()
        {
            var dapperExecution = BuildByTemplate<RadiusServerInfoDTO>();
            dapperExecution.SqlBuilder.Select("t2.MarketName AS `MarketAreaName`");
            dapperExecution.SqlBuilder.LeftJoin("MarketAreas t2 ON t1.MarketAreaId = t2.Id");
            return dapperExecution.ExecuteQuery();
        }
    }
}
