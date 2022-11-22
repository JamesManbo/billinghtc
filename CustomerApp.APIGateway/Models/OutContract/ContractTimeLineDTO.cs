using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class ContractTimeLineDTO
    {
        public int RenewPeriod { get; set; }
        public int PaymentPeriod { get; set; }
        public DateTime? Expiration { get; set; }
        public string ExpirationFormat { get { return Expiration?.ToString("dd/MM/yyyy"); } }
        public DateTime? Liquidation { get; set; }
        public string LiquidationFormat { get { return Liquidation?.ToString("dd/MM/yyyy"); } }
        public DateTime? Effective { get; set; }
        public string EffectiveFormat { get { return Effective?.ToString("dd/MM/yyyy"); } }
        public DateTime? Signed { get; set; }
        public string SignedFormat { get { return Signed?.ToString("dd/MM/yyyy"); } }

    }
}
