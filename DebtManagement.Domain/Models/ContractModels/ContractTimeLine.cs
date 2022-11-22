using System;
using System.Collections.Generic;

namespace DebtManagement.Domain.Models
{
    public class ContractTimeLine
    {
        public int RenewPeriod { get; set; } // kỳ hạn hợp đồng
        public int PaymentPeriod { get; set; } // kỳ hạn thanh toán
        public DateTime? Expiration { get; set; } // ngày hết hạn hợp đồng
        public DateTime? Liquidation { get; set; } // ngày thanh lý hợp đồng
        public DateTime? Effective { get; set; } // ngày nghiệm thu kỹ thuật
        public DateTime? Signed { get; set; } // ngày k hợp đồng

        protected ContractTimeLine()
        {

        }

        public ContractTimeLine(int period, int paymentPeriod, DateTime? signedDate = null)
        {
            this.RenewPeriod = period;
            this.PaymentPeriod = paymentPeriod;
            this.Signed = signedDate;
            if (this.Signed.HasValue)
            {
                this.Expiration = this.Signed.Value.AddMonths(RenewPeriod);
            }
        }
    }
}