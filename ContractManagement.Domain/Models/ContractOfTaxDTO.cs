using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContractOfTaxDTO 
    {
        public int Id { get; set; }
        public int ValueTax { get; set; }
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }
        public string TaxName { get; set; }
    }
}
