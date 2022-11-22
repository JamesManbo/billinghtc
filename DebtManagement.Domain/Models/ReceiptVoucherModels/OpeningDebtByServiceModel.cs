using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class OpeningDebtByServiceModel
    {
        public int ReceiptVoucherId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal DebtAmount { get; set; }
    }
}
