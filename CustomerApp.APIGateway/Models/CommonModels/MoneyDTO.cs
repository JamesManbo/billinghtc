using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.CommonModels
{
    public class MoneyDTO
    {
        public string Value { get; set; }
        public string FormatValue { get; set; }
        public string CurrencyCode { get; set; }
    }
}
