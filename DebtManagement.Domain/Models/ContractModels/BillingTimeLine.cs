using DebtManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class BillingTimeLine
    {
        public PaymentMethodForm PaymentForm { get; set; }
        public int PaymentPeriod { get; set; } // kỳ hạn thanh toán
        public int PrepayPeriod { get; set; } // kỳ hạn gia hạn
        public DateTime? Effective { get; set; } // ngày nghiệm thu kỹ thuật
        public DateTime Signed { get; set; } // ngày đăng ký dịch vụ
        public DateTime? StartBilling { get; set; } // ngày bắt đầu tính cước
        public DateTime? LatestBilling { get; set; } // ngày tính cước gần nhất
        public DateTime? NextBilling { get; set; } // ngày tính cước tiếp theo
        public DateTime? SuspensionStartDate { get; set; } // ngày bắt đầu tạm ngưng dịch vụ
        public DateTime? SuspensionEndDate { get; set; } // ngày kết thúc tạm ngưng dịch vụ
        public DateTime? TerminateDate { get; set; } // ngày kết thúc tạm ngưng dịch vụ
        public int DaysSuspended { get; set; }
        public int DaysPromotion { get; set; }
    }
}
