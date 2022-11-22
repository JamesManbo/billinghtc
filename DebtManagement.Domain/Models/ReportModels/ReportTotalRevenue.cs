using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReportModels
{
    public class ReportTotalRevenue
    {
        public string ContractCode { get; set; }
        public string TargetFullName { get; set; }
        public string InvoiceCode { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime TimeLine_Effective { get; set; }
    }

    public class ReportTotalRevenueRaw
    {
        public ReportTotalRevenueRaw()
        {
            TimeLine_Effectives_OutContract = new List<DateTime>();
            ListTargetFullName = new List<string>();
            ListInVoiceCode = new List<string>();
        }
        public string ContractCode { get; set; }
        public List<DateTime> TimeLine_Effectives_OutContract { get; set; }
        public List<string> ListTargetFullName { get; set; }
        public List<string> ListInVoiceCode { get; set; }
        public decimal GrandTotal { get; set; }
        public int Total { get; set; }
    }
}
