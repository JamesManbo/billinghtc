using Dapper;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Domain.Models.PaymentVoucherModels;
using GenericRepository;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IPaymentVoucherReportQueries : IQueryRepository
    {
        IPagedList<PaymentVoucherReportGridDTO> GetList(PaymentVoucherReportFilter requestFilterModel);
        IPagedList<PaymentVoucherReportGridDTO> GetReportServiceDebtExcelData(PaymentVoucherReportFilter filterModel);
        IPagedList<PaymentVoucherReportGridDTO> GetReportServiceDebtData(PaymentVoucherReportFilter filterModel);
    }
    public class PaymentVoucherReportQueries : QueryRepository<PaymentVoucher, int>, IPaymentVoucherReportQueries
    {
        public PaymentVoucherReportQueries(DebtDbContext context) : base(context)
        {
        }

        public IPagedList<PaymentVoucherReportGridDTO> GetList(PaymentVoucherReportFilter requestFilterModel)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<PaymentVoucherReportGridDTO>(requestFilterModel);
            //dapperExecution.SqlBuilder.InnerJoin("VoucherTargets vt ON vt.Id = t1.TargetId");
           
            //dapperExecution.SqlBuilder.Select("vt.TargetFullName");
            //dapperExecution.SqlBuilder.Select("t1.ContractCode AS ContractCode");
            //dapperExecution.SqlBuilder.Select("t1.GrandTotal AS GrandTotal");
            //dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode AS CurrencyUnitCode");

            //dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            //dapperExecution.SqlBuilder.Where("t1.StatusId = 4 ");
            //dapperExecution.SqlBuilder.Where("YEAR(t1.IssuedDate) = @year", new { requestFilterModel.Year });
            //if (requestFilterModel.Month > 0)
            //{
            //    dapperExecution.SqlBuilder.Where("MONTH(t1.IssuedDate) = @month", new { requestFilterModel.Month });
            //}
            //if (requestFilterModel.ProjectId > 0)
            //{
            //    dapperExecution.SqlBuilder.Where("t1.ProjectId = @projectId", new { projectId = requestFilterModel.ProjectId });
            //}

            //if (requestFilterModel.Quarter > 0)
            //{
            //    dapperExecution.SqlBuilder.Where("MONTH(t1.IssuedDate) >= @fromMonth", new { fromMonth = requestFilterModel.Quarter - 2 });
            //    dapperExecution.SqlBuilder.Where("MONTH(t1.IssuedDate) <= @toMonth", new { toMonth = requestFilterModel.Quarter });
            //}
            //if (!string.IsNullOrEmpty(requestFilterModel.ContractCode))
            //{
            //    dapperExecution.SqlBuilder.Where("t1.ContractCode LIKE @contractCode ", new { contractCode = $"%{requestFilterModel.ContractCode}%" });
            //}
            //if (!string.IsNullOrEmpty(requestFilterModel.CustomerCode))
            //{
            //    dapperExecution.SqlBuilder.Where("vt.TargetCode LIKE @customerCode ", new { customerCode = $"%{requestFilterModel.CustomerCode}%" });
            //}

            //if (requestFilterModel.TypeId == 1)
            //{
            //    dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.OutContractServicePackages AS ic ON t1.InContractId = ic.InContractId");
            //    dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.OutputChannelPoints AS epc ON ic.EndPointChannelId = epc.Id");
            //    dapperExecution.SqlBuilder.LeftJoin("ITC_FBM_Contracts.OutputChannelPoints AS spc ON ic.StartPointChannelId = spc.Id");
            //    dapperExecution.SqlBuilder.Where("t1.TypeId = 1");
            //    dapperExecution.SqlBuilder.Select("CONCAT_WS(' ,', spc.InstallationAddress_Street, spc.InstallationAddress_District, spc.InstallationAddress_City) AS `StartPoint`");
            //    dapperExecution.SqlBuilder.Select("CONCAT_WS(' ,', epc.InstallationAddress_Street, epc.InstallationAddress_District, epc.InstallationAddress_City) AS `EndPoint`");
            //    if (requestFilterModel.Any("inputChannelName"))
            //    {
            //        var inputChannelName = requestFilterModel.GetProperty("inputChannelName");
            //        dapperExecution.SqlBuilder.OrWhere("ic.StartPoint LIKE @startPoint", new { startPoint = $"%{inputChannelName.FilterValue}%" });
            //        dapperExecution.SqlBuilder.OrWhere("ic.EndPoint LIKE @endPoint", new { endPoint = $"%{inputChannelName.FilterValue}%" });
            //    }
            //}
            //if (requestFilterModel.TypeId == 2 || requestFilterModel.TypeId == 3)
            //{
            //    dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.InContractServices AS ics ON ics.InContractId = t1.InContractId");
            //    dapperExecution.SqlBuilder.Where("t1.TypeId = @typeId", new { typeId = requestFilterModel.TypeId });
            //    dapperExecution.SqlBuilder.Select("ics.ServiceName");
            //    if(requestFilterModel.PercentDivision > 0 )
            //    {
            //        if(requestFilterModel.PercentDivision <= 5)
            //        {
            //            dapperExecution.SqlBuilder.Where("ics.SharedPackagePercent <= 5");
            //        }
            //        if(requestFilterModel.PercentDivision > 5 && requestFilterModel.PercentDivision <= 10)
            //        {
            //            dapperExecution.SqlBuilder.Where("ics.SharedPackagePercent > 5 AND ics.SharedPackagePercent <= 10");
            //        }
            //        if (requestFilterModel.PercentDivision > 10)
            //        {
            //            dapperExecution.SqlBuilder.Where("ics.SharedPackagePercent > 10");
            //        }
            //    }

            //    if (requestFilterModel.Any("serviceId"))
            //    {
            //        dapperExecution.SqlBuilder.AppendPredicate<int>("ics.ServiceId",
            //            requestFilterModel.GetProperty("serviceId"));
            //    }
            //}

            //if (requestFilterModel.Any("targetFullName"))
            //{
            //    dapperExecution.SqlBuilder.AppendPredicate<int>("vt.TargetFullName",
            //        requestFilterModel.GetProperty("targetFullName"));
            //}

            return dapperExecution.ExecutePaginateQuery();
        }
        public IPagedList<PaymentVoucherReportGridDTO> GetReportServiceDebtExcelData(PaymentVoucherReportFilter requestFilterModel)
        {
            var pr = new DynamicParameters();           
            pr.Add("_skip", requestFilterModel.Skip);
            pr.Add("_take", requestFilterModel.Take);
            pr.Add("_orderby", requestFilterModel.OrderBy);
            pr.Add("_dir", requestFilterModel.Dir);
            pr.Add("_total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var dateFilter = new DateTime(requestFilterModel.Year, requestFilterModel.Month, 1);
            pr.Add("_startDate", dateFilter);

            var sqlStored = "ReportPaymentVoucher";
            if(requestFilterModel.TypeId == 1)
            {
                sqlStored = "ReportPaymentVoucher";
            }  
            if(requestFilterModel.TypeId == 2 || requestFilterModel.TypeId == 3)
            {
                sqlStored = "ReportPaymentVoucherSharing";
            }
            var subSetResult = WithConnection(conn =>
                    conn.Query<PaymentVoucherReportGridDTO>(
                        sqlStored, 
                        pr,
                        commandType: CommandType.StoredProcedure
                        )
                    ).Distinct().ToList();

            var totalRecords = requestFilterModel.Paging ? pr.Get<int>("_total") : subSetResult.Count();
            var result = new PagedList<PaymentVoucherReportGridDTO>(requestFilterModel.Skip, requestFilterModel.Take, totalRecords)
            {
                Subset = subSetResult
            };

            return result;
        }
        public IPagedList<PaymentVoucherReportGridDTO> GetReportServiceDebtData(PaymentVoucherReportFilter requestFilterModel)
        {
            var pr = new DynamicParameters();
            pr.Add("skip", requestFilterModel.Paging ? requestFilterModel.Skip : 0);
            pr.Add("take", requestFilterModel.Paging ? requestFilterModel.Take : int.MaxValue);

            pr.Add("orderBy", "ORDER BY " + requestFilterModel.OrderBy.ToUpperFirstLetter() + " " + requestFilterModel.Dir);
            pr.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            pr.Add("marketAreaId", requestFilterModel.MarketAreaId);
            pr.Add("contractCode", requestFilterModel.ContractCode ?? "");
            pr.Add("customerCode", requestFilterModel.CustomerCode ?? "");
            pr.Add("contractorFullName", requestFilterModel.ContractorFullName ?? "");
            pr.Add("startDate", requestFilterModel.TimelineSignedStart, DbType.Date);
            pr.Add("endDate", requestFilterModel.TimelineSignedEnd,DbType.Date);

            pr.Add("serviceId", requestFilterModel.ServiceId);           
            pr.Add("projectId", requestFilterModel.ProjectId);           
            pr.Add("agentId", requestFilterModel.AgentId ?? "");           
            
            pr.Add("isEnterprise", requestFilterModel.IsEnterprise);
            pr.Add("status", requestFilterModel.Status);

            
            string sql = "";
            if(requestFilterModel.ReportType == 1)
            {
                pr.Add("contractType", requestFilterModel.ContractType);
                sql = "sp_GetReportServiceDebtData";
            }
            else
            {
                sql = "sp_GetReportServiceOutDebtData";
            }


            var subSetResult = WithConnection(conn =>
                    conn.Query<PaymentVoucherReportGridDTO>(
                        sql, 
                        pr,
                        commandType: CommandType.StoredProcedure
                        )
                    ).Distinct().ToList();

            var totalRecords =  requestFilterModel.Paging ? pr.Get<int>("total") : subSetResult.Count();
            var result = new PagedList<PaymentVoucherReportGridDTO>(requestFilterModel.Skip, requestFilterModel.Take, totalRecords)
            {
                Subset = subSetResult.ToList(),
                TotalItemCount = totalRecords
            };

            return result;
        }
    }
}
