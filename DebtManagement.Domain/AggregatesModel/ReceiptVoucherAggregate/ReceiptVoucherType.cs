using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVoucherTypes")]
    public class ReceiptVoucherType : Enumeration
    {
        public static ReceiptVoucherType Signed = new ReceiptVoucherType(1, "Phiếu thu lần đầu");
        public static ReceiptVoucherType Billing = new ReceiptVoucherType(2, "Phiếu thu cước định kỳ");
        public static ReceiptVoucherType Realtime = new ReceiptVoucherType(4, "Phiếu thu tính cước không định kỳ");
        public static ReceiptVoucherType Other = new ReceiptVoucherType(3, "Phiếu thu khác");
        public static ReceiptVoucherType Clearing = new ReceiptVoucherType(5, "Phiếu thu bù trừ");

        public static List<ReceiptVoucherType> List()
        {
            return new List<ReceiptVoucherType>()
            {
                Signed,
                Billing,
                Realtime,
                Other
            };
        }

        public ReceiptVoucherType(int id, string name) : base(id, name)
        {
        }

    }
}
