using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherGridDTO : BaseDTO
    {
        private decimal remainingTotal;
        public ReceiptVoucherGridDTO()
        {
            ReceiptLines = new List<ReceiptVoucherDetailGridDTO>();
        }

        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public string MarketAreaName { get; set; }
        public string ProjectName { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public string VoucherCode { get; set; }
        public int OutContractId { get; set; }
        public string ContractCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StatusName { get; set; }
        public string StatusNameApp { get; set; }
        public string InvoiceCode { get; set; }
        public string Content { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal ReductionFreeTotal { get; set; }
        public decimal CashTotal { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal RemainingTotal { get; set; }
        public string PayerName { get; set; }
        public decimal DiscountAmount { get; set; }
        public string TargetFullName { get; set; }
        public string TargetPhone { get; set; }
        public string TargetCode { get; set; }
        public string TargetAddress { get; set; }
        public decimal InstallationFee { get; set; }
        public bool IsEnterprise { get; set; }
        public string IsEnterpriseName { get; set; }
        public PaymentMethod Payment { get; set; }
        public List<ReceiptVoucherDetailGridDTO> ReceiptLines { get; set; }

        public string AccountingCode { get; set; }
        public string CreatedBy { get; set; }
        public string CashierUserName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public string CancellationReason { get; set; }
        public int NumberDaysBadDebt { get; set; }
        public int NumberBillingLimitDays { get; set; }
        //public bool? IsBadDebt { get; set; }
        //public bool IsOverdue => IsBadDebt == true ? false : ReceiptVoucherStatus.UnpaidStatuses().Contains(StatusId) 
        //    && ((!IsEnterprise && IssuedDate.AddDays(NumberBillingLimitDays + 1) <= DateTime.Now) 
        //    || (IsEnterprise && InvoiceDate.HasValue && InvoiceDate.Value.AddDays((NumberBillingLimitDays > 30 ? NumberBillingLimitDays : 30 ) + 1) <= DateTime.Now));
        public int NumberDaysOverdue { get; set; }
        public string ReductionReasons { get; set; }
        public string ReductionFees { get; set; }
        private int _feedbackCount = 0;
        public int FeedbackCount {
            get
            {
                if(StatusId == ReceiptVoucherStatus.Pending.Id)
                {
                    return _feedbackCount;
                }
                else
                {
                    var reductionIds = string.Join("##", ReceiptLines.Select(r => r.ReductionReasonIds));
                    if (string.IsNullOrEmpty(reductionIds)) return 0;

                    return reductionIds.Split("##").Length;
                }
            }
            set
            {
                _feedbackCount = value;
            }
        }
    }

    public class ReceiptVoucherDetailGridDTO : BaseDTO
    {
        public string CId { get; set; }
        public int? PaymentVoucherId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public DateTime StartBillingDate { get; set; }
        public DateTime EndBillingDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal DiscountAmountSuspend { get; set; }
        public string ReductionReasonIds { get; set; }
    }
}
