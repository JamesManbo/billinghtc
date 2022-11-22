using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels
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
