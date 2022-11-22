using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Transactions
{
    public class TransactionChannelTaxDTO : BaseDTO
    {
        public int TransactionServicePackageId { get; set; }
        public int TransactionId { get; set; }
        public int TaxCategoryId { get; set; }
        public string TaxCategoryName { get; set; }
        public string TaxCategoryCode { get; set; }
        public int TaxValue { get; set; }
        public int? ContractChannelTaxId { get; set; }
    }
}
