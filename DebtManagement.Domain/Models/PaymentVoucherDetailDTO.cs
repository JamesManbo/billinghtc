using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.Models.ContractModels;
using DebtManagement.Domain.Models.PaymentVoucherModels;
using DebtManagement.Domain.Models.ReportModels;

namespace DebtManagement.Domain.Models
{
    public class PaymentVoucherDetailDTO : BaseDTO
    {
        public PaymentVoucherDetailDTO()
        {
            this.PaymentVoucherLineTaxes = new List<PaymentVoucherLineTaxDTO>();
            this.AttachmentFiles = new List<AttachmentFileDTO>();
        }
        public string CId { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ReductionFee { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public InstallationAddress StartPointAddress { get; set; }
        public InstallationAddress EndPointAddress { get; set; }
        public List<PaymentVoucherLineTaxDTO> PaymentVoucherLineTaxes { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
        public int? OutContractId { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
