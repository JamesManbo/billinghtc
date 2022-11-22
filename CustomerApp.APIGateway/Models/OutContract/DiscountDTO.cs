using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class DiscountDTO
    {
        public float Percent { get; set; }
        public MoneyDTO Amount { get; set; }
        public bool Type { get; set; }
    }
}
