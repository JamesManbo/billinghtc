using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Dashboard.DashboardSupporter;
using Dapper;
using GenericRepository;
using Global.Models.PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IDashboardQueries : IQueryRepository
    {
        DashboardDto GetOutContractTotalQuantityEveryMonth();
        DashboardDto GetInContractTotalQuantityEveryMonth();
        DashboardDto GetOutContractSignedQuantityEveryMonth();
        IEnumerable<EffectedContract> GetOutContractEffectedQuantityEveryMonth();

        DashboardSupporterDTO GetTotalNationalProject(DateTime startDate, DateTime endDate);
        DashboardSupporterDTO GetTotalCustomerDashboardSupporter(DateTime startDate, DateTime endDate);
        DashboardSupporterDTO GetTotalEquipmentDashboardSupporter(DateTime startDate, DateTime endDate);
        DashboardSupporterDTO GetTotalCustomerIncrementDashboardSupporter(DateTime startDate, DateTime endDate);
        DashboardSupporterDTO GetTotalCustomerDecrementDashboardSupporter(DateTime startDate, DateTime endDate);
        CurrentWorkStatusDTO GetCurrentWorkStatus(int maketId);
    }
    public class DashboardQueries : QueryRepository<OutContract, int>, IDashboardQueries
    {
        public DashboardQueries(ContractDbContext context) : base(context)
        {
        }

        #region Dashboard Admin

        public DashboardDto GetOutContractTotalQuantityEveryMonth()
        {
            var value = WithConnection((conn) =>
               conn.Query<DashboardDto>("GetOutContractTotalQuantityEveryMonth", commandType: CommandType.StoredProcedure));

            return value.FirstOrDefault();
        }
        public DashboardDto GetInContractTotalQuantityEveryMonth()
        {
            var value = WithConnection((conn) =>
               conn.Query<DashboardDto>("GetInContractTotalQuantityEveryMonth", commandType: CommandType.StoredProcedure));

            return value.FirstOrDefault();
        }
        public DashboardDto GetOutContractSignedQuantityEveryMonth()
        {
            var value = WithConnection((conn) =>
               conn.Query<DashboardDto>("GetOutContractSignedQuantityEveryMonth", commandType: CommandType.StoredProcedure));

            return value.FirstOrDefault();
        }
        public IEnumerable<EffectedContract> GetOutContractEffectedQuantityEveryMonth()
        {
            return WithConnection((conn) =>
               conn.Query<EffectedContract>("GetOutContractEffectedQuantityEveryMonth", commandType: CommandType.StoredProcedure));
            
        }

        #endregion

        #region Dashboard supporter

        public DashboardSupporterDTO GetTotalNationalProject(DateTime startDate, DateTime endDate)
        {
            if (endDate == DateTime.MinValue)
            {
                endDate = DateTime.Now;
            }
            var dapperExe = BuildByTemplateWithoutSelect<DashboardSupporterDTO>();
            dapperExe.SqlBuilder.Select("SUM(IF (t1.MarketAreaId = 1,1,0) )AS TotalNorth");
            dapperExe.SqlBuilder.Select("SUM(IF (t1.MarketAreaId = 2,1,0) )AS TotalCenter");
            dapperExe.SqlBuilder.Select("SUM(IF (t1.MarketAreaId = 3,1,0) )AS TotalSouth");
            dapperExe.SqlBuilder.Select("COUNT( t1.ProjectId )AS Total");
            dapperExe.SqlBuilder.RightJoin(@"
                (   SELECT
                    DISTINCT
                    t1.Id,
                    t1.MarketAreaId,
                    t1.ContractorId
                    FROM
	                    `OutContracts` AS t1
	                    INNER JOIN OutContractServicePackages t2 ON t1.Id = t2.OutContractId 
                    WHERE
	                    t1.`IsDeleted` = FALSE 
	                    AND t2.IsDeleted = FALSE 
	                    AND IFNULL(t1.ProjectId,0) > 0 
                        AND t2.ServiceId = 1
	                    AND t1.IsDeleted = FALSE 
	                    AND t1.TimeLine_Signed >= @startDate
	                    AND t1.TimeLine_Signed <  @endDate
                    ) AS t2 ON t1.Id=t2.Id", new { startDate, endDate });
   
            dapperExe.SqlBuilder.Where("t1.ContractStatusId IN (2,3,4)"); // mới
            
            return dapperExe.ExecuteScalarQuery();
        }

        public DashboardSupporterDTO GetTotalCustomerDashboardSupporter(DateTime startDate, DateTime endDate)
        {
            //var dapperExe = BuildByTemplateWithoutSelect<DashboardSupporterDTO>();
            //dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 1,1,0) )AS TotalNorth");
            //dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 2,1,0) )AS TotalCenter");
            //dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 3,1,0) )AS TotalSouth");
            //dapperExe.SqlBuilder.Select("COUNT(DISTINCT t1.Id )AS Total");

            //dapperExe.SqlBuilder.RightJoin(@"
            //    (   SELECT
            //        DISTINCT
            //        t1.Id,
            //        t1.MarketAreaId,
            //        t1.ContractorId

            //        FROM
            //         `OutContracts` AS t1
            //         INNER JOIN OutContractServicePackages t2 ON t1.Id = t2.OutContractId 
            //        WHERE
            //         t1.`IsDeleted` = FALSE 
            //         AND t2.IsDeleted = FALSE 
            //         AND IFNULL(t1.ProjectId,0) > 0 
            //            AND t2.ServiceId = 1
            //         AND t1.IsDeleted = FALSE 
            //         AND t1.TimeLine_Signed >= @startDate
            //         AND t1.TimeLine_Signed <  @endDate
            //        ) AS t2 ON t1.Id=t2.Id", new { startDate, endDate });
            //dapperExe.SqlBuilder.Where("t1.ContractStatusId IN (2,3,4)"); // mới
            //return dapperExe.ExecuteScalarQuery();
            var p = new DynamicParameters();
            p.Add("startDate", startDate);
            p.Add("endDate", endDate);
            var x = WithConnection(conn =>
               conn.QueryFirstOrDefault<DashboardSupporterDTO>(
                   "GetCustomerInMarketArea",
                   p,
                   commandType: CommandType.StoredProcedure
               )
            );
            return x;
        }

        public DashboardSupporterDTO GetTotalEquipmentDashboardSupporter(DateTime startDate, DateTime endDate)
        {          
            var dapperExe = BuildByTemplateWithoutSelect<DashboardSupporterDTO>();
            dapperExe.SqlBuilder.Select("SUM(IF(t1.MarketAreaId = 1,ce.RealUnit,0)) AS TotalNorth");
            dapperExe.SqlBuilder.Select("SUM(IF(t1.MarketAreaId = 2,ce.RealUnit,0)) AS TotalCenter");
            dapperExe.SqlBuilder.Select("SUM(IF(t1.MarketAreaId = 3,ce.RealUnit,0)) AS TotalSouth");
            dapperExe.SqlBuilder.Select("SUM(ce.RealUnit) AS Total");

            dapperExe.SqlBuilder.InnerJoin("OutContractServicePackages AS t2 ON t2.OutContractId = t1.Id");
            dapperExe.SqlBuilder.InnerJoin("ContractEquipments AS ce ON ce.OutContractPackageId = t2.Id");
            dapperExe.SqlBuilder.Where("t2.ServiceId = 1");
            dapperExe.SqlBuilder.Where("IFNULL(t1.ProjectId,0) > 0 ");
            dapperExe.SqlBuilder.Where("t1.IsDeleted = FALSE ");
            dapperExe.SqlBuilder.Where("t2.IsDeleted = FALSE ");
            dapperExe.SqlBuilder.Where("t1.TimeLine_Signed > @startDate", new { startDate });
            dapperExe.SqlBuilder.Where("t1.TimeLine_Signed < @endDate", new { endDate });
           
            return dapperExe.ExecuteScalarQuery();
        }

        public DashboardSupporterDTO GetTotalCustomerIncrementDashboardSupporter(DateTime startDate, DateTime endDate)
        {
           
            var dapperExe = BuildByTemplateWithoutSelect<DashboardSupporterDTO>();
            dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 1,1,0) )AS TotalNorth");
            dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 2,1,0) )AS TotalCenter");
            dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 3,1,0) )AS TotalSouth");
            dapperExe.SqlBuilder.Select("COUNT(DISTINCT t2.ContractorId )AS Total");

            dapperExe.SqlBuilder.RightJoin(@"
                (   SELECT
                    DISTINCT
                    t1.Id,
                    t1.MarketAreaId,
                    t1.ContractorId

                    FROM
	                    `OutContracts` AS t1
	                    INNER JOIN OutContractServicePackages t2 ON t1.Id = t2.OutContractId 
                    WHERE
	                    t1.`IsDeleted` = FALSE 
	                    AND t2.IsDeleted = FALSE 
	                    AND IFNULL(t1.ProjectId,0) > 0 
                        AND t2.ServiceId = 1
	                    AND t1.IsDeleted = FALSE 
	                    AND t1.TimeLine_Signed >= @startDate
	                    AND t1.TimeLine_Signed <  @endDate
                    ) AS t2 ON t1.Id=t2.Id", new { startDate, endDate });
            dapperExe.SqlBuilder.Where("t1.ContractStatusId IN (2,3,4)"); // mới

                    
            return dapperExe.ExecuteScalarQuery();

        }

        public DashboardSupporterDTO GetTotalCustomerDecrementDashboardSupporter(DateTime startDate, DateTime endDate)
        {
           
            var dapperExe = BuildByTemplateWithoutSelect<DashboardSupporterDTO>();
            dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 1,1,0) )AS TotalNorth");
            dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 2,1,0) )AS TotalCenter");
            dapperExe.SqlBuilder.Select("SUM(IF (t2.MarketAreaId = 3,1,0) )AS TotalSouth");
            dapperExe.SqlBuilder.Select("COUNT(DISTINCT t2.ContractorId )AS Total");

            dapperExe.SqlBuilder.RightJoin(@"
                (   SELECT
                    DISTINCT
                    t1.Id,
                    t1.MarketAreaId,
                    t1.ContractorId

                    FROM
	                    `OutContracts` AS t1
	                    INNER JOIN OutContractServicePackages t2 ON t1.Id = t2.OutContractId 
                    WHERE
	                    t1.`IsDeleted` = FALSE 
	                    AND t2.IsDeleted = FALSE 
	                    AND IFNULL(t1.ProjectId,0) > 0 
                        AND t2.ServiceId = 1
	                    AND t1.IsDeleted = FALSE 
	                    AND t1.TimeLine_Signed >= @startDate
	                    AND t1.TimeLine_Signed <  @endDate
                    ) AS t2 ON t1.Id=t2.Id", new { startDate, endDate });

            dapperExe.SqlBuilder.Where("t1.ContractStatusId = 5"); // đã thanh lý    
            return dapperExe.ExecuteScalarQuery();

        }

        public CurrentWorkStatusDTO GetCurrentWorkStatus(int maketId)
        {
            var dapperExe = BuildByTemplateWithoutSelect<CurrentWorkStatusDTO>();
            dapperExe.SqlBuilder.Select("SUM(IF (t2.StatusId = 1,1,0) )AS PendingWorkQuantity");
            dapperExe.SqlBuilder.Select("SUM(IF (t2.StatusId = 2,1,0) )AS DoneWorkQuantity");
            dapperExe.SqlBuilder.Select("SUM(IF (t2.StatusId = 3,1,0) )AS CancelWorkQuantity");

            dapperExe.SqlBuilder.RightJoin("Transactions t2 ON t2.OutContractId = t1.Id AND t2.IsDeleted = FALSE");           

            dapperExe.SqlBuilder.Where("t1.MarketAreaId =  @maketId", new { maketId });
            dapperExe.SqlBuilder.Where("t1.IsDeleted = FALSE");


            return dapperExe.ExecuteScalarQuery();          
        }

        #endregion

    }
}
