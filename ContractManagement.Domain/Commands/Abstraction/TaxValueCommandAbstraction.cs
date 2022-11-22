using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.Abstraction
{
    public abstract class TaxValueCommandAbstraction
    {
        public int Id { get; set; }
        public int TaxCategoryId { get; set; }
        public string TaxCategoryName { get; set; }
        public string TaxCategoryCode { get; set; }
        public float TaxValue { get; set; }
    }
}
