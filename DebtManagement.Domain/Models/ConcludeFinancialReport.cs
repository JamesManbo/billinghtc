using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ConcludeFinancialReport
    {

        public decimal RevenueTotal { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal DebtTotal { get;set;  }
        public int CurrnecyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
    }
}
