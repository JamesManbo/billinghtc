using Global.Models.Filter;
using System;

namespace ContractManagement.Domain.FilterModels.ReportsModel
{
    public class EquipmentInProjectFilterModel : ReportFilterBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? TimelineEffectiveStart { get; set; }
        public DateTime? TimelineEffectiveEnd { get; set; }
        public int[] ProjectIds { get; set; }
        public int[] EquipmentIds { get; set; }
        public int? IsEnterprise { get; set; }
        public string SerialCode { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerCategoryId { get; set; }

    }

    public class ContractorInProjectFilterModel : ReportFilterBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int[] ProjectIds { get; set; }
        public bool GrowthOrLeave { get; set; }
        public bool Reflect { get; set; }
        public int[] PackageIds { get; set; }
    }

    public class MasterCustomerNationwideBusinessFilterModel : ReportFilterBase
    {
        private string _orderBy;
        //Mã hđ, ngày ký, ngày nghiệm thu tính cước   
        public override string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "Id";

            set => _orderBy = value;
        }

        public byte CustomerCategoryId { get; set; }
        public int StatusId { get; set; }
        public string GroupBy { get; set; }
    }
    public class ReportTotalRevenueFillter : ReportFilterBase
    {
        private string _orderBy;

        public string TransactionCode { get; set; }
        public DateTime TimelineEffectiveStart { get; set; }
        public DateTime TimelineEffectiveEnd { get; set; }
        public string SignedUserId { get; set; }
        public string OrganizationUnitId { get; set; }
        public int SignedUser { get; set; }
        public byte ReportType { get; set; }
        public byte IsEnterprise { get; set; }
        public override string OrderBy {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "Id";
            set => _orderBy = value;
        }
        public int CurrencyUnitId { get; set; }
    }
    public class CommissionAndSharingReportFilter : ReportFilterBase
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

        public override string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "Id";

            set => _orderBy = value;
        }
    } 
    public class FeeReportFilter : ReportFilterBase
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
        public string OutContractCode { get; set; }
        public string CustomerName { get; set; }

        public override string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "Id";

            set => _orderBy = value;
        }
    }

}
