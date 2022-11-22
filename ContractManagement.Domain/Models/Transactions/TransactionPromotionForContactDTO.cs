using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class TransactionPromotionForContactDTO : BaseDTO
    {
        public int TransactionServicePackageId { get; set; }
        public int? OutContractServicePackageId { get; set; }
        public int PromotionDetailId { get; set; }
        public int PromotionType { get; set; }
        public int PromotionValue { get; set; }
        public bool IsApplied { get; set; }

    }
}
