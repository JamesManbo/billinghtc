using DebtManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;

namespace DebtManagement.Domain.Commands.BaseVoucherCommand
{
    public abstract class CUVoucherBaseCommand
    {
        public CUVoucherBaseCommand()
        {
        }
        public int Id { get; set; }
        public int TargetId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Discount Discount { get; set; }
        public PaymentMethod Payment { get; set; }
        public CUVoucherTargetCommand Target { get; set; }
        public int InContractId { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public string MarketAreaCode { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ContractCode { get; set; }
        public int TypeId { get; set; }
        public string VoucherCode { get; set; }
        public string AccountingCode { get; set; }
        public int PaymentPeriod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public int StatusId { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string CreatedUserId { get; set; }
        public string CreatedUserFullName { get; set; }
        public string CashierUserId { get; set; }
        public string OrganizationUnitId { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public decimal OtherFee { get; set; }
        public decimal ReductionFreeTotal { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal DiscountAmountSuspendTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal FixedGrandTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal CashTotal { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal OpeningDebtAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal GrandTotalIncludeDebt { get; set; }
        public decimal RemainingTotal { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public string OrganizationUnitName { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public CommandSource Source { get; set; } = CommandSource.CMS;
        public bool FixedTotal { get; set; }
        public bool IsPaidAll { get; set; }
        public int? ClearingId { get; set; }
        public bool IsAutomaticGenerate { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public int NumberDaysOverdue { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public string CancellationReason { get; set; }
    }
}
