
using StaffApp.APIGateway.Models.CommonModels;
using System;


namespace StaffApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherDetailGridDTO
    {
        public int? PaymentVoucherId { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageName { get; set; }
        public DateTime StartBillingDate { get; set; }
        public DateTime EndBillingDate { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO OtherFeeTotal { get; set; }
        public MoneyDTO GrandTotal { get; set; }
    }
}
