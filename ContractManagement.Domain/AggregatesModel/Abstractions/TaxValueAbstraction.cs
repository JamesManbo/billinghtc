using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.Abstractions
{
    public abstract class TaxValueAbstraction
    {
        public int TaxCategoryId { get; set; }
        [StringLength(256)]
        public string TaxCategoryName { get; set; }
        [StringLength(256)]
        public string TaxCategoryCode { get; set; }
        public float TaxValue { get; set; }

        public virtual TaxCategory TaxCategory { get; set; }
    }

}
