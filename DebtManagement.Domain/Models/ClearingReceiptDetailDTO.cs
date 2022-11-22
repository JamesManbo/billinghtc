using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ClearingReceiptDetailDTO : BaseDTO
    {
        public string ClearingId { get; set; }
        public string ReceiptVoucherId { get; set; }
        public decimal ClearingAmount { get; set; }
    }
}
