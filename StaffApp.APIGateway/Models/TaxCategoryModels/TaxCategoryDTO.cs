using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TaxCategoryModels
{
    public class TaxCategoryDTO
    {
        public int Id { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string ExplainTax { get; set; }
    }
}
