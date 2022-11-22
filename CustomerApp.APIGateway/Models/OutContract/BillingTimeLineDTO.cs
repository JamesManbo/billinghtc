using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class BillingTimeLineDTO
    {
        public int? PaymentPeriod { get; set; }
        public DateTime? Effective { get; set; }
        public string EffectiveFormat { get { return Effective.HasValue ? Effective.Value.ToString("dd-MM-yyyy") : ""; } }
        public DateTime? Signed { get; set; }
        public string SignedFormat { get { return Signed.HasValue ? Signed.Value.ToString("dd-MM-yyyy") : ""; } }
        public DateTime? LatestBilling { get; set; }
        public string LatestBillingFormat { get { return LatestBilling.HasValue ? LatestBilling.Value.ToString("dd-MM-yyyy") : ""; } }
        public DateTime? NextBilling { get; set; }
        public string NextBillingFormat { get { return NextBilling.HasValue ? NextBilling.Value.ToString("dd-MM-yyyy") : ""; } }
    }
}
