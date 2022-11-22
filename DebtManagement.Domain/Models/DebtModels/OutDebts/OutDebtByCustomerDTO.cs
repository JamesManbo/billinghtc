using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.DebtModels
{
    public class OutDebtByCustomerDTO
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerTaxNo { get; set; }
        
        public decimal OpeningDebt { get; set; }
        public decimal IncurredDebt { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal ReductionAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ClosingDebt { get; set; }
        public decimal ClearingAmount { get; set; }
        public int TotalContract { get; set; }
    }
}
