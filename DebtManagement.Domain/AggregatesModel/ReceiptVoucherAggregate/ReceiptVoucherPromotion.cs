using CsvHelper.Configuration.Attributes;
using DebtManagement.Domain.Seed;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("PromotionForReceiptVoucher")]
    public class PromotionForReceiptVoucher : Entity
    {
        public int? ReceiptVoucherId { get; set; }
        public int PromotionDetailId { get; set; }
        public int OutContractServicePackageId { get; set; }
        public int? PromotionValue { get; set; }
        public int? PromotionValueType { get; set; }
        public int PromotionAmount { get; set; }
        public bool IsApplied { get; set; }
        public int? NumberMonthApplied { get; set; }
        public int ReceiptVoucherDetailId { get; set; }
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeName { get; set; }

        [IgnoreDataMember]
        [Ignore]
        public virtual ReceiptVoucher ReceiptVoucher { get; set; }

    }

    public class PromotionForReceiptVoucherCommand
    {
        public int Id { get; set; }
        public int ReceiptVoucherId { get; set; }
        public int PromotionForReceiptVoucherId { get; set; }
        public int PromotionDetailId { get; set; }
        public int OutContractServicePackageId { get; set; }
        public int? PromotionValue { get; set; }
       
        public int? PromotionValueType { get; set; }
        public int? PromotionValueTypeName { get; set; }
        public bool IsApplied { get; set; }
        public int? NumberMonthApplied { get; set; }
        public int ReceiptVoucherDetailId { get; set; }
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeName { get; set; }
        public int PromotionAmount { get; set; }
    }
}
