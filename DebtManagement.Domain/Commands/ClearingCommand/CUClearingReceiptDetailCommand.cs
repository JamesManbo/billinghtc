using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ClearingCommand
{
    public class CUClearingReceiptDetailCommand
    {
        public string Id { get; set; }
        public string ClearingId { get; set; }
        public string ReceiptVoucherId { get; set; }
        public decimal ClearingAmount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
