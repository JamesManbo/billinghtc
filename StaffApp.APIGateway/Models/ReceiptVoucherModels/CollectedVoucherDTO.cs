using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ReceiptVoucherModels
{
    public class CollectedVoucherDTO
    {
        public string Month { get; set; }
        public int CollectedVouchers { get; set; }
        public int UnCollectedVouchers { get; set; }
    }
}
