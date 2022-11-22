using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.PaymentVoucherModels
{
    public class PaymentVoucherReportGridDTO
    {
        public string InContractId { get; set; }
        public string ContractorFullName { get; set; }
        public string CategoryName { get; set; }
        public string ProjectName { get; set; }
        public string ContractCode { get; set; }
        public string ContractType { get; set; }
        public int PaymentPeriod { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ClearingDate { get; set; }
        public decimal ClearingTotal { get; set; }
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
        public string IndustryNames { get; set; }

        public decimal ReductionFee { get; set; }

        public string SignedUserName { get; set; }
        public string OrganizationUnitName { get; set; }
        public string CustomerCareStaffUserName { get; set; }
    }
}
