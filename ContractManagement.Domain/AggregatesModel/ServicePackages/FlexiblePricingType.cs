using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("FlexiblePricingType")]
    public class FlexiblePricingType : Enumeration
    {
        public static FlexiblePricingType FixedPricing = new FlexiblePricingType(1, "Đơn giá cố định hàng tháng");
        public static FlexiblePricingType OverlimitUsagedBasedPricing = new FlexiblePricingType(2, "Đơn giá cố định có tính vượt mức");
        public static FlexiblePricingType CumulativeUsagedBasedPricing = new FlexiblePricingType(3, "Đơn giá lũy kế theo dung lượng sử dụng");
        public static FlexiblePricingType DailyBusTablePricing = new FlexiblePricingType(5, "Bustable hàng ngày");
        public static FlexiblePricingType MonthlyBusTablePricing = new FlexiblePricingType(6, "Bustable hàng tháng");


        public FlexiblePricingType(int id, string name) : base(id, name)
        {
        }

        public static FlexiblePricingType[] List()
        {
            return new FlexiblePricingType[]
            {
                FixedPricing,
                OverlimitUsagedBasedPricing,
                CumulativeUsagedBasedPricing,
                DailyBusTablePricing,
                MonthlyBusTablePricing
            };
        }
    }
}
