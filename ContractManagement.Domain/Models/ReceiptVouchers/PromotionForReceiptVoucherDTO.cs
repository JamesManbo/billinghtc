using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.ReceiptVouchers
{
    public class PromotionForReceiptVoucherDTO
    {
        public int PromotionDetailId { get; set; }
        public int OutContractServicePackageId { get; set; }
        public decimal PromotionValue { get; set; }
        public int? PromotionValueType { get; set; }
        public int PromotionAmount { get; set; }
        public bool IsApplied { get; set; }
        public int? NumberMonthApplied { get; set; }
        public int ReceiptVoucherDetailId { get; set; }
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeName { get; set; }
    }
}
