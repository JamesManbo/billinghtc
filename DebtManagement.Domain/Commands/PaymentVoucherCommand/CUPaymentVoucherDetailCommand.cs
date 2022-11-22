using DebtManagement.Domain.Commands.Commons;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.PaymentVoucherCommand
{
    public class CUPaymentVoucherDetailCommand
    {
        public string OutContractIds { get; set; }
        public int? OutContractId { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }
        public CUPaymentVoucherDetailCommand()
        {
            this.PaymentVoucherLineTaxes = new List<CreatePaymentVoucherLineTaxCommand>();
            this.AttachmentFiles = new List<AttachmentFileCommand>();
        }

        public CUPaymentVoucherDetailCommand(CUPaymentVoucherDetailCommand command)
        {
            this.CurrencyUnitId = command.CurrencyUnitId;
            this.CurrencyUnitCode = command.CurrencyUnitCode;
            this.PaymentVoucherId = command.PaymentVoucherId;
            this.ServiceId = command.ServiceId;
            this.ServiceName = command.ServiceName;
            this.CId = command.CId;
            this.ServicePackageId = command.ServicePackageId;
            this.ServicePackageName = command.ServicePackageName;
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
            
            this.OutContractServicePackageId = command.OutContractServicePackageId;
            this.IsFirstDetailOfService = command.IsFirstDetailOfService;

            this.CreatedDate = command.CreatedDate;
            this.CreatedBy = command.CreatedBy;


            this.HasDistinguishBandwidth = command.HasDistinguishBandwidth;
            this.HasStartAndEndPoint = command.HasStartAndEndPoint;
            this.DomesticBandwidth = command.DomesticBandwidth;
            this.InternationalBandwidth = command.InternationalBandwidth;

            this.PaymentVoucherLineTaxes = new List<CreatePaymentVoucherLineTaxCommand>();
            this.AttachmentFiles = new List<AttachmentFileCommand>();
        }

        public int Id { get; set; }

        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }

        public int PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string CId { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
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

        public List<CreatePaymentVoucherLineTaxCommand> PaymentVoucherLineTaxes { get; set; }

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
        public int[] DeletedAttachments { get; set; }
        public string IdentityGuid { get; set; }
    }
}
