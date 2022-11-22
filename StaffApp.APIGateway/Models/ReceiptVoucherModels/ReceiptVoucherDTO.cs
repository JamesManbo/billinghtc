
using DebtManagement.API.Protos;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.DebtModels;
using StaffApp.APIGateway.Models.TaxCategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherDTO
    {
        public ReceiptVoucherDTO()
        {
            ReceiptLines = new List<ReceiptVoucherDetailDTO>();
            ReceiptVoucherTaxes = new List<ReceiptVoucherTaxDTO>();
            IncurredDebtPayments = new List<OpeningDebtByReceiptVoucherModel>();
            OpeningDebtPayments = new List<OpeningDebtByReceiptVoucherModel>();
        }
        public string Id { get; set; }
        public string AccountingCode { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TargetId { get; set; }
        public int TypeId { get; set; }
        public string VoucherCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
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
        public DateTime? PaymentDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PaymentPeriod { get; set; }

        public Money OtherFee { get; set; }
        public Money TaxAmount { get; set; }
        public Money EquipmentTotalAmount { get; set; }
        public Money InstallationFee { get; set; }
        public Money ReductionFreeTotal { get; set; }
        public Money CashTotal { get; set; }
        public Money ClearingTotal { get; set; }
        public Money SubTotal { get; set; }
        public Money GrandTotalBeforeTax { get; set; }
        public Money GrandTotal { get; set; }
        public Money FixedGrandTotal { get; set; }
        public Money PaidTotal { get; set; }
        public Money RemainingTotal { get; set; }
        public Money TaxTotalAmount { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public Discount Discount { get; set; }
        public bool IsEnterprise { get; set; }
        public PaymentMethod Payment { get; set; }
        public bool IsLock { get; set; }
        public string ClearingId { get; set; }
        public float TaxPercentage { get; set; }
        public string CodeClearing { get; set; }
        public int NumberBillingLimitDays { get; set; }

        public int? OutContractId { get; set; }
        public Money DiscountAmount { get; set; }
        public float DiscountPercent { get; set; }
        public bool DiscountType { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentBankAccount { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public VoucherTargetDTO VoucherTarget { get; set; }
        public List<ReceiptVoucherDetailDTO> ReceiptLines { get; set; }
        public List<ReceiptVoucherTaxDTO> ReceiptVoucherTaxes { get; set; }
        public List<OpeningDebtByReceiptVoucherModel> IncurredDebtPayments { get; set; }
        public List<OpeningDebtByReceiptVoucherModel> OpeningDebtPayments { get; set; }
        public string GrandTotalText { get; set; }
        public string CancellationReason { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }

        public Money OpeningDebtAmount { get; set; } // Nợ đầu kỳ
        public Money OpeningDebtPaidAmount { get; set; } // Nợ đầu kỳ đã thanh toán
        public Money GrandTotalIncludeDebt { get; set; } // Chi phí cuối cùng sau thuế bao gồm thêm nợ đầu kỳ
        public bool? IsBadDebt { get; set; }
        public bool IsHasCollectionFee { get; set; }
        public decimal CODCollectionFee { get; set; }

        public string IssuedDateFormat => IssuedDate.ToString("dd/MM/yyyy");
        public string PaymentDateFormat => PaymentDate?.ToString("dd/MM/yyyy");
        public string InvoiceDateFormat => InvoiceDate?.ToString("dd/MM/yyyy");
        public string CreatedDateFormat => CreatedDate.ToString("dd/MM/yyyy");
    }
}
