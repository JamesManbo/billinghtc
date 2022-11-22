using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.Commons
{
    [Table("AttachmentFiles")]
    public class AttachmentFile : Entity
    {
        public int? ReceiptVoucherId { get; set; }
        public int? ClearingVoucherId { get; set; }
        public string ResourceStorage { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
        public int? ReceiptVoucherDetailId { get; set; }

        public int? PaymentVoucherId { get; set; }
        public int? PaymentVoucherDetailId { get; set; }
    }
}
