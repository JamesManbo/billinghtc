using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ReportModels
{
    public class ReceiptVoucherNumberByMonthReport
    {
        public string Month { get; set; }
        public int CollectedVouchers { get; set; }
        public string UnCollectedVouchers { get; set; }
    }

    public class ReceiptVoucherNumberValue
    {
        public int NumberOfVouchers { get; set; }
        public int VoucherType { get; set; }
        public string Unit { get; set; }
    }
}
