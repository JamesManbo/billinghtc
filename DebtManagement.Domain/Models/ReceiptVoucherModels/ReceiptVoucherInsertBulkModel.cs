using System;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherInsertBulkModel
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string AccountingCode { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public string Content { get; set; }
        public string ContractCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUserId { get; set; }
        public string Culture { get; set; }
        public string Description { get; set; }
        public decimal Discount_Amount { get; set; }
        public float Discount_Percent { get; set; }
        public bool Discount_Type { get; set; }
        public int DisplayOrder { get; set; }
        public bool InvalidIssuedDate { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool? IsBadDebt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime IssuedDate { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public string OrganizationPath { get; set; }
        public string OrganizationUnitId { get; set; }
        public int OutContractId { get; set; }
        public string Payment_Address { get; set; }
        public int Payment_Form { get; set; }
        public int Payment_Method { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal OtherFee { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal ReductionFreeTotal { get; set; }
        public string ReductionReason { get; set; }
        public decimal RemainingTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal CashTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal CashierDebtRemaningTotal { get; set; }
        public decimal TargetDebtRemaningTotal { get; set; }
        public decimal OpeningDebtAmount { get; set; }
        public decimal OpeningDebtPaidAmount { get; set; }
        public decimal GrandTotalIncludeDebt { get; set; }
        public decimal TaxAmount { get; set; }
        public int StatusId { get; set; }
        public int? TargetId { get; set; }
        public int TypeId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string VoucherCode { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsAutomaticGenerate { get; set; }
        public bool NumberOfDebtHistories { get; set; }
        public bool NumberOfOpeningDebtHistories { get; set; }
        public double ExchangeRate { get; set; }
        public DateTime ExchangeRateApplyDate { get; set; }
        public int PaymentPeriod { get; set; }
        public bool NumberDaysOverdue { get; set; }
    }

    public class DebtHistoryInsertBulkModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IdentityGuid { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int Status { get; set; }
        public int? ReceiptVoucherId { get; set; }
        public string ReceiptVoucherCode { get; set; }
        public string ReceiptVoucherContent { get; set; }
        public int? SubstituteVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public int NumberOfPaymentDetails { get; set; }
        public bool IsActive { get; set; }
        public bool IsAutomaticGenerate { get; set; }

        // Tổng công nợ của nhân viên/đại lý thu hộ
        public decimal OpeningCashierDebtTotal { get; set; }
        // Tổng công nợ của khách hàng(đối tượng phải thu)
        public decimal OpeningTargetDebtTotal { get; set; }
        public decimal CashingPaidTotal { get; set; }
        public decimal TransferringPaidTotal { get; set; }
        public decimal CashingAccountedTotal { get; set; }
        public decimal TransferringAccountedTotal { get; set; }
        public bool IsOpeningDebtRecorded { get; set; }
    }
}