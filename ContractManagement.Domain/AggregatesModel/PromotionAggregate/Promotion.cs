using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.PromotionAggregate
{
    [Table("Promotions")]
    public class Promotion : Entity
    {
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
