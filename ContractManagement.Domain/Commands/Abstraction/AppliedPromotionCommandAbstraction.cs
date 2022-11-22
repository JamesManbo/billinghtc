using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.Abstraction
{
    public abstract class AppliedPromotionCommandAbstraction
    {
        public int Id { get; set; }
        public int PromotionDetailId { get; set; }
        public decimal PromotionValue { get; set; }
        public int? PromotionValueType { get; set; }
        public bool IsApplied { get; set; }
        public int NumberMonthApplied { get; set; }

        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeName { get; set; }
    }
}
