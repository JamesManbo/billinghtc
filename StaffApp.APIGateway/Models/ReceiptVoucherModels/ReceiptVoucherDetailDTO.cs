
using DebtManagement.API.Protos;
using StaffApp.APIGateway.Models.CommonModels;
using System;

namespace StaffApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherDetailDTO
    {
        public string Id { get; set; }
        public int? PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public int UsingMonths { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public string StartBillingDateFormat { get { return StartBillingDate.HasValue?StartBillingDate.Value.ToString("dd/MM/yyyy"):""; } }
        public DateTime? EndBillingDate { get; set; }
        public string EndBillingDateFormat { get { return EndBillingDate.HasValue? EndBillingDate.Value.ToString("dd/MM/yyyy"):""; } }
        public Money ReductionFee { get; set; }
        public Money SubTotal { get; set; }
        public Money OtherFeeTotal { get; set; }
        public Money GrandTotal { get; set; }
        public Money EquipmentTotalAmount { get; set; }
        public Money InstallationFee { get; set; }
        public Money OffsetUpgradePackageAmount { get; set; }
        public Money PromotionAmount { get; set; }
        public Money PackagePrice { get; set; }
        public Money DiscountAmountSuspend { get; set; }
    }
}
