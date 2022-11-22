using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CommonModels
{
    public class ContractTimeLineDTO
    {
        public int RenewPeriod { get; set; } // kỳ hạn hợp đồng
        public int PaymentPeriod { get; set; } // kỳ hạn thanh toán
        public DateTime? Expiration { get; set; } // ngày hết hạn hợp đồng
        public string ExpirationFormat { get { return Expiration?.ToString("dd/MM/yyyy"); } }
        public DateTime? Liquidation { get; set; } // ngày thanh lý hợp đồng
        public string LiquidationFormat { get { return Liquidation?.ToString("dd/MM/yyyy"); } }
        public DateTime? Effective { get; set; } // ngày nghiệm thu kỹ thuật
        public string EffectiveFormat { get { return Effective?.ToString("dd/MM/yyyy"); } }
        public DateTime? Signed { get; set; } // ngày k hợp đồng
        public string SignedFormat { get { return Signed?.ToString("dd/MM/yyyy"); } }
    }
}
