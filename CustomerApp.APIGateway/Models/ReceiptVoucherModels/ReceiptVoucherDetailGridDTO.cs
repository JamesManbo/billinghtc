using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherDetailGridDTO
    {
        public int? PaymentVoucherId { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageName { get; set; }
        public DateTime StartBillingDate { get; set; }
        public DateTime EndBillingDate { get; set; }
        public string StartBillingDateFormat { get { return StartBillingDate.ToString("dd/MM/yyyy"); } }
        public string EndBillingDateFormat { get { return EndBillingDate.ToString("dd/MM/yyyy"); } }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO OtherFeeTotal { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public MoneyDTO PackagePrice { get; set; }
        public MoneyDTO GrandTotalBeforeTax { get; set; }
    }
}
