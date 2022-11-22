using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherPaymentDetailDTO : BaseDTO
    {
        public static ReceiptVoucherPaymentDetailDTO Default(int method)
        {
            return new ReceiptVoucherPaymentDetailDTO()
            {
                IsActive = true,
                PaidAmount = 0,
                PaymentMethod = method,
                PaymentMethodName = Enumeration.FromValue<VoucherPaymentMethod>(method).ToString(),
                PaymentDate = DateTime.Now
            };
        }
        public int? ReceiptVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public int? DebtHistoryId { get; set; }
        public DateTime IssuedDate { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsActive { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int IssuedMonth => this.IssuedDate.Month;
        public string CurrencyUnitCode { get; set; }
        public int PaymentTurn { get; set; }
    }
}
