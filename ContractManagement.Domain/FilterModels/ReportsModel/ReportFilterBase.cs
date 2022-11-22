using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels.ReportsModel
{
    public class ReportFilterBase2 : RequestFilterModel
    {
		public string ContractCode { get; set; }
		public string CustomerCode { get; set; }
		public DateTime TimelineSignedStart { get; set; }
		public DateTime TimelineSignedEnd { get; set; }
		public int ServiceId { get; set; }
		public int ProjectId { get; set; }
		public int MarketAreaId { get; set; }
	}
}
