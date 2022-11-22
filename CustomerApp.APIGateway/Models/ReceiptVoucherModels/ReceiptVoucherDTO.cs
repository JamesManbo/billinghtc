using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherDTO
    {
        public string Id { get; set; }
        public ReceiptVoucherDTO()
        {
            ReceiptLines = new List<ReceiptVoucherDetailDTO>();
            ReceiptVoucherTaxes = new List<TaxCategoryDTO>();
            IncurredDebtPayments = new List<OpeningDebtByReceiptVoucherModel>();
            OpeningDebtPayments = new List<OpeningDebtByReceiptVoucherModel>();
        }
        public string AccountingCode { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TargetId { get; set; }
        public string TargetFullName { get; set; }
        public string TargetCode { get; set; }
        public VoucherTargetDTO VoucherTarget { get; set; }
        public int TypeId { get; set; }
        public string VoucherCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedDateFormat { get { return IssuedDate.ToString("dd/MM/yyyy"); } }
        public int StatusId { get; set; }
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
        public string OrganizationUnitId { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateFormat { get { return InvoiceDate != null? InvoiceDate.Value.ToString("dd/MM/yyyy"):""; } }
        public DateTime? PaymentDate { get; set; }
        public string PaymentDateFormat { get { return PaymentDate != null ? PaymentDate.Value.ToString("dd/MM/yyyy") : ""; } }
        public int PaymentPeriod { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }

        public MoneyDTO OtherFee { get; set; }
        public MoneyDTO TaxAmount { get; set; }
        public MoneyDTO EquipmentTotalAmount { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO ReductionFreeTotal { get; set; }
        public MoneyDTO CashTotal { get; set; }
        public MoneyDTO ClearingTotal { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO GrandTotalBeforeTax { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public MoneyDTO FixedGrandTotal { get; set; }
        public MoneyDTO PaidTotal { get; set; }
        public MoneyDTO RemainingTotal { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsLock { get; set; }
        public string ClearingId { get; set; }
        public float TaxPercentage { get; set; }
        public string CodeClearing { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public int? OutContractId { get; set; }
        public MoneyDTO DiscountAmount { get; set; }
        public float DiscountPercent { get; set; }
        public bool DiscountType { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentBankAccount { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public List<ReceiptVoucherDetailDTO> ReceiptLines { get; set; }
        public List<TaxCategoryDTO> ReceiptVoucherTaxes { get; set; }
        public List<OpeningDebtByReceiptVoucherModel> IncurredDebtPayments { get; set; }
        public List<OpeningDebtByReceiptVoucherModel> OpeningDebtPayments { get; set; }
        public string CancellationReason { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public DateTime CreatedDate { get; set; }
        public MoneyDTO OpeningDebtAmount { get; set; } // Nợ đầu kỳ
        public MoneyDTO OpeningDebtPaidAmount { get; set; } // Nợ đầu kỳ đã thanh toán
        public MoneyDTO GrandTotalIncludeDebt { get; set; } // Chi phí cuối cùng sau thuế bao gồm thêm nợ đầu kỳ
        public bool? IsBadDebt { get; set; }
        public int FeedbackCount { get; set; }
        public bool IsHasCollectionFee { get; set; }
        public decimal CODCollectionFee { get; set; }
    }
}
