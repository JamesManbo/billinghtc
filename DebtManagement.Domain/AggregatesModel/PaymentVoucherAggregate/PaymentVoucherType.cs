using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate
{
    [Table("PaymentVoucherTypes")]
    public class PaymentVoucherType : Enumeration
    {
        public static PaymentVoucherType ChannelRental = new PaymentVoucherType(1, "Thuê kênh");
        public static PaymentVoucherType Commission = new PaymentVoucherType(2, "Phân chia hoa hồng");
        public static PaymentVoucherType SharingRevenue = new PaymentVoucherType(3, "Phân chia doanh thu");
        public static PaymentVoucherType Maintenance = new PaymentVoucherType(4, "Bảo trì, bảo dưỡng");
        public static PaymentVoucherType Clearing = new PaymentVoucherType(5, "Đề nghị thanh toán bù trừ");

        public static List<PaymentVoucherType> List()
        {
            return new List<PaymentVoucherType>()
            {
                ChannelRental,
                Commission,
                SharingRevenue,
                Maintenance
            };
        }

        public PaymentVoucherType(int id, string name) : base(id, name)
        {
        }

    }
}
