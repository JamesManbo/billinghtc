using DebtManagement.Domain.Commands.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    /// <summary>
    /// Thể hiện của phiếu thu real-time
    /// Được tạo bởi form thêm mới phiếu thu tại CMS
    /// </summary>
    public class CUReceiptVoucherDetailCommand
    {
        public CUReceiptVoucherDetailCommand()
        {
            this.ReceiptVoucherLineTaxes = new List<CreateReceiptVoucherLineTaxCommand>();
            this.ReductionDetails = new List<ReductionDetailCommand>();
            this.AttachmentFiles = new List<AttachmentFileCommand>();
            this.PriceBusTables = new List<CUChannelPriceBusTableCommand>();
            this.BusTablePricingCalculators = new List<CUBusTablePricingCalculatorCommand>();
        }

        public CUReceiptVoucherDetailCommand(CUReceiptVoucherDetailCommand command)
        {
            this.ProjectId = command.ProjectId;
            this.CurrencyUnitId = command.CurrencyUnitId;
            this.CurrencyUnitCode = command.CurrencyUnitCode;
            this.ReceiptVoucherId = command.ReceiptVoucherId;
            this.ServiceId = command.ServiceId;
            this.ServiceName = command.ServiceName;
            this.CId = command.CId;
            this.ServicePackageId = command.ServicePackageId;
            this.ServicePackageName = command.ServicePackageName;
            this.HasDistinguishBandwidth = command.HasDistinguishBandwidth;
            this.HasStartAndEndPoint = command.HasStartAndEndPoint;
            this.DomesticBandwidth = command.DomesticBandwidth;
            this.InternationalBandwidth = command.InternationalBandwidth;

            this.StartBillingDate = command.StartBillingDate;
            this.EndBillingDate = command.EndBillingDate;
            this.UsingMonths = command.UsingMonths;
            this.TaxPercent = command.TaxPercent;
            this.TaxAmount = command.TaxAmount;
            this.OrgPackagePrice = command.OrgPackagePrice;
            this.PackagePrice = command.PackagePrice;
            this.PromotionAmount = command.PromotionAmount;
            this.ReductionFee = command.ReductionFee;
            this.OffsetUpgradePackageAmount = command.OffsetUpgradePackageAmount;
            this.InstallationFee = command.InstallationFee;
            this.EquipmentTotalAmount = command.EquipmentTotalAmount;
            this.OtherFeeTotal = command.OtherFeeTotal;
            this.SubTotalBeforeTax = command.SubTotalBeforeTax;
            this.SubTotal = command.SubTotal;
            this.GrandTotalBeforeTax = command.GrandTotalBeforeTax;
            this.GrandTotal = command.GrandTotal;
            this.OutContractServicePackageId = command.OutContractServicePackageId;
            this.IsFirstDetailOfService = command.IsFirstDetailOfService;
            this.DiscountAmountSuspend = command.DiscountAmountSuspend;
            this.DiscountDescription = command.DiscountDescription;
            this.SPSuspensionTimeIds = command.SPSuspensionTimeIds;

            this.PricingType = command.PricingType;
            this.OverloadUsageDataPrice = command.OverloadUsageDataPrice;
            this.IOverloadUsageDataPrice = command.IOverloadUsageDataPrice;
            this.ConsumptionBasedPrice = command.ConsumptionBasedPrice;
            this.IConsumptionBasedPrice = command.IConsumptionBasedPrice;
            this.DataUsage = command.DataUsage;
            this.DataUsageUnit = command.DataUsageUnit;
            this.IDataUsageUnit = command.IDataUsageUnit;
            this.IDataUsage = command.IDataUsage;
            this.UsageDataAmount = command.UsageDataAmount;
            this.IUsageDataAmount = command.IUsageDataAmount;
            this.IsJoinedPayment = command.IsJoinedPayment;
            this.IsMainPaymentChannel = command.IsMainPaymentChannel;

            this.CreatedDate = command.CreatedDate;
            this.CreatedBy = command.CreatedBy;

            this.ReceiptVoucherLineTaxes = new List<CreateReceiptVoucherLineTaxCommand>();
            this.ReductionDetails = new List<ReductionDetailCommand>();
            this.AttachmentFiles = new List<AttachmentFileCommand>();
            this.PriceBusTables = new List<CUChannelPriceBusTableCommand>();
            this.BusTablePricingCalculators = new List<CUBusTablePricingCalculatorCommand>();
        }

        public int Id { get; set; }

        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }

        public int ReceiptVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public int? ProjectId { get; set; }
        public string ServiceName { get; set; }
        public string CId { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }

        public bool HasDistinguishBandwidth { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }

        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public int UsingMonths { get; set; }
        public float TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal? OrgPackagePrice { get; set; }
        public decimal? PackagePrice { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal ReductionFee { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public decimal DiscountAmountSuspend { get; set; }
        public string SPSuspensionTimeIds { get; set; }
        public int PricingType { get; set; }
        public decimal OverloadUsageDataPrice { get; set; }
        public decimal IOverloadUsageDataPrice { get; set; }
        public decimal ConsumptionBasedPrice { get; set; }
        public decimal IConsumptionBasedPrice { get; set; }
        public decimal DataUsage { get; set; }
        public decimal DataUsageUnit { get; set; }
        public decimal IDataUsageUnit { get; set; }
        public decimal IDataUsage { get; set; }
        public decimal UsageDataAmount { get; set; }
        public decimal IUsageDataAmount { get; set; }
        public bool IsJoinedPayment { get; set; }
        public bool IsMainPaymentChannel { get; set; }
        public string DiscountDescription { get; set; }

        public List<CreateReceiptVoucherLineTaxCommand> ReceiptVoucherLineTaxes { get; set; }
        public List<ReductionDetailCommand> ReductionDetails { get; set; }

        public void CalculateTotal()
        {
            this.GrandTotalBeforeTax
                = this.SubTotalBeforeTax
                - this.PromotionAmount
                + this.InstallationFee
                + this.EquipmentTotalAmount
                + this.OtherFeeTotal;
            this.GrandTotal = this.GrandTotalBeforeTax + this.TaxAmount;
            this.SubTotal = this.SubTotalBeforeTax + this.TaxAmount;
        }

        public List<AttachmentFileCommand> AttachmentFiles { get; set; }
        public List<CUChannelPriceBusTableCommand> PriceBusTables { get; set; }
        public List<CUBusTablePricingCalculatorCommand> BusTablePricingCalculators { get; set; }
        public int[] DeletedAttachments { get; set; }
        public string IdentityGuid { get; set; }

        public CUReceiptVoucherDetailCommand ShallowCopy()
        {
            var clone = (CUReceiptVoucherDetailCommand)this.MemberwiseClone();
            clone.AttachmentFiles = new List<AttachmentFileCommand>();
            clone.PriceBusTables = new List<CUChannelPriceBusTableCommand>();
            clone.BusTablePricingCalculators = new List<CUBusTablePricingCalculatorCommand>();
            clone.ReductionDetails = new List<ReductionDetailCommand>();

            return clone;
        }
    }

    public class ReductionDetailCommand
    {
        public int Id { get; set; }
        public string ReasonId { get; set; }
        public string ReductionReason { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public string Duration { get; set; }
        public string CId { get; set; }
    }
}
