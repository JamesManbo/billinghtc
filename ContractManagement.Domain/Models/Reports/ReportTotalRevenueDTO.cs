using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class ReportTotalRevenueDTO
    {
		public string ContractorName {get;set;}
		public string CategoryName { get;set;}
		public string TransactionCode { get;set;}
		public string ContractCode {get;set;}
		public DateTime TimeLineSigned {get;set;}
		public string ServiceName {get;set;}
		public DateTime TimeLineExpiration {get;set;}
		public DateTime? TimeLineEffective { get;set;}
		public decimal TotalRevenue { get;set;}
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string ContractorCategoryName { get; set; }
        public int Total { get; set; }
	}

    public class CommissionAndSharingReportDTO
    {
        public string InContractId { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractCode { get; set; }
        public string ContractType { get; set; }
        public int PaymentPeriod { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string MarketAreaName { get; set; }
        public decimal ContractValue { get; set; }
        public decimal DebtValue { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal OpeningDebtAmount { get; set; }
        public decimal TongLuyKeThang { get; set; }
        public decimal PaidTotal { get; set; }
        public string Content { get; set; }
        public string ServiceName { get; set; }
        public string ServiceGroupName { get; set; }
        public string StartPoint { get; set; }
        public string StatusName { get; set; }
        public string EndPoint { get; set; }
        public string TargetFullName { get; set; }
        public string CurrencyUnitCode { get; set; }
        public decimal GrandTotalIncludeDebt { get; set; }
        public string IndustryName { get; set; }

        public decimal ReductionFee { get; set; }
    }
}
