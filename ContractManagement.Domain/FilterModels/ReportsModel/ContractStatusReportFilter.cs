using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels.ReportsModel
{
    public class ContractStatusReportFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
