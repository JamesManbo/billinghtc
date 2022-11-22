using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.BaseVoucher
{
    [Table("TemporaryGeneratingVouchers")]
    public class TemporaryGeneratingVoucher : Entity
    {
        public string ReceiptVoucherId { get; set; }
        public string ReceiptVoucherDetailId { get; set; }
        public string VoucherTargetId { get; set; }
        public string DebtHistoryId { get; set; }
        public string VoucherTaxId { get; set; }
        public string PromotionForVoucherId { get; set; }
    }
}
