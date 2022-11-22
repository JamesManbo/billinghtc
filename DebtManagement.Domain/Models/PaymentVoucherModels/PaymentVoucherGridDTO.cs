using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Models.ReceiptVoucherModels;

namespace DebtManagement.Domain.Models.PaymentVoucherModels
{
    public class PaymentVoucherGridDTO
    {
        public PaymentVoucherGridDTO()
        {
            PaymentVoucherDetails = new List<PaymentVoucherDetailGridDTO>();
        }

        public string Id { get; set; }

        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string MarketAreaName { get; set; }
        public string ProjectName { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public string VoucherCode { get; set; }
        public int InContractId { get; set; }
        public string ContractCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public string StatusName { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public decimal ReductionFreeTotal { get; set; }
        public decimal CashTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal RemainingTotal { get; set; }
        public decimal ClearingTotal { get; set; }
        public string PayerName { get; set; }
        public decimal DiscountAmount { get; set; }
        public string TargetFullName { get; set; }
        public bool IsEnterprise { get; set; }
        public PaymentMethod Payment { get; set; }

        public string AccountingCode { get; set; }
        public string CreatedBy { get; set; }
        public string CashierUserName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public List<PaymentVoucherDetailGridDTO> PaymentVoucherDetails { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public bool IsOverdue => PaymentVoucherStatus.New.Id == StatusId
            && InvoiceDate.HasValue && InvoiceDate.Value.AddDays(NumberBillingLimitDays + 1) <= DateTime.Now;
        public int NumberDaysOverdue => (InvoiceDate.HasValue ? DateTime.Now - InvoiceDate.Value.AddDays(NumberBillingLimitDays + 1) : TimeSpan.Zero).Days;

        public string CashierUserId { get; set; } //nhân viên thanh toán
    }

    public class PaymentVoucherDetailGridDTO : BaseDTO
    {
        public string PaymentVoucherId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public string ServicePackageName { get; set; }
        public string ServicePackageId { get; set; }
        public DateTime StartBillingDate { get; set; }
        public DateTime EndBillingDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public decimal GrandTotal { get; set; }
    }


}
