using System;

namespace DebtManagement.Domain.Models.FilterModels
{
    public class CollectedVoucherFilter
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string CashierUserId { get; set; }
    }
}
