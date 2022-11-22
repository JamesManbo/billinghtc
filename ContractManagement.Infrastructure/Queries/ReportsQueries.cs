using AutoMapper;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Reports;
using Dapper;
using EventBus.Extensions;
using GenericRepository;
using GenericRepository.Extensions;
using Global.Configs.SystemArgument;
using Global.Models.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IReportsQueries : IQueryRepository
    {
        PagedList<ReportEquipmentInProjectRaw> GetEquipmentInProjectOfOutContract(EquipmentInProjectFilterModel filter);
        PagedList<ReportContractorInProjectDTO> GetContractorInProjectOfOutContract(ContractorInProjectFilterModel contractorInProjectFilterModel);
        IEnumerable<OutContractSimpleDTO> GetOutContractInfos(Global.Models.Filter.ReportFilterBase reportFilter);
        PagedList<ReportMasterCustomerNationwideBusinessDTO> GetListMasterCustomerNationwideBusiness(MasterCustomerNationwideBusinessFilterModel masterCustomerNationwideBusinessFilterModel);
        IPagedList<ReportEquipmentInProjectRaw> GetEquipmentInProjectOfOutContractDetail(EquipmentInProjectFilterModel filter);
        PagedList<ReportTotalRevenueDTO> GetDataTotalRevenue(ReportTotalRevenueFillter reportFilter);
        PagedList<CommissionAndSharingReportDTO> GetCommissionAndSharingReportData(CommissionAndSharingReportFilter reportFilter);
        PagedList<ReportTotalRevenueDTO> GetReportIncreasementOutContract(ReportTotalRevenueFillter reportFilter);
        PagedList<ReportFeeDTO> GetFeeReportData(FeeReportFilter reportFilter);
        PagedList<ReportTotalFeeByCurrency> GetFeeTotalReportData(FeeReportFilter reportFilter);
        TotalDataIncontractFeeSharingReportDto GetTotalDataIncontractFeeSharingReport(FeeReportFilter reportFilter);
        Task<PagedList<TotalRevenuePersonalDTO>> GetTotalRevenueEnterpiseReport(MasterCustomerNationwideBusinessFilterModel filterModel);
        Task<PagedList<FTTHProjectRevenueDTO>> GetFTTHProjectRevenue(MasterCustomerNationwideBusinessFilterModel filterModel);
    }

    public class ReportsQueries : QueryRepository<ContractEquipment, int>, IReportsQueries
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public ReportsQueries(ContractDbContext context, IMapper mapper, IConfiguration config) : base(context)
        {
            _mapper = mapper;
            _config = config;
        }

        public PagedList<ReportEquipmentInProjectRaw> GetEquipmentInProjectOfOutContract(EquipmentInProjectFilterModel filter)
        {
            var p = new DynamicParameters();
            var projects = filter.ProjectIds != null ? String.Join(",", filter.ProjectIds) : "";
            var equipmentIds = filter.EquipmentIds != null ? String.Join(",", filter.EquipmentIds) : "";
            p.Add("skips", filter.Paging == true ? filter.Skip : 0);
            p.Add("take", filter.Paging == true ? filter.Take : int.MaxValue);
            p.Add("startDate", filter.StartDate.ToString("yyyy-MM-dd") != null ? filter.StartDate.ToString("yyyy-MM-dd") : "");
            p.Add("endDate", filter.EndDate.ToString("yyyy-MM-dd") != null ? filter.EndDate.AddDays(1).ToString("yyyy-MM-dd") : "");
            p.Add("projectIds", projects);
            p.Add("equipmentIds", equipmentIds);
            p.Add("columnOderBy", filter.OrderBy);
            p.Add("dir", filter.Dir);
            var orderBy = filter.OrderBy;
            var equipments = WithConnection((conn) =>
                conn.Query<ReportEquipmentInProjectRaw>("GetEquipmentInProjectOfOutContract", p, commandType: CommandType.StoredProcedure));
            var page = new PagedList<ReportEquipmentInProjectRaw>(filter.Skip, filter.Take, equipments.Count());
            var total = equipments.Count() > 0 ? equipments.First().Total : 0;
            page.Subset = equipments.ToList();
            page.TotalItemCount = total;
            return page;
        }

        public IPagedList<ReportEquipmentInProjectRaw> GetEquipmentInProjectOfOutContractDetail(EquipmentInProjectFilterModel filter)
        {
            filter.RestrictOrderBy = true;
            var dapperExecution = BuildByTemplate<ReportEquipmentInProjectRaw>(filter);
            dapperExecution.SqlBuilder.Select("t3.ContractCode");
            dapperExecution.SqlBuilder.Select("t2.CId AS ChannelCid");
            dapperExecution.SqlBuilder.Select("t4.ContractorFullName as CustomerName");
            dapperExecution.SqlBuilder.Select("t5.ContractorCategoryName AS CustomerCategory");
            dapperExecution.SqlBuilder.Select("t3.ProjectId");
            dapperExecution.SqlBuilder.Select("t6.ProjectName");
            dapperExecution.SqlBuilder.Select("t2.TimeLine_Effective AS DeployedDateEquipment");
            dapperExecution.SqlBuilder.Select("CONCAT_WS (' ', SUM(t1.ExaminedUnit), t1.EquipmentUom) AS  Examined");
            dapperExecution.SqlBuilder.Select("CONCAT_WS (' ', SUM(t1.RealUnit), t1.EquipmentUom) AS  Deployed");
            dapperExecution.SqlBuilder.Select("CONCAT_WS (' ', SUM(t1.ReclaimedUnit), t1.EquipmentUom) AS  Reclaimed");
            dapperExecution.SqlBuilder.Select("CONCAT_WS (' ', SUM(IFNULL(t1.SupporterHoldedUnit, 0)), t1.EquipmentUom) AS  SupporterHolded");
            dapperExecution.SqlBuilder.Select("CONCAT_WS (' ', SUM(t1.ActivatedUnit), t1.EquipmentUom) AS  Activated");

            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages AS t2" +
                " ON t2.EndPointChannelId = t1.OutputChannelPointId OR t2.StartPointChannelId = t1.OutputChannelPointId");
            dapperExecution.SqlBuilder.InnerJoin("OutContracts AS t3 ON t3.Id = t2.OutContractId");
            dapperExecution.SqlBuilder.InnerJoin("Contractors AS t4 ON t4.Id = t3.ContractorId");
            dapperExecution.SqlBuilder.LeftJoin("ContractorProperties AS t5 ON t5.ContractorId = t4.Id");
            dapperExecution.SqlBuilder.LeftJoin("Projects t6 ON t6.Id = t2.ProjectId");

            if (filter.EquipmentIds != null && filter.EquipmentIds.Any())
            {
                var equipmentIds = string.Join(",", filter.EquipmentIds);

                dapperExecution.SqlBuilder.Where("FIND_IN_SET(t1.EquipmentId, @equipmentIds)", new { equipmentIds });
            }

            dapperExecution.SqlBuilder.Where("(@marketAreaId = 0 OR t3.MarketAreaId = @marketAreaId)",
                new
                {
                    marketAreaId = filter.MarketAreaId
                });

            if (filter.TimelineSignedStart.HasValue && filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) BETWEEN DATE(@startDate) AND DATE(@endDate)", new
                {
                    startDate = filter.TimelineSignedStart.Value.ToExactLocalDate(),
                    endDate = filter.TimelineSignedEnd.Value.ToExactLocalDate()
                });
            }
            else if (filter.TimelineSignedStart.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) >= DATE(@startDate)", new
                {
                    startDate = filter.TimelineSignedStart.Value.ToExactLocalDate()
                });
            }
            else if (filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) <= DATE(@endDate)", new
                {
                    endDate = filter.TimelineSignedEnd.Value.ToExactLocalDate()
                });
            }

            if (filter.TimelineEffectiveStart.HasValue && filter.TimelineEffectiveEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Effective) BETWEEN DATE(@startDate) AND DATE(@endDate)", new
                {
                    startDate = filter.TimelineEffectiveStart.Value.ToExactLocalDate(),
                    endDate = filter.TimelineEffectiveEnd.Value.ToExactLocalDate()
                });
            }
            else if (filter.TimelineEffectiveStart.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Effective) >= DATE(@startDate)", new
                {
                    startDate = filter.TimelineEffectiveStart.Value.ToExactLocalDate()
                });
            }
            else if (filter.TimelineEffectiveEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Effective) <= DATE(@endDate)", new
                {
                    endDate = filter.TimelineEffectiveEnd.Value.ToExactLocalDate()
                });
            }

            if (!string.IsNullOrEmpty(filter.SerialCode))
            {
                dapperExecution.SqlBuilder.Where("t1.SerialCode LIKE @serialCode", new { serialCode = $"%{filter.SerialCode}%" });
            }

            if (filter.CustomerCategoryId.HasValue)
            {
                dapperExecution.SqlBuilder
                    .Where("t5.ContractorCategoryId = @customerCategoryId", new
                    {
                        contractorCategoryId = filter.CustomerCategoryId
                    });
            }

            if (!string.IsNullOrEmpty(filter.ContractCode))
            {
                dapperExecution.SqlBuilder.Where("t3.ContractCode LIKE @contractCode", new { contractCode = $"%{filter.ContractCode}%" });
            }

            if (!string.IsNullOrEmpty(filter.CustomerCode))
            {
                dapperExecution.SqlBuilder.Where("t4.ContractorCode LIKE @customerCode", new { customerCode = $"%{filter.CustomerCode}%" });
            }

            if (filter.ProjectId > 0)
            {
                dapperExecution.SqlBuilder.Where("t3.ProjectId = @projectId", new { projectId = filter.ProjectId });
            }

            if (filter.ServiceId > 0)
            {
                dapperExecution.SqlBuilder.Where("t2.ServiceId = @serviceId", new { serviceId = filter.ServiceId });
            }

            if (filter.IsEnterprise.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t4.IsEnterprise = @isEnterprise", new { isEnterprise = filter.IsEnterprise.Value });
            }
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                dapperExecution.SqlBuilder.Where("t4.ContractorFullName LIKE @contractorFullName", new { contractorFullName = $"%{filter.CustomerName}%" });
            }

            if ("CustomerName".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"t4.ContractorFullName {filter.Dir}");
            }
            else if ("Examined".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"SUM(t1.ExaminedUnit) {filter.Dir}");
            }
            else if ("Deployed".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"SUM(t1.RealUnit) {filter.Dir}");
            }
            else if ("Reclaimed".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"SUM(t1.ReclaimedUnit) {filter.Dir}");
            }
            else if ("SupporterHolded".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"SUM(t1.SupporterHoldedUnit) {filter.Dir}");
            }
            else if ("Activated".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"SUM(t1.ActivatedUnit) {filter.Dir}");
            }
            else if ("ProjectName".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"t6.ProjectName {filter.Dir}");
            }
            else if ("CustomerCategory".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"t5.ContractorCategoryName {filter.Dir}");
            }
            else if ("ContractCode".Equals(filter.OrderBy, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.OrderBy($"t3.ContractCode {filter.Dir}");
            }

            dapperExecution.SqlBuilder.GroupBy("t1.EquipmentId, t3.Id, t2.Id");

            return dapperExecution.ExecutePaginateQuery();
        }
        public PagedList<ReportContractorInProjectDTO> GetContractorInProjectOfOutContract(ContractorInProjectFilterModel contractorInProjectFilterModel)
        {
            var p = new DynamicParameters();
            var projects = contractorInProjectFilterModel.ProjectIds != null ? String.Join(",", contractorInProjectFilterModel.ProjectIds) : "";
            var packageIds = contractorInProjectFilterModel.PackageIds != null ? String.Join(",", contractorInProjectFilterModel.PackageIds) : "";
            p.Add("skips", contractorInProjectFilterModel.Paging == true ? contractorInProjectFilterModel.Skip : 0);
            p.Add("take", contractorInProjectFilterModel.Paging == true ? contractorInProjectFilterModel.Take : int.MaxValue);
            p.Add("startDate", contractorInProjectFilterModel.StartDate != null ? contractorInProjectFilterModel.StartDate : DateTime.MinValue);
            p.Add("endDate", contractorInProjectFilterModel.EndDate != null ? contractorInProjectFilterModel.EndDate : DateTime.Now);
            p.Add("projectIds", projects);
            p.Add("packageIds", packageIds);
            var equipments = WithConnection((conn) =>
                conn.Query<ReportContractorInProjectDTO>("GetContractorInProjectOfOutContract", p, commandType: CommandType.StoredProcedure));
            var page = new PagedList<ReportContractorInProjectDTO>(contractorInProjectFilterModel.Skip, contractorInProjectFilterModel.Take, equipments.Count());
            var total = equipments.Count() > 0 ? equipments.First().Total : 0;
            page.Subset = equipments.ToList();
            page.TotalItemCount = total;
            return page;
        }

        public PagedList<ReportMasterCustomerNationwideBusinessDTO> GetListMasterCustomerNationwideBusiness(MasterCustomerNationwideBusinessFilterModel filterModel)
        {
            var p = new DynamicParameters();
            p.Add("skips", filterModel.Paging == true ? filterModel.Skip : 0);
            p.Add("take", filterModel.Paging == true ? filterModel.Take : int.MaxValue);
            p.Add("groupBy", filterModel.GroupBy);

            p.Add("timeLineSignedStartDate", filterModel.TimelineSignedStart, DbType.Date);
            p.Add("timeLineSignedEndDate", filterModel.TimelineSignedEnd, DbType.Date);
            p.Add("marketAreaId", filterModel.MarketAreaId);
            p.Add("customerCode", filterModel.CustomerCode ?? "");
            p.Add("serviceId", filterModel.ServiceId);
            p.Add("projectId", filterModel.ProjectId);
            p.Add("contractCode", filterModel.ContractCode ?? "");
            p.Add("customerCategoryId", filterModel.CustomerCategoryId);
            p.Add("statusId", filterModel.StatusId);

            p.Add("orderBy", "ORDER BY t1." + filterModel.OrderBy.ToUpperFirstLetter() + " " + filterModel.Dir);

            p.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var equipments = WithConnection((conn) =>
               conn.Query<ReportMasterCustomerNationwideBusinessDTO>("GetListMasterCustomerReport", p, commandType: CommandType.StoredProcedure));
            var page = new PagedList<ReportMasterCustomerNationwideBusinessDTO>(filterModel.Skip, filterModel.Take, equipments.Count());
            var totalRecords = p.Get<int>("total");
            page.Subset = equipments.ToList();
            page.TotalItemCount = totalRecords;
            return page;
            //var masters = WithConnection((conn) =>
            //    conn.Query<ReportMasterCustomerNationwideBusinessDTO>("GetListMasterCustomerReport", p, commandType: CommandType.StoredProcedure));

            //int lastOutContractId = 0;
            //var lastServicePackageId = new List<int>();
            //var lastInContractId = new List<int>();
            //var inContractInfo = new InContractInfoForReport();
            //var outContractInfo = new OutContractInfoForReport();
            //var lsiOutContractInfo = new List<OutContractInfoForReport>();
            //var servicePackage = new ServicePackageForMasterReport();
            //foreach (var reportData in masters)
            //{
            //    if (lastOutContractId == 0 || lastOutContractId != reportData.OutContractId)
            //    {
            //        if (lastOutContractId != 0)
            //        {
            //            if (!string.IsNullOrEmpty(servicePackage.ServiceName))
            //            {
            //                outContractInfo.ServicePackages.Add(servicePackage);
            //            }

            //            if (inContractInfo.InContractId != 0)
            //            {
            //                outContractInfo.InContractInfos.Add(inContractInfo);
            //            }

            //            lsiOutContractInfo.Add(outContractInfo);
            //            lastServicePackageId = new List<int>();
            //            lastInContractId = new List<int>();
            //            inContractInfo = new InContractInfoForReport();
            //            servicePackage = new ServicePackageForMasterReport();
            //        }
            //        lastOutContractId = reportData.OutContractId;
            //        outContractInfo = _mapper.Map<OutContractInfoForReport>(reportData);
            //    }

            //    if ((lastServicePackageId.Count == 0 || !lastServicePackageId.Contains(reportData.ServicePackageId)) && reportData.ServicePackageId != 0)
            //    {
            //        if (lastServicePackageId.Count != 0)
            //            outContractInfo.ServicePackages.Add(servicePackage);
            //        lastServicePackageId.Add(reportData.ServicePackageId);
            //        servicePackage = new ServicePackageForMasterReport();
            //        _mapper.Map(reportData, servicePackage);
            //        servicePackage.EndPoint.InstallationAddress.Street = reportData.InstallationAddressEndPoint;
            //        servicePackage.StartPoint.InstallationAddress.Street = reportData.InstallationAddressStartPoint;
            //        if (reportData.TimeLineEffective != null)
            //            servicePackage.TimeLineEffective = DateTime.Parse(reportData.TimeLineEffective.ToString()).ToString("yyyy-MM-dd");
            //    }


            //    if ((lastInContractId.Count == 0 || !lastInContractId.Contains(reportData.InContractId)) && reportData.InContractId != 0)
            //    {
            //        if (lastInContractId.Count != 0)
            //            outContractInfo.InContractInfos.Add(inContractInfo);
            //        lastInContractId.Add(reportData.InContractId);
            //        inContractInfo = new InContractInfoForReport();
            //        inContractInfo = _mapper.Map<InContractInfoForReport>(reportData);
            //    }
            //}

            //if (inContractInfo.InContractId != 0)
            //{
            //    outContractInfo.InContractInfos.Add(inContractInfo);
            //}
            //if (!string.IsNullOrEmpty(servicePackage.ServiceName))
            //{
            //    outContractInfo.ServicePackages.Add(servicePackage);
            //}
            //lsiOutContractInfo.Add(outContractInfo);
            //var con = lsiOutContractInfo.Where(e => e.OutContractId == 0).FirstOrDefault();
            //lsiOutContractInfo.Remove(con);
            //var totalRecords = p.Get<int>("total");
            //var page = new PagedList<OutContractInfoForReport>(filterModel.Skip, filterModel.Take, lsiOutContractInfo.Count());
            //page.Subset = filterModel.Paging ? lsiOutContractInfo.Skip(filterModel.Skip).Take(filterModel.Take).ToList() : lsiOutContractInfo.ToList();
            //page.TotalItemCount = totalRecords;
            //return page;
        }

        public PagedList<ReportTotalRevenueDTO> GetDataTotalRevenue(ReportTotalRevenueFillter reportFilter)
        {
            var p = new DynamicParameters();
            p.Add("skips", reportFilter.Paging == true ? reportFilter.Skip : 0);
            p.Add("take", reportFilter.Paging == true ? reportFilter.Take : int.MaxValue);

            p.Add("reportType", reportFilter.ReportType);

            p.Add("marketAreaId", reportFilter.MarketAreaId);
            p.Add("startDate", reportFilter.TimelineSignedStart, DbType.Date);
            p.Add("endDate", reportFilter.TimelineSignedEnd, DbType.Date);
            p.Add("startEffectDate", reportFilter.TimelineEffectiveStart, DbType.Date);
            p.Add("endEffectDate", reportFilter.TimelineEffectiveEnd, DbType.Date);
            p.Add("customerCode", reportFilter.CustomerCode ?? "");
            p.Add("serviceId", reportFilter.ServiceId);
            p.Add("projectId", reportFilter.ProjectId);
            p.Add("currencyUnitId", reportFilter.CurrencyUnitId);
            p.Add("contractCode", reportFilter.ContractCode ?? "");

            p.Add("isEnterprise", reportFilter.IsEnterprise);
            p.Add("signedUserId", reportFilter.SignedUserId ?? "");
            p.Add("organizationUnitId", reportFilter.OrganizationUnitId ?? "");
            p.Add("orderBy", "ORDER BY " + reportFilter.OrderBy.ToUpperFirstLetter() + " " + reportFilter.Dir);
            p.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("sSqlQue", dbType: DbType.String, direction: ParameterDirection.Output);
            var records = WithConnection((conn) =>
                conn.Query<ReportTotalRevenueDTO>("sp_GetReportEstimateRevenue", p, commandType: CommandType.StoredProcedure).Distinct().ToList());

            var totalRecords = p.Get<int>("total");
            var sQue = p.Get<string>("sSqlQue");
            var result = new PagedList<ReportTotalRevenueDTO>(reportFilter.Skip, reportFilter.Take, totalRecords)
            {
                Subset = records,
                TotalItemCount = totalRecords
            };

            return result;
        }
        public PagedList<ReportTotalRevenueDTO> GetReportIncreasementOutContract(ReportTotalRevenueFillter reportFilter)
        {
            var p = new DynamicParameters();

            p.Add("skips", reportFilter.Paging == true ? reportFilter.Skip : 0);
            p.Add("take", reportFilter.Paging == true ? reportFilter.Take : int.MaxValue);

            p.Add("reportType", reportFilter.ReportType);

            p.Add("marketAreaId", reportFilter.MarketAreaId);
            p.Add("customerCode", reportFilter.CustomerCode ?? "");
            p.Add("startDate", reportFilter.TimelineSignedStart, DbType.Date);
            p.Add("endDate", reportFilter.TimelineSignedEnd, DbType.Date);
            p.Add("startEffectDate", reportFilter.TimelineEffectiveStart, DbType.Date);
            p.Add("endEffectDate", reportFilter.TimelineEffectiveEnd, DbType.Date);

            p.Add("serviceId", reportFilter.ServiceId);
            p.Add("projectId", reportFilter.ProjectId);
            p.Add("contractCode", reportFilter.ContractCode ?? "");
            p.Add("transactionCode", reportFilter.TransactionCode ?? "");

            p.Add("isEnterprise", reportFilter.IsEnterprise);
            p.Add("signedUserId", reportFilter.SignedUserId ?? "");
            p.Add("organizationUnitId", reportFilter.OrganizationUnitId ?? "");
            p.Add("orderBy", "ORDER BY t1." + reportFilter.OrderBy.ToUpperFirstLetter() + " " + reportFilter.Dir);
            p.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var records = WithConnection((conn) =>
                conn.Query<ReportTotalRevenueDTO>("sp_GetReportIncreasementOutContract", p, commandType: CommandType.StoredProcedure).Distinct().ToList());

            var totalRecords = p.Get<int>("total");
            var result = new PagedList<ReportTotalRevenueDTO>(reportFilter.Skip, reportFilter.Take, totalRecords)
            {
                Subset = records,
                TotalItemCount = totalRecords
            };

            return result;
        }

        public PagedList<CommissionAndSharingReportDTO> GetCommissionAndSharingReportData(CommissionAndSharingReportFilter reportFilter)
        {
            var p = new DynamicParameters();
            var records = WithConnection((conn) =>
               conn.Query<CommissionAndSharingReportDTO>("sp_GetReportIncreasementOutContract", p, commandType: CommandType.StoredProcedure).Distinct().ToList());

            var totalRecords = p.Get<int>("total");
            var result = new PagedList<CommissionAndSharingReportDTO>(reportFilter.Skip, reportFilter.Take, totalRecords)
            {
                Subset = reportFilter.Paging ? records.Skip(reportFilter.Skip).Take(reportFilter.Take).ToList() : records,
                TotalItemCount = totalRecords
            };

            return result;
        }

        public PagedList<ReportFeeDTO> GetFeeReportData(FeeReportFilter reportFilter)
        {
            var storedProcedureParams = new DynamicParameters();
            storedProcedureParams.Add("sharingType", reportFilter.TypeId); // loại chi phí
            storedProcedureParams.Add("marketAreaId", reportFilter.MarketAreaId);
            storedProcedureParams.Add("projectId", reportFilter.ProjectId);
            storedProcedureParams.Add("outCustomerCode", reportFilter.CustomerCode?.Trim() ?? ""); //mã khách hàng đầu ra
            storedProcedureParams.Add("contractorFullName", reportFilter.ContractorFullName?.Trim() ?? "");
            storedProcedureParams.Add("inContractCode", reportFilter.ContractCode?.Trim() ?? ""); //Mã hợp đồng đầu vào
            storedProcedureParams.Add("serviceId", reportFilter.ServiceId);
            storedProcedureParams.Add("effectiveStartDate", reportFilter.TimelineEffectiveStart ?? null, DbType.Date);
            storedProcedureParams.Add("effectiveEndDate", reportFilter.TimelineEffectiveEnd ?? null, DbType.Date);
            storedProcedureParams.Add("currencyUnitCode", reportFilter.CurrencyUnitCode ?? "");
            storedProcedureParams.Add("skips", reportFilter.Skip);
            storedProcedureParams.Add("take", reportFilter.Take);
            storedProcedureParams.Add("totalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumInstallationFee", dbType: DbType.Int32, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumFeeOfMonthByOutContract", dbType: DbType.Int32, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumCostOfInstallFee", dbType: DbType.Int32, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumCostOfFeeInMonth", dbType: DbType.Int32, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumEachMonthVND", dbType: DbType.String, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumAllMonthsVND", dbType: DbType.Int32, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumEachMonthUSD", dbType: DbType.String, direction: ParameterDirection.Output);
            storedProcedureParams.Add("sumAllMonthsUSD", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var records = WithConnection((conn) =>
               conn.Query<ReportFeeDTO>("sp_GetFeeReportData", storedProcedureParams, commandType: CommandType.StoredProcedure));
            var totalRecords = storedProcedureParams.Get<int>("totalRecords");
            var sumInstallationFee = storedProcedureParams.Get<int?>("sumInstallationFee") ?? 0;
            var sumFeeOfMonthByOutContract = storedProcedureParams.Get<int?>("sumFeeOfMonthByOutContract") ?? 0;
            var sumCostOfInstallFee = storedProcedureParams.Get<int?>("sumCostOfInstallFee") ?? 0;
            var sumCostOfFeeInMonth = storedProcedureParams.Get<int?>("sumCostOfFeeInMonth") ?? 0;
            var sumEachMonthVND = storedProcedureParams.Get<string>("sumEachMonthVND") != null ? storedProcedureParams.Get<string>("sumEachMonthVND") : null;
            var sumAllMonthsVND = storedProcedureParams.Get<int?>("sumAllMonthsVND") ?? 0;
            var sumEachMonthUSD = storedProcedureParams.Get<string>("sumEachMonthUSD") != null ? storedProcedureParams.Get<string>("sumEachMonthUSD") : null;
            var sumAllMonthsUSD = storedProcedureParams.Get<int?>("sumAllMonthsUSD") ?? 0;

            var totalDataReportFee = new TotalDataReportFeeDTO()
            {
                SumInstallationFee = sumInstallationFee,
                SumFeeOfMonthByOutContract = sumFeeOfMonthByOutContract,
                SumCostOfInstallFee = sumCostOfInstallFee,
                SumCostOfFeeInMonth = sumCostOfFeeInMonth,
                SumEachMonthVND = sumEachMonthVND != null ? sumEachMonthVND.Split('/') : null,
                SumAllMonthsVND = sumAllMonthsVND,
                SumEachMonthUSD = sumEachMonthUSD != null ? sumEachMonthUSD.Split('/') : null,
                SumAllMonthsUSD = sumAllMonthsUSD
            };
            if (records.Count() > 0)
            {
                records.ElementAt(0).TotalDataReportFee = totalDataReportFee;
            }

            var result = new PagedList<ReportFeeDTO>(reportFilter.Skip, reportFilter.Take, totalRecords)
            {
                Subset = records.ToList(),
                TotalItemCount = totalRecords
            };

            return result;
        }
        public PagedList<ReportTotalFeeByCurrency> GetFeeTotalReportData(FeeReportFilter reportFilter)
        {
            var storedProcedureParams = new DynamicParameters();
            storedProcedureParams.Add("marketAreaId", reportFilter.MarketAreaId);
            storedProcedureParams.Add("outCustomerCode", reportFilter.CustomerCode?.Trim() ?? ""); //mã khách hàng đầu ra
            storedProcedureParams.Add("inContractCode", reportFilter.ContractCode?.Trim() ?? ""); //Mã hợp đồng đầu vào
            storedProcedureParams.Add("outContractCode", reportFilter.OutContractCode?.Trim() ?? "");
            storedProcedureParams.Add("customerName", reportFilter.CustomerName?.Trim() ?? "");
            storedProcedureParams.Add("serviceId", reportFilter.ServiceId);
            storedProcedureParams.Add("projectId", reportFilter.ProjectId);
            storedProcedureParams.Add("month", reportFilter.Month);
            storedProcedureParams.Add("quarter", reportFilter.Quarter);
            storedProcedureParams.Add("year", reportFilter.Year);
            storedProcedureParams.Add("signedStartDate", reportFilter.TimelineSignedStart ?? null, DbType.Date);
            storedProcedureParams.Add("signedEndDate", reportFilter.TimelineSignedEnd ?? null, DbType.Date);
            storedProcedureParams.Add("take", reportFilter.Take);
            storedProcedureParams.Add("skip", reportFilter.Skip);
            storedProcedureParams.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var records = WithConnection((conn) =>
               conn.Query<ReportTotalFeeByCurrency>(
                   "sp_GetFeeTotalReportData",
                   storedProcedureParams,
                   commandType: CommandType.StoredProcedure)
               .Distinct()
               .ToList());

            var total = storedProcedureParams.Get<int>("total");
            var result = new PagedList<ReportTotalFeeByCurrency>(reportFilter.Skip, reportFilter.Take, total)
            {
                Subset = records.ToList(),
                TotalItemCount = total
            };
            return result;
        }

        public TotalDataIncontractFeeSharingReportDto GetTotalDataIncontractFeeSharingReport(FeeReportFilter reportFilter)
        {
            var storedProcedureParams = new DynamicParameters();
            storedProcedureParams.Add("marketAreaId", reportFilter.MarketAreaId);
            storedProcedureParams.Add("outCustomerCode", reportFilter.CustomerCode ?? ""); //mã khách hàng đầu ra
            storedProcedureParams.Add("inContractCode", reportFilter.ContractCode ?? ""); //Mã hợp đồng đầu vào
            storedProcedureParams.Add("outContractCode", reportFilter.OutContractCode ?? "");
            storedProcedureParams.Add("customerName", reportFilter.CustomerName ?? "");
            storedProcedureParams.Add("serviceId", reportFilter.ServiceId);
            storedProcedureParams.Add("projectId", reportFilter.ProjectId);
            storedProcedureParams.Add("month", reportFilter.Month);
            storedProcedureParams.Add("quarter", reportFilter.Quarter);
            storedProcedureParams.Add("year", reportFilter.Year);
            storedProcedureParams.Add("signedStartDate", reportFilter.TimelineSignedStart ?? null, DbType.Date);
            storedProcedureParams.Add("signedEndDate", reportFilter.TimelineSignedEnd ?? null, DbType.Date);

            var result = WithConnection((conn) =>
               conn.QueryFirst<TotalDataIncontractFeeSharingReportDto>(
                   "sp_GetTotalDataIncontractFeeSharingReport",
                    storedProcedureParams,
                    commandType: CommandType.StoredProcedure));

            if (result != null)
            {
                result.ShareOtherTotal = result.ShareAllTotal - result.ShareChannelRentalTotal
                    - result.ShareCommissionTotal - result.ShareRevenueTotal
                     - result.ShareMaintenanceTotal - result.ShareInfrastructureTotal
                      - result.ShareConstructionTotal - result.ShareEquipmentTotal
                       - result.ShareSupplyTotal - result.ShareCostOfAcceptanceTotal;
                if (result.ShareOtherTotal < 0) result.ShareOtherTotal = 0;
            }
            return result;
        }

        public IEnumerable<OutContractSimpleDTO> GetOutContractInfos(Global.Models.Filter.ReportFilterBase filterModel)
        {
            var spParams = new DynamicParameters();
            spParams.Add("skips", filterModel.Paging == true ? filterModel.Skip : 0);
            spParams.Add("take", filterModel.Paging == true ? filterModel.Take : int.MaxValue);

            spParams.Add("timeLineSignedStartDate", filterModel.TimelineSignedStart, DbType.Date);
            spParams.Add("timeLineSignedEndDate", filterModel.TimelineSignedEnd, DbType.Date);
            spParams.Add("marketAreaId", filterModel.MarketAreaId);
            spParams.Add("customerCode", filterModel.CustomerCode ?? "");
            spParams.Add("serviceId", filterModel.ServiceId);
            spParams.Add("projectId", filterModel.ProjectId);
            spParams.Add("contractCode", filterModel.ContractCode ?? "");

            spParams.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var cached = new Dictionary<int, OutContractSimpleDTO>();
            //var validChannelStatuses = OutContractServicePackageStatus.ValidStatuses();
            return WithConnection(conn => conn.Query("GetOutContractSimpleAllByInContractId",
                new[]
                {
                    typeof(OutContractSimpleDTO),
                    typeof(ContractorSimpleDTO),
                    typeof(ContractPackageSimpleDTO),
                    typeof(BillingTimeLine),
                    typeof(OutputChannelPointSimpleDTO),
                    typeof(InstallationAddress),
                    typeof(OutputChannelPointSimpleDTO),
                    typeof(InstallationAddress),
                    typeof(ContractTotalByCurrencyDTO)
                },
                results =>
                {
                    var transactionEntry = results[0] as OutContractSimpleDTO;
                    if (transactionEntry == null) return null;

                    if (!cached.TryGetValue(transactionEntry.Id, out var result))
                    {
                        result = transactionEntry;
                        cached.Add(transactionEntry.Id, transactionEntry);
                    }

                    result.Contractor = results[1] as ContractorSimpleDTO;

                    var channel = results[2] as ContractPackageSimpleDTO;
                    channel.TimeLine = results[3] as BillingTimeLine;

                    var existChannel = transactionEntry.ServicePackages.FirstOrDefault(c => c.Id == channel.Id);
                    if (existChannel == null)
                    {
                        result.ServicePackages.Add(channel);
                    }
                    else
                    {
                        existChannel = channel;
                    }

                    var channelStartPoint = results[4] as OutputChannelPointSimpleDTO;
                    if (channel.StartPoint == null) channel.StartPoint = channelStartPoint;
                    if (channel.StartPoint != null)
                    {
                        channel.StartPoint.InstallationAddress = results[5] as InstallationAddress;
                    }

                    var channelEndPoint = results[6] as OutputChannelPointSimpleDTO;
                    if (channel.EndPoint == null) channel.EndPoint = channelEndPoint;
                    if (channel.EndPoint != null)
                    {
                        channel.EndPoint.InstallationAddress = results[7] as InstallationAddress;
                    }

                    var contractTotal = results[8] as ContractTotalByCurrencyDTO;
                    if (transactionEntry.ContractTotalByCurrencies.All(s => s.Id != contractTotal.Id))
                    {
                        transactionEntry.ContractTotalByCurrencies.Add(contractTotal);
                    }

                    return result;
                },
                spParams, //, validChannelStatuses },
                null,
                true,
                "Id,Id,PaymentPeriod,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter,Id",
                null,
                CommandType.StoredProcedure))
            .Distinct()
            .ToList();

        }

        public async Task<PagedList<TotalRevenuePersonalDTO>> GetTotalRevenueEnterpiseReport(MasterCustomerNationwideBusinessFilterModel requestFilterModel)
        {
            var storedProcedureParams = new DynamicParameters();
            storedProcedureParams.Add("skips", requestFilterModel.Paging ? requestFilterModel.Skip : 0);
            storedProcedureParams.Add("take", requestFilterModel.Paging ? requestFilterModel.Take : int.MaxValue);

            storedProcedureParams.Add("marketAreaId", requestFilterModel.MarketAreaId);
            storedProcedureParams.Add("contractCode", requestFilterModel.ContractCode?.Trim() ?? "");
            storedProcedureParams.Add("customerCode", requestFilterModel.CustomerCode?.Trim() ?? "");
            storedProcedureParams.Add("contractorFullName", requestFilterModel.ContractorFullName?.Trim() ?? "");
            storedProcedureParams.Add("effectiveStartDate", requestFilterModel.TimelineEffectiveStart, DbType.Date);
            storedProcedureParams.Add("effectiveEndDate", requestFilterModel.TimelineEffectiveEnd, DbType.Date);
            storedProcedureParams.Add("serviceId", requestFilterModel.ServiceId);
            storedProcedureParams.Add("projectId", requestFilterModel.ProjectId);
            storedProcedureParams.Add("orderBy", requestFilterModel.OrderBy);
            storedProcedureParams.Add("statusId", requestFilterModel.StatusId);
            storedProcedureParams.Add("customerCategoryId", requestFilterModel.CustomerCategoryId);
            storedProcedureParams.Add("reportYear", requestFilterModel.ReportYear);

            var transactionSpParams = new DynamicParameters();
            transactionSpParams.Add("marketAreaId", requestFilterModel.MarketAreaId);
            transactionSpParams.Add("contractCode", requestFilterModel.ContractCode?.Trim() ?? "");
            transactionSpParams.Add("customerCode", requestFilterModel.CustomerCode?.Trim() ?? "");
            transactionSpParams.Add("contractorFullName", requestFilterModel.ContractorFullName?.Trim() ?? "");
            transactionSpParams.Add("effectiveStartDate", requestFilterModel.TimelineEffectiveStart, DbType.Date);
            transactionSpParams.Add("effectiveEndDate", requestFilterModel.TimelineEffectiveEnd, DbType.Date);
            transactionSpParams.Add("serviceId", requestFilterModel.ServiceId);
            transactionSpParams.Add("projectId", requestFilterModel.ProjectId);
            transactionSpParams.Add("orderBy", requestFilterModel.OrderBy);
            transactionSpParams.Add("statusId", requestFilterModel.StatusId);
            transactionSpParams.Add("customerCategoryId", requestFilterModel.CustomerCategoryId);
            transactionSpParams.Add("reportYear", requestFilterModel.ReportYear);

            var summarySpParams = new DynamicParameters();
            summarySpParams.Add("marketAreaId", requestFilterModel.MarketAreaId);
            summarySpParams.Add("contractCode", requestFilterModel.ContractCode?.Trim() ?? "");
            summarySpParams.Add("customerCode", requestFilterModel.CustomerCode?.Trim() ?? "");
            summarySpParams.Add("contractorFullName", requestFilterModel.ContractorFullName?.Trim() ?? "");
            summarySpParams.Add("effectiveStartDate", requestFilterModel.TimelineEffectiveStart, DbType.Date);
            summarySpParams.Add("effectiveEndDate", requestFilterModel.TimelineEffectiveEnd, DbType.Date);
            summarySpParams.Add("serviceId", requestFilterModel.ServiceId);
            summarySpParams.Add("projectId", requestFilterModel.ProjectId);
            summarySpParams.Add("orderBy", requestFilterModel.OrderBy);
            summarySpParams.Add("statusId", requestFilterModel.StatusId);
            summarySpParams.Add("customerCategoryId", requestFilterModel.CustomerCategoryId);
            summarySpParams.Add("reportYear", requestFilterModel.ReportYear);

            var debtSpPrams = new DynamicParameters();
            debtSpPrams.Add("marketAreaId", requestFilterModel.MarketAreaId);
            debtSpPrams.Add("contractCode", requestFilterModel.ContractCode?.Trim() ?? "");
            debtSpPrams.Add("customerCode", requestFilterModel.CustomerCode?.Trim() ?? "");
            debtSpPrams.Add("contractorFullName", requestFilterModel.ContractorFullName?.Trim() ?? "");
            debtSpPrams.Add("effectiveStartDate", requestFilterModel.TimelineEffectiveStart, DbType.Date);
            debtSpPrams.Add("effectiveEndDate", requestFilterModel.TimelineEffectiveEnd, DbType.Date);
            debtSpPrams.Add("serviceId", requestFilterModel.ServiceId);
            debtSpPrams.Add("projectId", requestFilterModel.ProjectId);
            debtSpPrams.Add("orderBy", requestFilterModel.OrderBy);
            debtSpPrams.Add("statusId", requestFilterModel.StatusId);
            debtSpPrams.Add("customerCategoryId", requestFilterModel.CustomerCategoryId);
            debtSpPrams.Add("reportYear", requestFilterModel.ReportYear);
            debtSpPrams.Add("totalDebt", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            debtSpPrams.Add("totalPaid", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            debtSpPrams.Add("totalClearing", dbType: DbType.Decimal, direction: ParameterDirection.Output);

            var getContractDataTask = WithConnectionAsync((conn) =>
                conn.Query<TotalRevenuePersonalDTO, InstallationAddress, InstallationAddress, TotalRevenuePersonalDTO>(
                    "GetTotalRevenueEnterpiseReport",
                    (revenueLine, endPoint, startPoint) =>
                    {
                        revenueLine.EndPoint = endPoint;
                        revenueLine.StartPoint = startPoint;
                        return revenueLine;
                    },
                    storedProcedureParams,
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Street,Street"
                )
            );

            if (requestFilterModel.Paging)
            {
                await getContractDataTask;
                if (getContractDataTask.Result == null || getContractDataTask.Result.Count() == 0)
                    return new PagedList<TotalRevenuePersonalDTO>(requestFilterModel.Skip, requestFilterModel.Take, 0);

                transactionSpParams.Add("outContractIds", string.Join(",", getContractDataTask.Result.Select(c => c.Id)), dbType: DbType.String);
                summarySpParams.Add("outContractIds", string.Join(",", getContractDataTask.Result.Select(c => c.Id)), dbType: DbType.String);
                debtSpPrams.Add("outContractIds", string.Join(",", getContractDataTask.Result.Select(c => c.Id)), dbType: DbType.String);
            }
            else
            {
                transactionSpParams.Add("outContractIds", string.Empty, dbType: DbType.String);
                summarySpParams.Add("outContractIds", string.Empty, dbType: DbType.String);
                debtSpPrams.Add("outContractIds", string.Empty, dbType: DbType.String);
            }

            var getTransactionDataTask = WithConnectionAsync(conn => conn.Query<TotalRevenuePersonalDTO>(
                "GetTransactionOfOutContract",
                   transactionSpParams,
                   commandType: CommandType.StoredProcedure));

            var summaryQueryTask = WithConnectionAsync(conn =>
                conn.QueryFirst<TotalDataRevenuePersonalDTO>(
                    "GetSummaryPersonalCustomerRevenue",
                    summarySpParams,
                    commandType: CommandType.StoredProcedure));

            var debtDataTask = WithConnectionAsync((conn) =>
              conn.Query<DebtDTO>(
                  "GetTotalDebtReport",
                debtSpPrams,
                commandType: CommandType.StoredProcedure));

            if (requestFilterModel.Paging)
            {
                await Task.WhenAll(getTransactionDataTask, summaryQueryTask, debtDataTask);
            }
            else
            {
                await Task.WhenAll(getContractDataTask, getTransactionDataTask, summaryQueryTask, debtDataTask);
            }

            var result = new List<TotalRevenuePersonalDTO>();
            int totalRecords = 0;
            if (getContractDataTask.Result != null && getContractDataTask.Result.Any())
            {
                result = getContractDataTask.Result.Concat(getTransactionDataTask.Result).ToList();
                result = result
                    .OrderBy(c => c.Id)
                    .ThenBy(c => c.TransactionId)
                    .ThenBy(c => c.TimeLineEffective)
                    .ToList();

                if (summaryQueryTask.Result != null)
                {
                    if (!string.IsNullOrEmpty(summaryQueryTask.Result.SumJoinedEachMonthRevenue))
                    {
                        summaryQueryTask.Result.SumEachMonthTotal = summaryQueryTask.Result.SumJoinedEachMonthRevenue.Split('/');
                    }
                    if (!string.IsNullOrEmpty(summaryQueryTask.Result.SumJoinedEachMonthNewRevenue))
                    {
                        summaryQueryTask.Result.SumEachMonthNewTotal = summaryQueryTask.Result.SumJoinedEachMonthNewRevenue.Split('/');
                    }

                    summaryQueryTask.Result.SumDebtTotal = debtSpPrams.Get<decimal?>("totalDebt") ?? 0;
                    summaryQueryTask.Result.SumClearingTotal = debtSpPrams.Get<decimal?>("totalClearing") ?? 0;
                    summaryQueryTask.Result.SumPaidTotal = debtSpPrams.Get<decimal?>("totalPaid") ?? 0;

                    totalRecords = summaryQueryTask.Result.TotalRecords;

                    result.ElementAt(0).TotalDataRevenuePersonalDTO = summaryQueryTask.Result;
                }

                if (debtDataTask.Result != null && debtDataTask.Result.Any())
                {
                    foreach (var item in result.Where(x => !x.TransactionId.HasValue))
                    {
                        var debtItem = debtDataTask.Result
                            .FirstOrDefault(c => c.OutContractServicePackageId == item.OutContractServicePackageId);
                        if (debtItem != null)
                        {
                            item.DebtTotal = debtItem.DebtTotal;
                            item.ClearingTotal = debtItem.ClearingTotal;
                            item.PaidTotal = debtItem.PaidTotal;
                        }
                    }
                }
            }

            return new PagedList<TotalRevenuePersonalDTO>(requestFilterModel.Skip, requestFilterModel.Take, 1000)
            {
                Subset = result,
                TotalItemCount = totalRecords
            };
        }

        public async Task<PagedList<FTTHProjectRevenueDTO>> GetFTTHProjectRevenue(MasterCustomerNationwideBusinessFilterModel requestFilterModel)
        {
            var serviceConfig = _config.GetSection("ServiceConfigs").Get<ServiceConfigs>();

            var dataProcParams = new DynamicParameters();
            dataProcParams.Add("skips", requestFilterModel.Paging ? requestFilterModel.Skip : 0);
            dataProcParams.Add("take", requestFilterModel.Paging ? requestFilterModel.Take : int.MaxValue);
            dataProcParams.Add("marketAreaId", requestFilterModel.MarketAreaId);
            dataProcParams.Add("contractCode", requestFilterModel.ContractCode?.Trim() ?? "");
            dataProcParams.Add("customerCode", requestFilterModel.CustomerCode?.Trim() ?? "");
            dataProcParams.Add("contractorFullName", requestFilterModel.ContractorFullName?.Trim() ?? "");
            dataProcParams.Add("effectiveStartDate", requestFilterModel.TimelineEffectiveStart, DbType.Date);
            dataProcParams.Add("effectiveEndDate", requestFilterModel.TimelineEffectiveEnd, DbType.Date);
            dataProcParams.Add("tvServiceIds", serviceConfig.TVService, dbType: DbType.String);
            dataProcParams.Add("ftthServiceIds", serviceConfig.FTTHService, dbType: DbType.String);

            dataProcParams.Add("projectId", requestFilterModel.ProjectId);
            dataProcParams.Add("reportYear", requestFilterModel.ReportYear);

            var summaryProcParams = new DynamicParameters();
            summaryProcParams.Add("marketAreaId", requestFilterModel.MarketAreaId);
            summaryProcParams.Add("contractCode", requestFilterModel.ContractCode?.Trim() ?? "");
            summaryProcParams.Add("customerCode", requestFilterModel.CustomerCode?.Trim() ?? "");
            summaryProcParams.Add("contractorFullName", requestFilterModel.ContractorFullName?.Trim() ?? "");
            summaryProcParams.Add("effectiveStartDate", requestFilterModel.TimelineEffectiveStart, DbType.Date);
            summaryProcParams.Add("effectiveEndDate", requestFilterModel.TimelineEffectiveEnd, DbType.Date);
            summaryProcParams.Add("tvServiceIds", serviceConfig.TVService, dbType: DbType.String);
            summaryProcParams.Add("ftthServiceIds", serviceConfig.FTTHService, dbType: DbType.String);
            summaryProcParams.Add("projectId", requestFilterModel.ProjectId);
            summaryProcParams.Add("reportYear", requestFilterModel.ReportYear);

            var totalRecords = 0;

            var dataQueryTask = WithConnectionAsync(conn =>
                conn.Query<FTTHProjectRevenueDTO>(
                    "GetTotalFTTHProjectRevenue",
                    dataProcParams,
                    commandType: CommandType.StoredProcedure)
            );

            var summaryQueryTask = WithConnectionAsync(conn =>
                conn.QueryFirst<TotalDataFTTHProjectRevenueDTO>(
                    "GetSummaryFTTHProjectRevenue",
                    summaryProcParams,
                    commandType: CommandType.StoredProcedure));

            await Task.WhenAll(dataQueryTask, summaryQueryTask);

            var result = dataQueryTask.Result;

            if (result != null && result.Count() > 0 && summaryQueryTask.Result != null)
            {
                if (!string.IsNullOrEmpty(summaryQueryTask.Result.SumJoinedEachMonthRevenue))
                {
                    summaryQueryTask.Result.SumEachMonthRevenue = summaryQueryTask.Result.SumJoinedEachMonthRevenue.Split('/');
                }
                result.ElementAt(0).TotalDataFTTHProjectRevenueDTO = summaryQueryTask.Result;
                totalRecords = summaryQueryTask.Result.TotalRecords;
            }

            return new PagedList<FTTHProjectRevenueDTO>(
                requestFilterModel.Skip,
                requestFilterModel.Take,
                totalRecords)
            {
                Subset = result?.ToList(),
                TotalItemCount = totalRecords
            };
        }
    }
}
