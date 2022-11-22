using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    public class VoucherAutoGenerateHistory : Entity
    {
        public long Records { get; set; }
        public string Status { get; set; }
        public int TryTimes { get; set; }
        public string Message { get; set; }
    }
}
