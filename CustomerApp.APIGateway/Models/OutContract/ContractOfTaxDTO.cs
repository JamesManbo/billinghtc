using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class ContractOfTaxDTO
    {
        public int Id { get; set; }
        public int ValueTax { get; set; }
        public string TaxName { get; set; }
    }
}
