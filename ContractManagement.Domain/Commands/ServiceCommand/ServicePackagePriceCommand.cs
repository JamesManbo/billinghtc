using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.ServiceCommand
{
    public class ServicePackagePriceCommand
    {
        public int Id { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public decimal PriceBeforeTax { get; set; }
        public decimal PriceValue { get; set; }
        public int? MarketAreaId { get; set; }
        public int? TaxCategoryId { get; set; }
        public string TaxCategoryName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitName { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string CurrencyUnitSymbol { get; set; }
    }
}
