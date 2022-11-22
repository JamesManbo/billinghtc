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
    public interface IBrasInfoQueries : IQueryRepository
    {
        BrasInfoDTO Find(int id);
        IEnumerable<SelectionItem> GetSelectionList();
        IEnumerable<BrasInfoDTO> GetAll();
        IPagedList<BrasInfoDTO> GetList(RequestFilterModel filterModel);
    }
    public class BrasInfoQueries : QueryRepository<BrasInformation, int>, IBrasInfoQueries
    {
        public BrasInfoQueries(ContractDbContext context) : base(context)
        {
        }

        public IPagedList<BrasInfoDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<BrasInfoDTO>(filterModel);
            dapperExecution.SqlBuilder.Select("t2.ProjectName AS ProjectName");
            dapperExecution.SqlBuilder.LeftJoin("Projects t2 ON t1.ProjectId = t2.Id");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                   .OrWhere("t1.UserName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
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
        public BrasInfoDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<BrasInfoDTO>();

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<BrasInfoDTO> GetAll()
        {
            var dapperExecution = BuildByTemplate<BrasInfoDTO>();
            return dapperExecution.ExecuteQuery();
        }
    }
}
