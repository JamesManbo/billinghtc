using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReportModels
{
    public class ReportCustomerFillter : RequestFilterModel
    {
        private string _orderBy;
        public int Year { get; set; }
        public int Month { get; set; }
        public int[] CustomerIds { get; set; }
        public int[] ProjectIds { get; set; }
        public int MarketAreaId { get; set; }
        public override string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "Id";

            set => _orderBy = value;
        }

    }
}
