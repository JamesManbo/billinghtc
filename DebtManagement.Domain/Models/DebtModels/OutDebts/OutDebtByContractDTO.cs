using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.DebtModels
{
    public class OutDebtByContractDTO
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int OutContractId { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string ServiceName { get; set; }
        public string CustomerName { get; set; }
        public decimal OpeningDebt { get; set; }
        public decimal IncurredDebt { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal ReductionAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ClosingDebt { get; set; }
    }
}
