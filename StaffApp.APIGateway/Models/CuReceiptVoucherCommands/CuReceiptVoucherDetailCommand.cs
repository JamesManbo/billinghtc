using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CuReceiptVoucherCommands
{
    public class CUReceiptVoucherDetailCommand
    {
        public CUReceiptVoucherDetailCommand()
        {
            this.ReductionDetails = new List<ReductionDetailCommand>();
        }

        public string Id { get; set; }
        //public string ReceiptVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string CId { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public int UsingMonths { get; set; }
        public decimal OrgPackagePrice { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal ReductionFee { get; set; }
        public decimal SubTotal { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public decimal DiscountAmountSuspend { get; set; }
        public string SPSuspensionTimeIds { get; set; }
        public List<ReductionDetailCommand> ReductionDetails { get; set; }

        public void CalculateTotal()
        {
            this.GrandTotal
                = this.SubTotal
                - this.PromotionAmount
                + this.InstallationFee
                + this.EquipmentTotalAmount
                + this.OtherFeeTotal;
        }
    }
    public class ReductionDetailCommand
    {
        public string Id { get; set; }
        public string ReductionReason { get; set; }
        public decimal ReductionFee { get; set; }
    }
}
