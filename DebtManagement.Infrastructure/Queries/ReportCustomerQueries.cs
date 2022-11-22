using Dapper;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.ReportModels;
using GenericRepository;
using GenericRepository.Core;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IReportCustomerQueries : IQueryRepository
    {
        IPagedList<ReportCustomerDto> GetList(RequestFilterModel filterModel);
        PagedList<ContractInfoForReport> GetReportCustomer(ReportCustomerFillter reportCustomerFillter);
    }
    public class ReportCustomerQueries : QueryRepository<ReceiptVoucher, int>, IReportCustomerQueries
    {
        public ReportCustomerQueries(DebtDbContext context) : base(context)
        {
        }

        public IPagedList<ReportCustomerDto> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<ReportCustomerDto>(filterModel);

            dapperExecution.SqlBuilder.Select("t1.Content");

            dapperExecution.SqlBuilder.Select("us.UserName");
            dapperExecution.SqlBuilder.Select("us.TaxIdNo");
            dapperExecution.SqlBuilder.Select("us.CustomerCode");
            dapperExecution.SqlBuilder.Select("us.Email");

            dapperExecution.SqlBuilder.Select("ct.ContractCode");
            dapperExecution.SqlBuilder.Select("ct.MarketAreaName as region");
            dapperExecution.SqlBuilder.Select("ct.ProjectName");
            dapperExecution.SqlBuilder.Select("CASE ct.IsActive WHEN 0 THEN 'Thanh ly' ELSE 'Dang hoat dong' END AS 'TinhTrang' ");

            dapperExecution.SqlBuilder.Select("class.ClassName");

            dapperExecution.SqlBuilder.Select("csp.Floor");
            dapperExecution.SqlBuilder.Select("csp.Building");
            dapperExecution.SqlBuilder.Select("csp.RoomNumber");

            dapperExecution.SqlBuilder.Select("csp.PackagePrice ");
            dapperExecution.SqlBuilder.Select("csp.ServiceId ");
            dapperExecution.SqlBuilder.Select("csp.ServiceName ");
            dapperExecution.SqlBuilder.Select("csp.BandwidthLabel ");
            dapperExecution.SqlBuilder.Select("csp.InstallationFee ");
            dapperExecution.SqlBuilder.Select("csp.TimeLine_Signed              AS TimeLineSigned             ");                  // ngày ký hợp đồng
            dapperExecution.SqlBuilder.Select("ct.TimeLine_PaymentPeriod        AS TimeLinePaymentPeriod      ");             // ,ngày tính cước
            dapperExecution.SqlBuilder.Select("csp.TimeLine_SuspensionStartDate	AS TimeLineSuspensionStartDate");
            dapperExecution.SqlBuilder.Select("csp.TimeLine_SuspensionEndDate   AS TimeLineSuspensionEndDate  ");
            dapperExecution.SqlBuilder.Select("csp.TimeLine_LatestBilling       AS TimeLineLatestBilling      ");            // ngày kết thúc cước
            dapperExecution.SqlBuilder.Select("csp.TimeLine_NextBilling         AS TimeLineNextBilling        ");              //ngày kế tiếp

            dapperExecution.SqlBuilder.Select("ce.EquipmentName");
            dapperExecution.SqlBuilder.Select("ce.ExaminedUnit ");
            dapperExecution.SqlBuilder.Select("ce.SerialCode   ");

            dapperExecution.SqlBuilder.LeftJoin("ITC_FBM_Contracts.OutContracts AS ct ON t1.OutContractId = ct.Id");
            dapperExecution.SqlBuilder.LeftJoin("VoucherTargets as vch ON vch.Id = t1.TargetId	");
            dapperExecution.SqlBuilder.LeftJoin("itc_fbm_crm.applicationusers as us ON us.IdentityGuid = vch.Id");
            dapperExecution.SqlBuilder.LeftJoin("itc_fbm_crm.applicationuserclasses as class ON us.ClassId = class.Id");
            dapperExecution.SqlBuilder.LeftJoin("ITC_FBM_Contracts.OutContractServicePackages AS csp ON csp.OutContractId = ct.Id");
            dapperExecution.SqlBuilder.LeftJoin("ITC_FBM_Contracts.ContractEquipments AS ce ON ce.OutContractPackageId = csp.Id");

            return dapperExecution.ExecutePaginateQuery();
         
        }

        public PagedList<ContractInfoForReport> GetReportCustomer(ReportCustomerFillter reportCustomerFillter)
        {
            var p = new DynamicParameters();          
            StringBuilder filters = new StringBuilder();
            filters.Append(" ");
            
            foreach (var filterModel in reportCustomerFillter.PropertyFilterModels)
            {
                switch (filterModel.Operator)
                {
                    case "contains":
                        filters.Append(" AND ");
                        filters.Append(filterModel.Field);
                        filters.Append(" LIKE '%");
                        filters.Append(filterModel.FilterValue);
                        filters.Append("%' ");
                        break;
                    case "gte":
                        filters.Append(" AND ");
                        filters.Append(filterModel.Field);
                        filters.Append(" > ");
                        filters.Append(filterModel.FilterValue);
                        break;
                    case "lte":
                        filters.Append(" AND ");
                        filters.Append(filterModel.Field);
                        filters.Append(" < ");
                        filters.Append(filterModel.FilterValue);
                        break;
                    case "eq":
                        filters.Append(" AND ");
                        filters.Append(filterModel.Field);
                        filters.Append(" = ");
                        filters.Append(filterModel.FilterValue);
                        break;
                    default:
                        break;
                }
               
            }   
            p.Add("orderBys", reportCustomerFillter.OrderBy +" " + reportCustomerFillter.Dir);
            p.Add("dir", reportCustomerFillter.Dir);
            p.Add("skips", reportCustomerFillter.Paging == true ? reportCustomerFillter.Skip : 0);
            p.Add("take", reportCustomerFillter.Paging == true ? reportCustomerFillter.Take : int.MaxValue);
            p.Add("filters", filters.ToString());
            var reportDatas = WithConnection((conn) => conn.Query<ReportCustomerDto>("sp_GetReportOutContract", p, commandType: CommandType.StoredProcedure));
            int lastContractId = 0;
            int lastServicePackageId = 0;
            string lastVoucherId = "";
            var lstContractInfos = new List<ContractInfoForReport>();
            var contractInfo = new ContractInfoForReport();
            var service = new ServicePackage();
            var receiptVoucher = new ReceiptVoucherForReport();
            var equipment = new EquipmentForReport();
            for (int i = 0; i < reportDatas.Count(); i++)
            {
                var reportData = reportDatas.ElementAt(i);

                if (lastContractId == 0 || lastContractId != reportData.ContractId)
                {
                    if (lastContractId != 0)
                    {
                        contractInfo.servicePackages.Add(service);
                        lstContractInfos.Add(contractInfo);
                        lastServicePackageId = 0;
                    }
                    lastContractId = reportData.ContractId;

                    contractInfo = new ContractInfoForReport();
                    contractInfo.ContractId = reportData.ContractId;
                    contractInfo.ContractAdd = reportData.ContractAdd;
                    contractInfo.ContractCode = reportData.ContractCode;
                    contractInfo.CustomerClass = reportData.CustomerClass;
                    contractInfo.CustomerCode = reportData.CustomerCode;
                    contractInfo.CustomerName = reportData.CustomerName;
                    contractInfo.Email = reportData.Email;
                    contractInfo.InvoiceAdd = reportData.InvoiceAdd;
                    contractInfo.isActive = reportData.isActive;
                    contractInfo.MarketAreaName = reportData.MarketAreaName;
                    contractInfo.PaymentAdd = reportData.PaymentAdd;
                    contractInfo.PaymentDate = reportData.PaymentDate;
                    contractInfo.PhoneNo = reportData.PhoneNo;
                    contractInfo.ProjectName = reportData.ProjectName;
                    contractInfo.TaxIdNo = reportData.TaxIdNo;
                    contractInfo.TimeLineExpiration = reportData.TimeLineExpiration;
                    contractInfo.TimeLineSigned = reportData.TimeLineSigned;
                    contractInfo.RemainingTotal = reportData.RemainingTotal;

                }

                if (lastServicePackageId == 0 || lastServicePackageId != reportData.ServicePackageId)
                {
                    if(lastServicePackageId != 0 )
                    {
                        if (!string.IsNullOrEmpty(service.ServiceName))
                        {
                            contractInfo.servicePackages.Add(service);
                        }
                    }
                    lastServicePackageId = reportData.ServicePackageId;
                    service = new ServicePackage();
                    service.BandwithLabel = reportData.BandwithLabel;
                    service.Building = reportData.Building;
                    service.Floor = reportData.Floor;
                    service.InstallationFee = reportData.InstallationFee;
                    service.InternationalBandwidth = reportData.InternationalBandwidth;
                    service.DomesticBandwidth = reportData.DomesticBandwidth;
                    service.PackagePrice = reportData.PackagePrice;
                    service.PromotionDateQuantity = reportData.PromotionDateQuantity;
                    service.RoomNo = reportData.RoomNo;
                    service.ServiceName = reportData.ServiceName;
                    service.ServicePackageName = reportData.ServicePackageName;
                    service.TimeLineEffective = reportData.TimeLineEffective;
                    service.TimeLineLatestBilling = reportData.TimeLineLatestBilling;
                    service.TimeLineNextBilling = reportData.TimeLineNextBilling;
                    if (service.TimeLineNextBilling != null)
                    {
                        service.TimeLineNextDay = service.TimeLineNextBilling.Value.AddDays(1);
                    }
                    service.TimeLinePaymentPeriod = reportData.TimeLinePaymentPeriod;
                    service.TimeLinePrepayPeriod = reportData.TimeLinePrepayPeriod;
                    service.TimeLineRenewPeriod = reportData.TimeLineRenewPeriod;
                    service.TotalMonthUse = reportData.TotalMonthUse;
                    service.InstallationAddress.Street = reportData.InstallationAddressStreet;
                    service.HasStartAndEndPoint = reportData.HasStartAndEndPoint;
                }

                
                if (!string.IsNullOrEmpty(reportData.EquipmentName))
                {
                    equipment = new EquipmentForReport();
                    equipment.EquipmentName = reportData.EquipmentName;
                    equipment.EquipmentSerial = reportData.EquipmentSerial;
                    equipment.EquipmentQuantity = reportData.EquipmentQuantity;

                    var equip = new EquipmentForReport();
                    bool isExist = false;
                    if (service.Equipments.Count == 0)
                    {
                        service.Equipments.Add(equipment);
                    }
                    service.Equipments.ForEach(e=> {
                        if(e.EquipmentName == reportData.EquipmentName)
                        {
                            isExist = true;
                            return;
                        }
                    });
                    if (!isExist)
                    {
                        service.Equipments.Add(equipment);
                    }
                    
                }

                if (string.IsNullOrEmpty(lastVoucherId) && !string.Equals(lastVoucherId, reportData.ReceiptVoucherId) && reportData.ReceiptVoucherId != null)
                {
                    if (string.IsNullOrEmpty(lastVoucherId))
                    {
                        if (!string.IsNullOrEmpty(receiptVoucher.Content))
                        {
                            contractInfo.receiptVouchers.Add(receiptVoucher);
                        }
                    }
                    lastVoucherId = reportData.ReceiptVoucherId;
                    receiptVoucher = new ReceiptVoucherForReport();
                    receiptVoucher.Content = reportData.Content;
                    receiptVoucher.IssuedDate = reportData.IssuedDate;
                    receiptVoucher.GrandTotal = reportData.GrandTotal;
                    receiptVoucher.PaidTotal = reportData.PaidTotal;
                    receiptVoucher.PaymentDate = reportData.PaymentDate.Equals(DateTime.MinValue)? "" : reportData.PaymentDate.ToString();                   
                }
              
               
            }
            if (!string.IsNullOrEmpty(service.ServiceName))
            {
                contractInfo.servicePackages.Add(service);
            }

            if (!string.IsNullOrEmpty(receiptVoucher.Content))
            {
                contractInfo.receiptVouchers.Add(receiptVoucher);
            }

           
            lstContractInfos.Add(contractInfo);
            var con = lstContractInfos.Where(e => e.ContractId == 0).FirstOrDefault();
            lstContractInfos.Remove(con);
            var page = new PagedList<ContractInfoForReport>(reportCustomerFillter.Skip, reportCustomerFillter.Take, reportDatas.Count());
            //  var total = equipments.Count() > 0 ? equipments.First().Total : 0;
            page.Subset = lstContractInfos.ToList();

            return page;
        }
    }
}
