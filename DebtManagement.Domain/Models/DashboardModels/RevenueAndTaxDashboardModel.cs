using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.DashboardModels
{
    public class RevenueAndTaxDashboardModel
    {
        public  string Thang { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal TaxAmount { get; set; }
    }

    public class DailyRevenueByServiceModel
    {
        public string ServiceName { get; set; }
        public decimal GrandTotal { get; set; }
    }
    public class DailyRevenueByServiceGroupModel
    {
        public string GroupName { get; set; }
        public decimal GrandTotal { get; set; }
    }

}
