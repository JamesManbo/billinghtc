using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReportModels
{
    public class ReportInContractFeeAndSharingFillter : ReportFilterBase
    {
        private string _orderBy;
        public int Month { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public override string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "TypeId";

            set => _orderBy = value;
        }

    }
}
