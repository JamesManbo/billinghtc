using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVoucherStatuses")]
    public class ReceiptVoucherStatus : Enumeration
    {
        public static ReceiptVoucherStatus Pending = new ReceiptVoucherStatus(1, "Đang chờ xử lý");
        public static ReceiptVoucherStatus SentToAccountant = new ReceiptVoucherStatus(2, "Đã chuyển kế toán");
        public static ReceiptVoucherStatus Invoiced = new ReceiptVoucherStatus(3, "Đã xuất hóa đơn");
        public static ReceiptVoucherStatus Success = new ReceiptVoucherStatus(4, "Đã thu");
        public static ReceiptVoucherStatus Canceled = new ReceiptVoucherStatus(5, "Đã hủy");
        public static ReceiptVoucherStatus CollectOnBehalf = new ReceiptVoucherStatus(6, "Thu hộ");
        public static ReceiptVoucherStatus PayingBadDebt = new ReceiptVoucherStatus(7, "Đã xóa nợ xấu");
        public static ReceiptVoucherStatus BadDebt = new ReceiptVoucherStatus(8, "Nợ xấu");
        public static ReceiptVoucherStatus Overdue = new ReceiptVoucherStatus(9, "Đã quá hạn thanh toán");

        public static bool IsPaidStatus(int statusId)
        {
            return 
                Success.Id == statusId ||
                CollectOnBehalf.Id == statusId;
        }

        public static int[] UnpaidStatuses()
        {
            return new int[]
            {
                Pending.Id,
                SentToAccountant.Id,
                CollectOnBehalf.Id,
                BadDebt.Id,
                Overdue.Id
            };
        }

        public static int[] PaidStatuses()
        {
            return new int[]
            {
                Success.Id,
                PayingBadDebt.Id
            };
        }

        public ReceiptVoucherStatus(int id, string name) : base(id, name)
        {
        }

        public static List<ReceiptVoucherStatus> List()
        {
            return new List<ReceiptVoucherStatus>()
            {
                Pending,
                CollectOnBehalf,
                SentToAccountant,
                Invoiced,
                Success,
                Canceled,
                PayingBadDebt,
                BadDebt,
                Overdue
            };
        }

        public static List<ReceiptVoucherStatus> InvalidStates()
        {
            return new List<ReceiptVoucherStatus>()
            {
                Canceled
            };
        }
    }
}
