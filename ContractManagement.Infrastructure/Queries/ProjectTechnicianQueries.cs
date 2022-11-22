using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IProjectTechnicianQueries : IQueryRepository
    {
        IEnumerable<TransactionPendingTaskDTO> GetCurrentPendingTaskBySupporter(int marketId, DateTime startDate, DateTime endDate);
    }
    public class ProjectTechnicianQueries : QueryRepository<ProjectTechnician, int>, IProjectTechnicianQueries
    {
        private readonly IMapper _mapper;

        public ProjectTechnicianQueries(ContractDbContext contractDbContext, IMapper mapper) : base(contractDbContext)
        {
            _mapper = mapper;
        }

        public IEnumerable<TransactionPendingTaskDTO> GetCurrentPendingTaskBySupporter(int marketId, DateTime startDate, DateTime endDate)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<TransactionPendingTaskDTO>();
            dapperExecution.SqlBuilder.Select("COUNT(distinct t4.Id)/t3.NumberOfSupporters AS TaskPerSupporterAVG");

            dapperExecution.SqlBuilder.InnerJoin("Outcontracts as t2 ON t2.ProjectId = t1.ProjectId");
            dapperExecution.SqlBuilder.InnerJoin("Projects as t3 ON t3.Id = t1.ProjectId");
            dapperExecution.SqlBuilder.InnerJoin("Transactions as t4 ON t4.OutContractId = t2.Id");            

            dapperExecution.SqlBuilder.Where("t4.StatusId = 1");
            dapperExecution.SqlBuilder.Where("t2.MarketAreaId = @marketId", new { marketId });
            dapperExecution.SqlBuilder.Where("IFNULL(t3.NumberOfSupporters,0) > 0");
            dapperExecution.SqlBuilder.GroupBy("t1.TechnicianName");
            dapperExecution.SqlBuilder.GroupBy("t2.ProjectName");
            dapperExecution.SqlBuilder.OrderBy("TaskPerSupporterAVG");
            var result = dapperExecution.ExecuteQuery();

            var x = new List<TransactionPendingTaskDTO>();
            foreach (var item in result.ToList())
            {
                if(x.Count() == 0)
                {
                    x.Add(item);
                    continue;
                }
                if(x.Any(y=>y.TechnicianName == item.TechnicianName))
                {
                    x.First(y => y.TechnicianName == item.TechnicianName).TaskPerSupporterAVG += item.TaskPerSupporterAVG;
                    continue;
                }
                else
                {
                    x.Add(item);
                    continue;
                }
            }
            return x;
            
        }
    }
}
