using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.FilterModels
{
    public class ReceiptVoucherBothProjectNullFilterModel: ReceiptVoucherFilterModel
    {
        public bool? OnlyProject { get; set; } //no include null when has project
    }
}
