using System;

namespace StaffApp.APIGateway.Models.CommonModels
{
    public class BillingTimeLineDTO
    {
        public int PaymentPeriod { get; set; } // kỳ hạn gia hạn
        public DateTime? Effective { get; set; } // ngày nghiệm thu kỹ thuật
        public string EffectiveFormat { get { return Effective.HasValue? Effective.Value.ToString("dd/MM/yyyy"):""; } }
        public DateTime Signed { get; set; } // ngày đăng ký dịch vụ
        public string SignedFormat { get { return Signed.ToString("dd/MM/yyyy"); } }
        public DateTime? LatestBilling { get; set; } // ngày tính cước gần nhất
        public string LatestBillingFormat { get { return LatestBilling.HasValue ? LatestBilling.Value.ToString("dd/MM/yyyy") : ""; } }
        public DateTime? NextBilling { get; set; } // ngày tính cước tiếp theo
        public string NextBillingFormat { get { return NextBilling.HasValue ? NextBilling.Value.ToString("dd/MM/yyyy") : ""; } }
        public DateTime? SuspensionStartDate { get; set; } // ngày bắt đầu tạm ngưng dịch vụ
        public string SuspensionStartDateFormat { get { return SuspensionStartDate.HasValue ? SuspensionStartDate.Value.ToString("dd/MM/yyyy") : ""; } }
        public DateTime? SuspensionEndDate { get; set; } // ngày kết thúc tạm ngưng dịch vụ
        public string SuspensionEndDateFormat { get { return SuspensionEndDate.HasValue ? SuspensionEndDate.Value.ToString("dd/MM/yyyy") : ""; } }
        public int DaysSuspended { get; set; }
    }
}
