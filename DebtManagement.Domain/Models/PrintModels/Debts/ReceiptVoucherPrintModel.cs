using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.PrintModels.Debts
{
    public class ReceiptVoucherPrintModel
    {
        public ReceiptVoucherPrintModel()
        {
            DebtsByService = new List<DebtPrintModel>();
            ReceiptLines = new List<ReceiptVoucherLinePrintModel>();
        }

        public int Id { get; set; }
        public string IncurredDebtLabel { get; set; }
        public string CancellationReason { get; set; }

        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public string TargetId { get; set; }
        public string VoucherCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public string ContractCode { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string ReductionReason { get; set; }
        public string CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedUserFullName { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PaymentPeriod { get; set; }
        public decimal OtherFee { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal ReductionFreeTotal { get; set; }
        public decimal CashTotal { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal GrandTotalIncludeDebt { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal RemainingTotal { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool IsEnterprise { get; set; }
        public PaymentMethod Payment { get; set; }
        public float TaxPercentage { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public string IssuedDateFormat => IssuedDate.ToString("dd/MM/yyyy");
        public string CreatedDateFormat => CreatedDate.ToString("dd/MM/yyyy");
        public int? OutContractId { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentBankAccount { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public string GrandTotalText { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public decimal OpeningDebtAmount { get; set; }
        public bool IsHasCollectionFee { get; set; }
        public decimal CODCollectionFee { get; set; }
        public VoucherTargetDTO Target { get; set; }
        public List<DebtPrintModel> DebtsByService { get; set; }
        public List<ReceiptVoucherLinePrintModel> ReceiptLines { get; set; }
    }

    public class ReceiptVoucherLinePrintModel
    {
        public int Id { get; set; }
        public string CId { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public int UsingMonths { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ReductionFee { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public bool IsHasPrice { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal DiscountAmountSuspend { get; set; }
        public string SPSuspensionTimeIds { get; set; }
        public decimal OpeningDebtAmount { get; set; } // Nợ đầu kỳ
        public bool HasDistinguishBandwidth { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public int PricingType { get; set; }
        public decimal OverloadUsageDataPrice { get; set; }
        public decimal IOverloadUsageDataPrice { get; set; }
        public decimal ConsumptionBasedPrice { get; set; }
        public decimal IConsumptionBasedPrice { get; set; }
        public decimal DataUsage { get; set; }
        public decimal DataUsageUnit { get; set; }
        public decimal IDataUsageUnit { get; set; }
        public decimal IDataUsage { get; set; }
        public decimal UsageDataAmount { get; set; }
        public decimal IUsageDataAmount { get; set; }
        public bool IsMainPaymentChannel { get; set; }
        public bool IsJoinedPayment { get; set; }
        public bool IsShow { get; set; }
    }

    public class DebtPrintModel
    {
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal OpeningDebtTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal ReductionFee { get; set; }
        public string Content { get; set; }
        public string ReductionContent { get; set; }
        public decimal DebtTotal
        {
            get
            {
                if (ReductionFee > 0)
                {
                    return GrandTotal + OpeningDebtTotal - ReductionFee > 0
                        ? GrandTotal + OpeningDebtTotal - ReductionFee
                        : 0;
                }

                return GrandTotal + OpeningDebtTotal;
            }
        }
    }
}
