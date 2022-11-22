using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class CollectedVouchersDTO
    {
        public string Month { get; set; }
        public int CollectedVouchers { get; set; }
        public int UnCollectedVouchers { get; set; }
    }
}
