using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.FilterModels
{
    public class ExportInvoiceFilterModel : ReportFilterBase
    {
        public int IsEnterprise { get; set; }
        public int CustomerCategoryId { get; set; }
        public int ServiceGroupId { get; set; }
        public int VoucherStatusId { get; set; }


    }
}
