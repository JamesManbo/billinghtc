using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.DebtModels
{
    public class OutDebtCollectionOnBehalfDTO
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string CashierUserId { get; set; }
        public string CashierFullName { get; set; }
        public string CashierUserName { get; set; }
        public decimal OpeningDebt { get; set; }
        public decimal IncurredDebt { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal ReductionAmount { get; set; }
        public decimal ClearingAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ClosingDebt { get; set; }
    }
}
