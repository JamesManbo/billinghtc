using System;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class CuReceiptVoucherPaymentDetailCommand
    {
        public int Id { get; set; }
        public int? SubstituteVoucherId { get; set; }
        public int? DebtHistoryId { get; set; }
        public int? ReceiptVoucherId { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int PaymentTurn { get; set; }
    }
}
