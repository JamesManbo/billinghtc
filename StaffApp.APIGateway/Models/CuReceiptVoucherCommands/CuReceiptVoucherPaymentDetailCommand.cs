using System;

namespace StaffApp.APIGateway.Models.CuReceiptVoucherCommands
{
    public class CuReceiptVoucherPaymentDetailCommand
    {
        public int Id { get; set; }
        public string SubstituteVoucherId { get; set; }
        public string DebtHistoryId { get; set; }
        public string ReceiptVoucherId { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}
