using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Domain.Models.Dashboard.DashboardSupporter;
using ContractManagement.Domain.Models.Dashboard.SupporterProjectFtth;
using Dapper;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface ISuppporterProjectFtthQueries : IQueryRepository
    {        
        DashboardSupporterDTO GetTotalRetailAndBussinessCustomers(FromAndToYearMonthFilter filter);
        DashboardSupporterDTO GetTotalRetailAndBussinessEquipments(DateTime startDate, DateTime endDate);
    }
    public class SuppporterProjectFtthQueries : QueryRepository<OutContract, int>, ISuppporterProjectFtthQueries
    {
        public SuppporterProjectFtthQueries(ContractDbContext context) : base(context)
        {
        }
      

        public DashboardSupporterDTO GetTotalRetailAndBussinessCustomers(FromAndToYearMonthFilter filter)
        {
            
            var p = new DynamicParameters();
            p.Add("startDate", string.IsNullOrEmpty(filter.FromMonth) ? filter.FromMonth : DateTime.MinValue.ToString("yyyy-MM-dd"));
            p.Add("endDate", string.IsNullOrEmpty(filter.ToMonth) ? filter.ToMonth : DateTime.MaxValue.ToString("yyyy-MM-dd"));
            var x =  WithConnection(conn =>
                conn.QueryFirstOrDefault<DashboardSupporterDTO>(
                    "GetCustomerInMarketArea",
                    p,
                    commandType: CommandType.StoredProcedure
                )
            );
            return x;
        }

        public DashboardSupporterDTO GetTotalRetailAndBussinessEquipments(DateTime startDate, DateTime endDate)
        {
            if (endDate == DateTime.MinValue)
            {
                endDate = DateTime.Now;
            }
            var dapperExecution = BuildByTemplateWithoutSelect<DashboardSupporterDTO>();
            dapperExecution.SqlBuilder.Select("SUM(IF (t1.MarketAreaId = 1,1,0) )AS TotalNorth");
            dapperExecution.SqlBuilder.Select("SUM(IF (t1.MarketAreaId = 2,1,0) )AS TotalCenter");
            dapperExecution.SqlBuilder.Select("SUM(IF (t1.MarketAreaId = 3,1,0) )AS TotalSouth");
            dapperExecution.SqlBuilder.Select("COUNT( t3.Id )AS Total");
            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages t2 ON t1.Id = t2.OutContractId");
            dapperExecution.SqlBuilder.InnerJoin("ContractEquipments t3 ON t3.OutContractPackageId = t2.Id");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE AND t2.IsDeleted = FALSE AND t3.IsDeleted = FALSE AND t1.ProjectId IS NULL OR t2.ServiceId <> 1");
            dapperExecution.SqlBuilder.Where("t1.TimeLine_Signed > @startDate", new { startDate });
            dapperExecution.SqlBuilder.Where("t1.TimeLine_Signed < @endDate", new { endDate });
            return dapperExecution.ExecuteScalarQuery();
        }
    }
}
