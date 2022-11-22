using AutoMapper.Configuration;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.ExportInvoiceFilesModels;
using DebtManagement.Domain.Models.FilterModels;
using GenericRepository;
using GenericRepository.Extensions;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IExportInvoiceFileQueries : IQueryRepository
    {
        IPagedList<ExportInvoiceFileModel> GetList(ExportInvoiceFilterModel requestFilterModel);
        CountReceiptVoucher GetCountReceiptVouchers(ExportInvoiceFilterModel filter);
        IPagedList<ExportInvoiceFileModel> GetInvoiceReportData(ExportInvoiceFilterModel filter);
    }
    public class ExportInvoiceFileQueries : QueryRepository<ReceiptVoucher, int>, IExportInvoiceFileQueries
    {

        public ExportInvoiceFileQueries(DebtDbContext context) : base(context)
        {

        }
        public IPagedList<ExportInvoiceFileModel> GetList(ExportInvoiceFilterModel filter)
        {
            var cached = new Dictionary<int, ExportInvoiceFileModel>();

            var dapperExecution = BuildByTemplateWithoutSelect<ExportInvoiceFileModel>(filter);
            dapperExecution.SqlBuilder.Select("t1.Id AS Id");
            dapperExecution.SqlBuilder.Select("t1.ContractCode");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode");
            dapperExecution.SqlBuilder.Select("t1.StatusId");
            dapperExecution.SqlBuilder.Select("t1.IsEnterprise");

            dapperExecution.SqlBuilder.Select("t2.MarketAreaName");
            dapperExecution.SqlBuilder.Select("t2.TimeLine_PaymentPeriod as TimeLinePaymentPeriod");

            dapperExecution.SqlBuilder.Select("t1.PaidTotal * t1.ExchangeRate AS PaidTotal");
            //dapperExecution.SqlBuilder.Select("t1.PaymentDate");
            dapperExecution.SqlBuilder.Select("CASE WHEN vch.IsEnterprise = 1 THEN DATE_ADD(InvoiceReceivedDate, INTERVAL t1.NumberBillingLimitDays DAY) ELSE DATE_ADD( t1.IssuedDate, INTERVAL t1.NumberBillingLimitDays DAY) END AS PaymentDate");
            dapperExecution.SqlBuilder.Select("0 as LuyKeThang");
            dapperExecution.SqlBuilder.Select("t1.VoucherCode");
            dapperExecution.SqlBuilder.Select("t1.InvoiceCode");
            dapperExecution.SqlBuilder.Select("t1.InvoiceDate");
            //dapperExecution.SqlBuilder.Select("t1.InvoiceReceivedDate");
            dapperExecution.SqlBuilder.Select("t1.InvoiceReceivedDate AS InvoiceReceivedDate");
            dapperExecution.SqlBuilder.Select("t1.Content");
            dapperExecution.SqlBuilder.Select("t1.Description");

            //
            dapperExecution.SqlBuilder.Select("t1.GrandTotal");
            dapperExecution.SqlBuilder.Select("t1.ClearingTotal");
            dapperExecution.SqlBuilder.Select("t1.RemainingTotal");

            dapperExecution.SqlBuilder.Select("t1.NumberBillingLimitDays");
            dapperExecution.SqlBuilder.Select("CASE WHEN vch.IsEnterprise = 1 THEN DATE_ADD(InvoiceReceivedDate, INTERVAL t1.NumberBillingLimitDays DAY) ELSE DATE_ADD( t1.IssuedDate, INTERVAL t1.NumberBillingLimitDays DAY) END AS DateBillingLimit");
            dapperExecution.SqlBuilder.Select("t1.NumberDaysOverdue");
            dapperExecution.SqlBuilder.Select("t1.TargetDebtRemaningTotal");
            dapperExecution.SqlBuilder.Select("t1.Description");
            
            dapperExecution.SqlBuilder.Select("NOW() AS Today");
            dapperExecution.SqlBuilder.Select("CancellationReason AS Reason");

            //dapperExecution.SqlBuilder.Select("us.CustomerCode as CustomerCode");
            //dapperExecution.SqlBuilder.Select("us.FullName");
            dapperExecution.SqlBuilder.Select("vch.TargetCode AS CustomerCode");
            dapperExecution.SqlBuilder.Select("vch.TargetFullName AS FullName");


            dapperExecution.SqlBuilder.Select("vtp.`CategoryName` AS CategoryName");
            dapperExecution.SqlBuilder.Select("t6.Name AS StatusName");

            dapperExecution.SqlBuilder.Select("t5.GroupName as ServiceGroupName");

            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.OutContracts AS t2 ON t1.OutContractId = t2.Id");
            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets AS vch ON vch.Id = t1.TargetId");
            //dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_CRM.ApplicationUsers AS us ON us.IdentityGuid = vch.ApplicationUserIdentityGuid");
            dapperExecution.SqlBuilder.LeftJoin("VoucherTargetProperties  AS vtp ON vtp.TargetId = vch.Id");
            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDetails AS t3 ON t3.ReceiptVoucherId  = t1.Id");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.Services AS t4 ON t4.Id  = t3.ServiceId");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.ServiceGroups AS t5 ON t5.Id  = t4.GroupId");
            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherStatuses AS t6 ON t6.Id  = t1.StatusId");

            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("IFNULL(t1.InvoiceCode,'') <> ''");

            if(filter.MarketAreaId > 0)
            {
                dapperExecution.SqlBuilder.Where("( @marketAreaId = 0 OR t1.MarketAreaId = @marketAreaId )", new { marketAreaId = filter.MarketAreaId });
            }

            if (filter.TimelineSignedStart.HasValue && filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) BETWEEN DATE(@startDate) AND DATE(@endDate)",
                    new
                    {
                        startDate = filter.TimelineSignedStart.Value.Date,
                        endDate = filter.TimelineSignedEnd.Value.Date
                    });
            }
            else if (filter.TimelineSignedStart.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) >= @startDate",
                   new
                   {
                       startDate = filter.TimelineSignedStart.Value.Date
                   });
            }
            else if (filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) <= @endDate",
                   new
                   {
                       endDate = filter.TimelineSignedStart.Value.Date
                   });
            }

            if (filter.ServiceGroupId > 0)
            {
                dapperExecution.SqlBuilder.Where("t5.Id = @serviceGroupId", new { serviceGroupId = filter.ServiceGroupId });
            }
            if (filter.CustomerCategoryId > 0)
            {
                dapperExecution.SqlBuilder.Where("vtp.CategoryId = @customerCategoryId", new { customerCategoryId = filter.CustomerCategoryId });
            }
            if (filter.ServiceId > 0)
            {
                dapperExecution.SqlBuilder.Where("t4.Id = @serviceId", new { serviceId = filter.ServiceId });
            }
            if (filter.ProjectId > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.ProjectId = @projectId", new { projectId = filter.ProjectId });
            }
            if (filter.VoucherStatusId > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId = @statusId", new { statusId = filter.VoucherStatusId });
            }
            if (filter.IsEnterprise < 2)
            {
                dapperExecution.SqlBuilder.Where("vch.IsEnterprise = @isEnterprise", new { isEnterprise = filter.IsEnterprise });
            }
            if (!string.IsNullOrEmpty(filter.ContractCode))
            {
                dapperExecution.SqlBuilder.Where("t1.ContractCode LIKE @contractCode ", new { contractCode = $"%{filter.ContractCode}%" });
            }
            if (!string.IsNullOrEmpty(filter.CustomerCode))
            {
                dapperExecution.SqlBuilder.Where("vch.TargetCode LIKE @customerCode ", new { customerCode = $"%{filter.CustomerCode}%" });
            }

            if (!string.IsNullOrEmpty(filter.ContractorFullName))
            {
                dapperExecution.SqlBuilder.Where("vch.TargetFullName LIKE @contractorFullName ", new { contractorFullName = $"%{filter.ContractorFullName}%" });
            }

            var queryResult = dapperExecution
                .ExecutePaginateQuery<ExportInvoiceFileModel, ReceiptVoucherLines>(
                    (receiptVoucher, receiptVoucherDetail) =>
                    {
                        if (!cached.TryGetValue(receiptVoucher.Id, out var voucherEntry))
                        {
                            voucherEntry = receiptVoucher;
                            
                            cached.Add(voucherEntry.Id, voucherEntry);
                        }

                        if (receiptVoucherDetail != null
                            )
                        {
                            voucherEntry.ReceiptVoucherLines.Add(receiptVoucherDetail);
                        }

                        return voucherEntry;
                    }, "StatusName");

            return queryResult;
            //return dapperExecution.ExecutePaginateQuery();
        }

        public IPagedList<ExportInvoiceFileModel> GetInvoiceReportData(ExportInvoiceFilterModel filter)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<ExportInvoiceFileModel>(filter);
            dapperExecution.SqlBuilder.Select("t1.ContractCode");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode");

            dapperExecution.SqlBuilder.Select("t5.GroupName as ServiceGroupName");
            dapperExecution.SqlBuilder.Select("t2.MarketAreaName");
            dapperExecution.SqlBuilder.Select("t2.TimeLine_PaymentPeriod as TimeLinePaymentPeriod");

            dapperExecution.SqlBuilder.Select("t1.PaidTotal * t1.ExchangeRate AS PaidTotal");
            dapperExecution.SqlBuilder.Select("t1.PaymentDate");
            dapperExecution.SqlBuilder.Select("t1.TargetDebtRemaningTotal + t1.GrandTotal - t1.PaidTotal as LuyKeThang");
            dapperExecution.SqlBuilder.Select("t1.VoucherCode");
            dapperExecution.SqlBuilder.Select("t1.InvoiceCode");
            dapperExecution.SqlBuilder.Select("t1.InvoiceDate");
            dapperExecution.SqlBuilder.Select("t1.InvoiceReceivedDate");
            dapperExecution.SqlBuilder.Select("t1.Content");
            dapperExecution.SqlBuilder.Select("t1.Description");

            //dapperExecution.SqlBuilder.Select("us.CustomerCode as CustomerCode");
            //dapperExecution.SqlBuilder.Select("us.FullName");
            dapperExecution.SqlBuilder.Select("vch.TargetCode AS CustomerCode");
            dapperExecution.SqlBuilder.Select("vch.TargetFullName AS FullName");

            dapperExecution.SqlBuilder.Select("vtp.`CategoryName` AS CategoryName");
            dapperExecution.SqlBuilder.Select("t6.Name AS StatusName");

            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.OutContracts AS t2 ON t1.OutContractId = t2.Id");
            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets AS vch ON vch.Id = t1.TargetId");
            //dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_CRM.ApplicationUsers AS us ON us.IdentityGuid = vch.ApplicationUserIdentityGuid");
            dapperExecution.SqlBuilder.LeftJoin("VoucherTargetProperties  AS vtp ON vtp.TargetId = vch.Id");
            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDetails AS t3 ON t3.ReceiptVoucherId  = t1.Id");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.Services AS t4 ON t4.Id  = t3.ServiceId");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.ServiceGroups AS t5 ON t5.Id  = t4.GroupId");
            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherStatuses AS t6 ON t6.Id  = t1.StatusId");

            dapperExecution.SqlBuilder.Where("(@marketAreaId = 0 OR t1.MarketAreaId = @marketAreaId )", new { marketAreaId = filter.MarketAreaId });
            dapperExecution.SqlBuilder.Where("IFNULL(t1.InvoiceCode,'') <> ''");
            if (filter.TimelineSignedStart.HasValue && filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) BETWEEN DATE(@startDate) AND DATE(@endDate)", new
                {
                    startDate = filter.TimelineSignedStart.Value.Date,
                    endDate = filter.TimelineSignedEnd.Value.Date
                });
            }
            else if (filter.TimelineSignedStart.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) >= @startDate", new
                {
                    startDate = filter.TimelineSignedStart.Value.Date
                });
            }
            else if (filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) <= @endDate", new
                {
                    startDate = filter.TimelineSignedEnd.Value.Date
                });
            }
            if (filter.ServiceGroupId > 0)
            {
                dapperExecution.SqlBuilder.Where("t5.Id = @serviceGroupId", new { serviceGroupId = filter.ServiceGroupId });
            }
            if (filter.ServiceId > 0)
            {
                dapperExecution.SqlBuilder.Where("t4.ServiceId = @serviceId", new { serviceId = filter.ServiceId });
            }
            if (filter.ProjectId > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @projectId", new { projectId = filter.ProjectId });
            }
            if (filter.IsEnterprise < 2)
            {
                dapperExecution.SqlBuilder.Where("vch.IsEnterprise = @isEnterprise", new { isEnterprise = filter.IsEnterprise });
            }
            if (!string.IsNullOrEmpty(filter.ContractCode))
            {
                dapperExecution.SqlBuilder.Where("t1.ContractCode LIKE @contractCode ", new { contractCode = $"%{filter.ContractCode}%" });
            }
            if (!string.IsNullOrEmpty(filter.CustomerCode))
            {
                dapperExecution.SqlBuilder.Where("vch.TargetCode LIKE @customerCode ", new { customerCode = $"%{filter.CustomerCode}%" });
            }

            dapperExecution.SqlBuilder.Skip(filter.Skip);
            dapperExecution.SqlBuilder.Take(filter.Take);

            return dapperExecution.ExecutePaginateQuery();
        }


        public CountReceiptVoucher GetCountReceiptVouchers(ExportInvoiceFilterModel filter)
        {
            filter.OrderBy = "";
            var dapperExecution = BuildByTemplateWithoutSelect<CountReceiptVoucher>(filter);
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 0, 1, 0)) AS TotalPersonal");
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 0 AND t1.StatusId <> 4, 1, 0)) AS TotalPersonalUnCollected");
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 0 AND t1.StatusId = 4, 1, 0)) AS TotalPersonalCollected");

            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 1 AND t1.StatusId <> 4, 1, 0)) AS TotalEnterpriseUnCollected");
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 1 AND t1.StatusId = 4, 1, 0)) AS TotalEnterpriseCollected");
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 1 , 1, 0)) AS TotalEnterprise");

            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 1 AND IFNULL(t1.InvoiceCode,'') <> '', 1, 0)) AS TotalEnterpriseExportInVoice");
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.IsEnterprise = 1 AND IFNULL(t1.InvoiceCode,'') = '', 1, 0)) AS TotalEnterpriseUnExportInvoice");
            dapperExecution.SqlBuilder.Where("t1.StatusId <> 5");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("IFNULL(t1.InvoiceCode,'') <> ''");

            return dapperExecution.ExecuteScalarQuery();
        }

        private void GenerateWhereClause(string item, ref string sqlWhere)
        {
            string[] filter = item.Split("::");
            string sColumnName = filter[0].ToUpperFirstLetter();
            string sTableName = "t1";
            string sOperatorFirst = "";
            string sOperatorLast = "'";

            string value = filter[1];
            if (sColumnName.Contains("Date"))
                value = DateTime.Parse(value).ToString("yyyy-MM-dd");
            switch (sColumnName)
            {
                case "CustomerCode":
                    sTableName = "us";
                    break;
                case "CategoryName":
                    sColumnName = "Name";
                    sTableName = "ctg";
                    break;
                case "FullName":
                    sTableName = "us";
                    break;
                case "MarketAreaName":
                    sTableName = "t2";
                    break;
                case "ServiceGroupName":
                    sColumnName = "GroupName";
                    sTableName = "t5";
                    break;
                case "TimeLinePaymentPeriod":
                    sColumnName = "TimeLine_PaymentPeriod";
                    sTableName = "t2";
                    break;
                default:
                    break;
            }
            switch (filter[2])
            {
                case "contains":
                    sOperatorFirst = " LIKE '%";
                    sOperatorLast = "%'";
                    break;
                case "doesnotcontain":
                    sOperatorFirst = "NOT LIKE '%";
                    sOperatorLast = "%'";
                    break;
                case "gte":
                    sOperatorFirst = " >= '";
                    break;
                case "lte":
                    sOperatorFirst = " <= '";
                    sOperatorLast = "'";
                    break;
                case "eq":
                    sOperatorFirst = " = '";
                    break;
                default:
                    sOperatorFirst = " = '";
                    break;
            }
            sqlWhere = sTableName + "." + sColumnName + sOperatorFirst + value + sOperatorLast;
        }
    }
}
