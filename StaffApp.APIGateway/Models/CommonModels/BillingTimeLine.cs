using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CommonModels
{
    public class BillingTimeLine
    {
        public int PaymentPeriod { get; set; } // kỳ hạn gia hạn
        public DateTime? Effective { get; set; } // ngày nghiệm thu kỹ thuật
       
        public DateTime Signed { get; set; } // ngày đăng ký dịch vụ
       
        public DateTime? LatestBilling { get; set; } // ngày tính cước gần nhất
       
        public DateTime? NextBilling { get; set; } // ngày tính cước tiếp theo
       
        public DateTime? SuspensionStartDate { get; set; } // ngày bắt đầu tạm ngưng dịch vụ
       
        public DateTime? SuspensionEndDate { get; set; } // ngày kết thúc tạm ngưng dịch vụ
        public int DaysSuspended { get; set; }
    }
}
