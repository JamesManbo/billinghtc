using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.DebtModels.OutDebts;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ReceiptVoucherDTO : VoucherBaseDTO
    {
        public ReceiptVoucherDTO()
        {
            ReceiptLines = new List<ReceiptVoucherDetailDTO>();
            IncurredDebtPayments = new List<OpeningDebtByReceiptVoucherModel>();
            OpeningDebtPayments = new List<OpeningDebtByReceiptVoucherModel>();
            PromotionForReceiptVouchers = new List<PromotionForReceiptVoucherDTO>();
        }

        public int? OutContractId { get; set; }
        public decimal DiscountAmount { get; set; }
        public float DiscountPercent { get; set; }
        public bool DiscountType { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentBankAccount { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public string GrandTotalText { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }

        public decimal OpeningDebtAmount { get; set; }

        public decimal OpeningDebtPaidAmount { get; set; } // Nợ đầu kỳ đã thanh toán
        public decimal GrandTotalIncludeDebt { get; set; } // Chi phí cuối cùng sau thuế bao gồm thêm nợ đầu kỳ
        public bool? IsBadDebt { get; set; }
        public int FeedbackCount { get; set; }
        public string StatusName => Enumeration.FromValue<ReceiptVoucherStatus>(StatusId).ToString();
        public bool IsHasCollectionFee { get; set; }
        public decimal CODCollectionFee { get; set; }
        public VoucherTargetDTO Target { get; set; }
        public List<ReceiptVoucherDetailDTO> ReceiptLines { get; set; }
        public List<OpeningDebtByReceiptVoucherModel> IncurredDebtPayments { get; set; }
        public List<OpeningDebtByReceiptVoucherModel> OpeningDebtPayments { get; set; }
        public List<PromotionForReceiptVoucherDTO> PromotionForReceiptVouchers { get; set; }
    }
}
