using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate
{
    [Table("InContractTaxes")]
    public class InContractTax : Entity
    {
        public int? InContractId { get; set; }
        public int TaxCategoryId { get; set; }

        public InContractTax()
        {

        }

        public InContractTax(int inContractId, int taxCategoryId)
        {
            InContractId = inContractId;
            TaxCategoryId = taxCategoryId;
        }
    }
}
