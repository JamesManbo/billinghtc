using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.DebtModels
{
    public class InDebtByPartnerDTO
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string PartnerId { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PartnerPhone { get; set; }
        public decimal OpeningDebt { get; set; }
        public decimal IncurredDebt { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal ReductionAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ClosingDebt { get; set; }
    }
}
