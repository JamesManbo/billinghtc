using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.FilterModels
{
    public class PaymentVoucherReportFilter : ReportFilterBase
    {
        private string _orderBy;
        public int Month { get; set; }
        public int Year { get; set; }
        public int TypeId { get; set; }
        public int Status { get; set; }
        public string AgentId { get; set; }
        public int IsEnterprise { get; set; }
        public int ReportType { get; set; }
        public int Quarter { get; set; }
        public int PercentDivision { get; set; }

        public int ContractType { get; set; }

        public override string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "Id";

            set => _orderBy = value;
        }
    }
}
