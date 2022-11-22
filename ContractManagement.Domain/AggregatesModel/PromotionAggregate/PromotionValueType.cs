using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.PromotionAggregate
{
    [Table("PromotionValueTypes")]
    public class PromotionValueType : Enumeration
    {
        public static PromotionValueType DiscountPercent = new PromotionValueType(1, "Giảm trừ cước (% giá trị)");
        public static PromotionValueType DiscountCash = new PromotionValueType(2, "Giảm trừ cước (tiền mặt)");
        public static PromotionValueType UseTimeMonth = new PromotionValueType(3, "Tặng thời gian sử dụng (theo tháng)");
        public static PromotionValueType UseTimeDay = new PromotionValueType(4, "Tặng thời gian sử dụng (theo ngày)");
        public static PromotionValueType Items = new PromotionValueType(5, "Tặng sản phẩm công ty");
        public static PromotionValueType OtherItems = new PromotionValueType(6, "Tặng sản phẩm khác");
        public static PromotionValueType OtherPromotion = new PromotionValueType(7, "Khuyến mại khác");

        public PromotionValueType(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<PromotionValueType> Seeds() => new[]
            {DiscountPercent, DiscountCash, UseTimeMonth, UseTimeDay, Items, OtherItems, OtherPromotion};

    }
}
