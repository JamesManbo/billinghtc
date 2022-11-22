using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.BaseContractModels
{
    public class CreateUpdateContractTax
    {
        public int Id { get; set; }
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }

        public int TaxCategoryId { get; set; }
        public string TaxCategoryName { get; set; }
        public string TaxCategoryCode { get; set; }
        public float TaxValue { get; set; }
    }
}
