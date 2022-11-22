using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Seed;
using GenericRepository.Extensions;

namespace DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate
{
    [Table("PaymentVoucherDetails")]
    public class PaymentVoucherDetail : Entity
    {
        public string CId { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        [StringLength(256)]
        public string CurrencyUnitCode { get; set; }
        [StringLength(68)]
        public int PaymentVoucherId { get; set; }
        public int? ServiceId { get; set; }
        [StringLength(256)]
        public string ServiceName { get; set; }
        public int? ServicePackageId { get; set; }
        [StringLength(256)]
        public string ServicePackageName { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public bool HasStartAndEndPoint { get; set; }

        [StringLength(68)] public string DomesticBandwidth { get; set; }
        [StringLength(68)] public string InternationalBandwidth { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public decimal PackagePrice { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public int? OutContractId { get; set; }
        public int PaymentPeriod { get; set; }
        public string OutContractIds { get; set; }
        public decimal TotalPerMonth { get; set; }
        public string Description { get; set; }

        public decimal EquipmentTotalAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public decimal ReductionFee { get; set; }
        public decimal OtherFeeTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public float TaxPercent { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }

        [IgnoreDataMember]
        public virtual PaymentVoucher PaymentVoucher { get; set; }
        private List<PaymentVoucherLineTax> _paymentVoucherLineTaxes;
        public IReadOnlyCollection<PaymentVoucherLineTax> PaymentVoucherLineTaxes => _paymentVoucherLineTaxes;
        public PaymentVoucherDetail()
        {
            _paymentVoucherLineTaxes = new List<PaymentVoucherLineTax>();
        }

        public PaymentVoucherDetail(CUPaymentVoucherDetailCommand command)
        {
            _paymentVoucherLineTaxes = new List<PaymentVoucherLineTax>();

            IdentityGuid = string.IsNullOrEmpty(command.IdentityGuid)
                ? Guid.NewGuid().ToString()
                : command.IdentityGuid;

            CreatedDate = DateTime.Now;
            CreatedBy = command.CreatedBy;

            if (command.StartBillingDate.HasValue)
            {
                StartBillingDate = command.StartBillingDate.Value.ToExactLocalDate();
            }

            if (command.EndBillingDate.HasValue)
            {
                EndBillingDate = command.EndBillingDate.Value.ToExactLocalDate();
            }

            Binding(command);
        }

        public void Update(CUPaymentVoucherDetailCommand command)
        {
            UpdatedDate = DateTime.Now;
            UpdatedBy = command.UpdatedBy;
            Binding(command);
        }
        private void Binding(CUPaymentVoucherDetailCommand command)
        {

            HasDistinguishBandwidth = command.HasDistinguishBandwidth;
            HasStartAndEndPoint = command.HasStartAndEndPoint;
            DomesticBandwidth = command.DomesticBandwidth?.Trim();
            InternationalBandwidth = command.InternationalBandwidth?.Trim();

            CurrencyUnitId = command.CurrencyUnitId;
            CurrencyUnitCode = command.CurrencyUnitCode;
            PaymentVoucherId = command.PaymentVoucherId;
            OutContractServicePackageId = command.OutContractServicePackageId;

            ServiceId = command.ServiceId;
            ServiceName = command.ServiceName;
            CId = command.CId;

            IsFirstDetailOfService = command.IsFirstDetailOfService;
            OutContractServicePackageId = command.OutContractServicePackageId;
            PaymentVoucherId = command.PaymentVoucherId;

            OutContractId = command.OutContractId;
            OutContractIds = command.OutContractIds;

            PackagePrice = command.PackagePrice ?? 0;
            ServicePackageId = command.ServicePackageId;
            ServicePackageName = command.ServicePackageName;
            OffsetUpgradePackageAmount = command.OffsetUpgradePackageAmount;
            EquipmentTotalAmount = command.EquipmentTotalAmount;
            InstallationFee = command.InstallationFee;

            ReductionFee = command.ReductionFee;
            OtherFeeTotal = command.OtherFeeTotal;
            TaxPercent = command.TaxPercent;
            TaxAmount = command.TaxAmount;

            SubTotalBeforeTax = command.SubTotalBeforeTax;
            SubTotal = command.SubTotal;

            GrandTotalBeforeTax = command.GrandTotalBeforeTax;
            GrandTotal = command.GrandTotal;

            if (this.IsTransient())
            {
                // Thêm danh mục thuế áp dụng vào phiếu thu
                if (command.PaymentVoucherLineTaxes != null
                    && command.PaymentVoucherLineTaxes.Any())
                {
                    command.PaymentVoucherLineTaxes.ForEach(AddApplyTax);
                }
            }

        }

        public void AddApplyTax(CreatePaymentVoucherLineTaxCommand taxCommand)
        {
            var newVoucherTax = new PaymentVoucherLineTax(taxCommand)
            {
                CreatedBy = this.CreatedBy,
                CreatedDate = this.CreatedDate
            };
            _paymentVoucherLineTaxes.Add(newVoucherTax);
        }
    }
}
