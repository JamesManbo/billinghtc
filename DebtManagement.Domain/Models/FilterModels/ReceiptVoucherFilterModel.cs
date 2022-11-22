using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.FilterModels
{
    public class ReceiptVoucherFilterModel : RequestFilterModel
    {
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string ProjectIds { get; set; }
        public int? OutContractId { get; set; }
        public string? OutContractIds { get; set; }
        public bool? IsOutOfDate { get; set; }
        public string StatusIds { get; set; }
        public string CashierUserId { get; set; }
        public string UserId { get; set; }
        public string ServiceIds { get; set; }
    }
}
