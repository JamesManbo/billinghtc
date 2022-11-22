using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CommonModels
{
    public class DiscountDTO
    {
        public float Percent { get; set; }
        public MoneyDTO Amount { get; set; }
        public bool Type { get; set; } // true: percent, false: amount

        protected DiscountDTO()
        {
        }

        public DiscountDTO(float percent = 0, bool type = true)
        {
            Percent = percent;
            Type = type;
        }
    }
}
