using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ContractModels
{
    public class OutContractServicePackageTaxDTO
    {
        public int OutContractServicePackageId { get; set; }
        public int TaxCategoryId { get; set; }
        public string TaxCategoryName { get; set; }
        public string TaxCategoryCode { get; set; }
        public int TaxValue { get; set; }
    }
}
