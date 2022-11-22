using DebtManagement.Domain.Commands.Commons;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using DebtManagement.Domain.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ReceiptVoucherDetailDTO : JoinedPaymentInformation
    {
        private decimal subTotalBeforeTax;
        private decimal subTotal;
        private decimal taxAmount;
        private decimal grandTotalBeforeTax;
        private decimal grandTotal;
        private string cId;
        private string serviceName;
        private DateTime? startBillingDate;
        private string domesticBandwidth;
        private string internationalBandwidth;

        public ReceiptVoucherDetailDTO()
        {
            this.ReductionDetails = new List<ReductionDetailDTO>();
            this.ReceiptVoucherLineTaxes = new List<ReceiptVoucherLineTaxDTO>();
            this.AttachmentFiles = new List<AttachmentFileDTO>();
            this.PriceBusTables = new List<ChannelPriceBusTableDTO>();
            this.BusTablePricingCalculators = new List<BusTablePricingCalculatorDTO>();
        }
        public int Id { get; set; }
        public string CId
        {
            get => cId; set
            {
                cId = value;
                NotifyJoinedPropertyChange(nameof(CId), value);

            }
        }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName
        {
            get => serviceName; set
            {
                serviceName = value;
                NotifyJoinedPropertyChange(nameof(ServiceName), value);

            }
        }
        public string DomesticBandwidth
        {
            get => domesticBandwidth; set
            {
                domesticBandwidth = value;
                NotifyJoinedPropertyChange(nameof(DomesticBandwidth), value);
            }
        }
        public string InternationalBandwidth
        {
            get => internationalBandwidth; set
            {
                internationalBandwidth = value;
                NotifyJoinedPropertyChange(nameof(InternationalBandwidth), value);
            }
        }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public DateTime? StartBillingDate
        {
            get => startBillingDate; set
            {
                startBillingDate = value;
                if (value != null)
                {
                    NotifyJoinedPropertyChange(nameof(StartBillingDate), value);
                }
            }
        }
        public DateTime? EndBillingDate { get; set; }
        public int UsingMonths { get; set; }
        public decimal SubTotalBeforeTax
        {
            get => subTotalBeforeTax;
            set
            {
                subTotalBeforeTax = value;
                NotifyJoinedPropertyChange(nameof(SubTotalBeforeTax), value);
            }
        }
        public decimal SubTotal
        {
            get => subTotal; set
            {
                subTotal = value;
                NotifyJoinedPropertyChange(nameof(SubTotal), value);
            }
        }
        public decimal ReductionFee { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public bool IsHasPrice { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount
        {
            get => taxAmount; set
            {
                taxAmount = value;
                NotifyJoinedPropertyChange(nameof(TaxAmount), value);
            }
        }
        public decimal GrandTotalBeforeTax
        {
            get => grandTotalBeforeTax; set
            {
                grandTotalBeforeTax = value;
                NotifyJoinedPropertyChange(nameof(GrandTotalBeforeTax), value);
            }
        }
        public decimal GrandTotal
        {
            get => grandTotal; set
            {
                grandTotal = value;
                NotifyJoinedPropertyChange(nameof(GrandTotal), value);
            }
        }
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal DiscountAmountSuspend { get; set; }
        public string SPSuspensionTimeIds { get; set; }
        public decimal OpeningDebtAmount { get; set; } // Nợ đầu kỳ
        public bool HasDistinguishBandwidth { get; set; }
        public bool HasStartAndEndPoint { get; set; }
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
        public bool IsMainPaymentChannel { get; set; }
        public bool IsJoinedPayment { get; set; }
        public bool IsShow { get; set; }
        public string DiscountDescription { get; set; }
        public InstallationAddress StartPointAddress { get; set; }
        public InstallationAddress EndPointAddress { get; set; }
        public List<ReductionDetailDTO> ReductionDetails { get; set; }
        public List<ReceiptVoucherLineTaxDTO> ReceiptVoucherLineTaxes { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
        public List<ChannelPriceBusTableDTO> PriceBusTables { get; set; }
        public List<BusTablePricingCalculatorDTO> BusTablePricingCalculators { get; set; }
    }

    public class ReductionDetailDTO
    {
        public string Id { get; set; }
        public int ReceiptVoucherDetailId { get; set; }
        public string ReasonId { get; set; }
        public string ReductionReason { get; set; }
        public decimal ReductionFee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public string Duration { get; set; }
        public string CId { get; set; }
    }
}
