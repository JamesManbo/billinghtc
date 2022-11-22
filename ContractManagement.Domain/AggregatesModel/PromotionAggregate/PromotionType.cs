using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.PromotionAggregate
{   
    [Table("PromotionTypes")]
    public class PromotionType: Entity
    {
        public static PromotionType OfferingServiceCharges = new PromotionType(1, "Tặng cước dịch vụ","");
        public static PromotionType GiveAwayTimeToUse = new PromotionType(2, "Tặng thời gian sử dụng", "");
        public static PromotionType GiveTheProduct = new PromotionType(3, "Tặng sản phẩm", "");
        
        public static IEnumerable<PromotionType> Seeds() => new PromotionType[] {
            OfferingServiceCharges,
            GiveAwayTimeToUse,
            GiveTheProduct,
        };

        public PromotionType() { }
        public PromotionType(int id, string promotionName, string description)
        {
            Id = id;
            PromotionName = promotionName;
            Description = description;
        }

        public string PromotionName { get; set; }
        public string Description { get; set; }

        public static string GetTypeName(int type)
        {
            var ob = Seeds().Where(t => t.Id == type).FirstOrDefault();
            if (ob == null) return "";
            return ob.PromotionName;
        }
    }
}
