using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherDetailDTO
    {
        public string Id { get; set; }
        public int? PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public int? ServicePackageId { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageName { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO OtherFeeTotal { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public MoneyDTO PromotionAmount { get; set; }
        public MoneyDTO ReductionFee { get; set; }
        public MoneyDTO EquipmentTotalAmount { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OffsetUpgradePackageAmount { get; set; }
        public MoneyDTO PackagePrice { get; set; }
        public MoneyDTO GrandTotalBeforeTax { get; set; }
    }
}
