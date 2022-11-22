using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.PromotionAggregate
{
    [Table("PromotionProducts")]
    public class PromotionProduct : Entity
    {
        public int PromotionDetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public bool IsOurProduct { get; set; }

    }
}
