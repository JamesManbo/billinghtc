using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.ReceiptVouchers
{
    public class ReceiptVoucherDetailDTO
    {
        public string Id { get; set; }
        public int? PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }
        public int UsingMonths { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ReductionFee { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public bool IsHasPrice { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal GrandTotal { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal DiscountAmountSuspend { get; set; }
        public string SPSuspensionTimeIds { get; set; }
        public string ReductionReasonIds { get; set; }
        public string ReductionReasons { get; set; }
        public string ReductionFees { get; set; }
    }
}
