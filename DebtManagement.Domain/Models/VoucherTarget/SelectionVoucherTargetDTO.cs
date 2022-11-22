using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.VoucherTarget
{
    public class SelectionVoucherTargetDTO
    {
        public string Text { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public object GlobalValue { get; set; }
        public int? ParentId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
