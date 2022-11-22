using System;
using System.Collections.Generic;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    public class BillingTimeLine : ValueObject
    {
        public int PrepayPeriod { get; set; } = 0; // số tháng thanh toán trước
        public int PaymentPeriod { get; set; } = 0; // kỳ hạn gia hạn
        public DateTime? Effective { get; set; } // ngày nghiệm thu kỹ thuật
        public DateTime Signed { get; set; } // ngày đăng ký dịch vụ
        public DateTime? StartBilling { get; set; } // ngày bắt đầu tính cước
        public DateTime? LatestBilling { get; set; } // ngày tính cước gần nhất
        public DateTime? NextBilling { get; set; } // ngày tính cước tiếp theo
        public DateTime? SuspensionStartDate { get; set; } // ngày bắt đầu tạm ngưng dịch vụ
        public DateTime? SuspensionEndDate { get; set; } // ngày kết thúc tạm ngưng dịch vụ
        public DateTime? TerminateDate { get; set; } // ngày kết thúc tạm ngưng dịch vụ
        public int DaysSuspended { get; set; } = 0;
        public int DaysPromotion { get; set; } = 0; // Số ngày khuyến mại
        public int PaymentForm { get; set; } = 0;
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PrepayPeriod;
            yield return PaymentPeriod;
            yield return Effective;
            yield return Signed;
            yield return StartBilling;
            yield return LatestBilling;
            yield return NextBilling;
            yield return SuspensionStartDate;
            yield return SuspensionEndDate;
            yield return TerminateDate;
            yield return DaysSuspended;
            yield return DaysPromotion;
            yield return PaymentForm;
        }
    }
}
