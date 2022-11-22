using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate
{
    [Table("PaymentVoucherStatuses")]
    public class PaymentVoucherStatus : Enumeration
    {
        public static PaymentVoucherStatus New = new PaymentVoucherStatus(1, "Đang chờ xử lý");
        public static PaymentVoucherStatus SentToAccountant = new PaymentVoucherStatus(2, "Đã chuyển kế toán");
        public static PaymentVoucherStatus Due = new PaymentVoucherStatus(3, "Đến hạn");
        public static PaymentVoucherStatus Success = new PaymentVoucherStatus(4, "Đã thanh toán");
        public static PaymentVoucherStatus Overdue = new PaymentVoucherStatus(5, "Quá hạn");
        public static PaymentVoucherStatus Rejected = new PaymentVoucherStatus(7, "Kế toán từ chối");
        public static PaymentVoucherStatus Canceled = new PaymentVoucherStatus(6, "Đã hủy");
        public PaymentVoucherStatus(int id, string name) : base(id, name)
        {
        }

        public static List<PaymentVoucherStatus> List()
        {
            return new List<PaymentVoucherStatus>()
            {
                New,
                SentToAccountant,
                Due,
                Overdue,
                Success,
                Rejected,
                Canceled
            };
        }

        public static List<PaymentVoucherStatus> ValidStates()
        {
            return new List<PaymentVoucherStatus>()
            {
                New,
                SentToAccountant,
                Due,
                Overdue,
                Success,
            };
        }

        public static List<PaymentVoucherStatus> InvalidStates()
        {
            return new List<PaymentVoucherStatus>()
            {
                Canceled,
                Rejected,
            };
        }
    }
}
