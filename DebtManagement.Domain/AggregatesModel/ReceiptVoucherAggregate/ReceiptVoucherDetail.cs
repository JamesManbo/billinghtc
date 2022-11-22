using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models.ReportModels;
using DebtManagement.Domain.Seed;
using GenericRepository.Extensions;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVoucherDetails")]
    public class ReceiptVoucherDetail : Entity
    {
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        [StringLength(256)]
        public string CurrencyUnitCode { get; set; }
        public int? ReceiptVoucherId { get; set; }
        public int? ServiceId { get; set; }
        [StringLength(256)] public string ServiceName { get; set; }
        public int? ServicePackageId { get; set; }
        public int? ProjectId { get; set; }

        [StringLength(256)] public string ServicePackageName { get; set; }
        [StringLength(68)] public string DomesticBandwidth { get; set; }
        [StringLength(68)] public string InternationalBandwidth { get; set; }
        public string CId { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public DateTime? EndBillingDate { get; set; }
        public int UsingMonths { get; set; }
        public decimal OrgPackagePrice { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal PromotionAmount { get; set; }
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
        public int OutContractServicePackageId { get; set; }
        public bool IsFirstDetailOfService { get; set; }
        public decimal DiscountAmountSuspend { get; set; }
        /// <summary>
        /// Danh sách mã giảm trừ tạm ngưng
        /// </summary>
        public string SPSuspensionTimeIds { get; set; }
        /// <summary>
        /// Danh sách lý do giảm trừ
        /// </summary>
        public string DiscountDescription { get; set; }
        public bool IsAutomaticGenerate { get; set; }
        public bool IsShow { get; set; }
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
        private List<ReceiptVoucherDetailReduction> _receiptVoucherDetailReductions;
        public IReadOnlyCollection<ReceiptVoucherDetailReduction> ReceiptVoucherDetailReductions => _receiptVoucherDetailReductions;

        [IgnoreDataMember]
        public virtual ReceiptVoucher ReceiptVoucher { get; set; }
        private List<ReceiptVoucherLineTax> _receiptVoucherLineTaxes;
        public IReadOnlyCollection<ReceiptVoucherLineTax> ReceiptVoucherLineTaxes => _receiptVoucherLineTaxes;

        public IReadOnlyCollection<ChannelPriceBusTable> PriceBusTables => _priceBusTables.ToList();
        protected readonly List<ChannelPriceBusTable> _priceBusTables;

        public IReadOnlyCollection<BusTablePricingCalculator> BusTablePricingCalculators => _busTablePricingCalculators.ToList();
        protected readonly List<BusTablePricingCalculator> _busTablePricingCalculators;

        public ReceiptVoucherDetail()
        {
            IdentityGuid = Guid.NewGuid().ToString();
            CurrencyUnitId = CurrencyUnit.VND.Id;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
            _receiptVoucherLineTaxes = new List<ReceiptVoucherLineTax>();
            _receiptVoucherDetailReductions = new List<ReceiptVoucherDetailReduction>();
            _priceBusTables = new List<ChannelPriceBusTable>();
            _busTablePricingCalculators = new List<BusTablePricingCalculator>();
        }

        public ReceiptVoucherDetail(CUReceiptVoucherDetailCommand command)
        {
            _receiptVoucherLineTaxes = new List<ReceiptVoucherLineTax>();
            _receiptVoucherDetailReductions = new List<ReceiptVoucherDetailReduction>();
            _priceBusTables = new List<ChannelPriceBusTable>();
            _busTablePricingCalculators = new List<BusTablePricingCalculator>();
            IdentityGuid = string.IsNullOrEmpty(command.IdentityGuid) ? Guid.NewGuid().ToString() : command.IdentityGuid;

            CurrencyUnitId = command.CurrencyUnitId;
            CurrencyUnitCode = command.CurrencyUnitCode;
            ReceiptVoucherId = command.ReceiptVoucherId;
            OutContractServicePackageId = command.OutContractServicePackageId;

            CreatedBy = command.CreatedBy;
            CreatedDate = command.CreatedDate;
            DiscountAmountSuspend = command.DiscountAmountSuspend;

            // Thêm danh mục thuế áp dụng vào phiếu thu
            if (command.ReceiptVoucherLineTaxes != null
                && command.ReceiptVoucherLineTaxes.Any())
            {
                command.ReceiptVoucherLineTaxes.ForEach(AddApplyTax);
            }

            if (command.StartBillingDate.HasValue)
            {
                StartBillingDate = command.StartBillingDate.Value
                    .ToExactLocalDate();
            }

            if (command.EndBillingDate.HasValue)
            {
                EndBillingDate = command.EndBillingDate.Value
                    .ToExactLocalDate();
            }

            if (command.ReductionDetails != null
                && command.ReductionDetails.Any())
            {
                command.ReductionDetails.ForEach(AddDetailReduction);
            }

            if (command.PriceBusTables != null
                && command.PriceBusTables.Any())
            {
                command.PriceBusTables.ForEach(AddOrUpdatePriceBusTable);
            }

            if (command.BusTablePricingCalculators != null
                && command.BusTablePricingCalculators.Any())
            {
                command.BusTablePricingCalculators.ForEach(
                    AddOrUpdateBusTableCalculator);
            }

            Binding(command);
        }

        public void Update(CUReceiptVoucherDetailCommand cUReceiptVoucherDetailCommand)
        {
            UpdatedBy = cUReceiptVoucherDetailCommand.UpdatedBy;
            UpdatedDate = cUReceiptVoucherDetailCommand.UpdatedDate;

            if (cUReceiptVoucherDetailCommand.ReductionDetails != null && cUReceiptVoucherDetailCommand.ReductionDetails.Any())
            {
                cUReceiptVoucherDetailCommand.ReductionDetails.ForEach(UpdateDetailReduction);
                var removedIds = this._receiptVoucherDetailReductions.Where(t =>
                    cUReceiptVoucherDetailCommand.ReductionDetails.All(c => c.ReasonId != t.ReasonId))
                    .Select(t => t.ReasonId);
                RemoveReductionDetails(removedIds);
            }

            Binding(cUReceiptVoucherDetailCommand);
        }

        private void Binding(CUReceiptVoucherDetailCommand cuCommand)
        {
            IsJoinedPayment = cuCommand.IsJoinedPayment;
            if (IsJoinedPayment)
            {
                IsMainPaymentChannel = cuCommand.IsMainPaymentChannel;
                IsShow = cuCommand.IsMainPaymentChannel;
            }
            else
            {
                IsMainPaymentChannel = true;
                IsShow = true;
            }

            ServiceId = cuCommand.ServiceId;
            ServiceName = cuCommand.ServiceName;
            CId = cuCommand.CId;
            ProjectId = cuCommand.ProjectId;

            HasDistinguishBandwidth = cuCommand.HasDistinguishBandwidth;
            HasStartAndEndPoint = cuCommand.HasStartAndEndPoint;
            DomesticBandwidth = cuCommand.DomesticBandwidth?.Trim();
            InternationalBandwidth = cuCommand.InternationalBandwidth?.Trim();

            UsingMonths = cuCommand.UsingMonths;
            IsFirstDetailOfService = cuCommand.IsFirstDetailOfService;
            OutContractServicePackageId = cuCommand.OutContractServicePackageId;

            OrgPackagePrice = cuCommand.OrgPackagePrice ?? 0;
            PackagePrice = cuCommand.PackagePrice ?? 0;
            ServicePackageId = cuCommand.ServicePackageId;
            ServicePackageName = cuCommand.ServicePackageName;
            OtherFeeTotal = cuCommand.OtherFeeTotal;
            OffsetUpgradePackageAmount = cuCommand.OffsetUpgradePackageAmount;
            EquipmentTotalAmount = cuCommand.EquipmentTotalAmount;
            InstallationFee = cuCommand.InstallationFee;

            DiscountAmountSuspend = cuCommand.DiscountAmountSuspend;
            ReductionFee = cuCommand.ReductionFee;
            DiscountDescription = cuCommand.DiscountDescription;
            SPSuspensionTimeIds = cuCommand.SPSuspensionTimeIds;

            PricingType = cuCommand.PricingType;
            OverloadUsageDataPrice = cuCommand.OverloadUsageDataPrice;
            IOverloadUsageDataPrice = cuCommand.IOverloadUsageDataPrice;
            ConsumptionBasedPrice = cuCommand.ConsumptionBasedPrice;
            IConsumptionBasedPrice = cuCommand.IConsumptionBasedPrice;
            DataUsage = cuCommand.DataUsage;
            DataUsageUnit = cuCommand.DataUsageUnit;
            IDataUsageUnit = cuCommand.IDataUsageUnit;
            IDataUsage = cuCommand.IDataUsage;
            UsageDataAmount = cuCommand.UsageDataAmount;
            IUsageDataAmount = cuCommand.IUsageDataAmount;

            PromotionAmount = cuCommand.PromotionAmount;

            if (this.IsJoinedPayment)
            {
                TaxPercent = ReceiptVoucherLineTaxes.Sum(c => c.TaxValue);
                SubTotalBeforeTax = BusTablePricingCalculators.Sum(b => b.TotalAmount);

                GrandTotalBeforeTax = CurrencyUnit.RoundByCurrency(this.CurrencyUnitId, SubTotalBeforeTax
                    + InstallationFee
                    + OtherFeeTotal
                    + EquipmentTotalAmount);


                TaxAmount = GrandTotalBeforeTax * (decimal)TaxPercent / 100;

                SubTotal = SubTotalBeforeTax + TaxAmount;
                GrandTotal = GrandTotalBeforeTax + TaxAmount;

                if (ReductionFee > 0)
                {
                    if (this.GrandTotal > this.ReductionFee)
                    {
                        this.GrandTotal -= this.ReductionFee;
                        if (this.PromotionAmount > 0)
                        {
                            if (this.GrandTotal > this.PromotionAmount)
                            {
                                this.GrandTotal -= this.PromotionAmount;
                            }
                            else
                            {
                                this.PromotionAmount = this.GrandTotal;
                                this.GrandTotal = 0;
                            }
                        }
                    }
                    else
                    {
                        this.ReductionFee = this.GrandTotal;
                        this.GrandTotal = 0;
                    }
                }
            }
            else
            {
                TaxPercent = cuCommand.TaxPercent;
                TaxAmount = cuCommand.TaxAmount;

                SubTotalBeforeTax = cuCommand.SubTotalBeforeTax;
                SubTotal = cuCommand.SubTotal;

                GrandTotalBeforeTax = cuCommand.GrandTotalBeforeTax;
                GrandTotal = cuCommand.GrandTotal;
            }

            if (this.Id > 0 && this.EndBillingDate.HasValue
                && this.EndBillingDate.Value.Date != cuCommand.EndBillingDate.Value.Date)
            {
                var changeBillingDateEvent = new VoucherBillingDateChangeDomainEvent()
                {
                    ChannelId = this.OutContractServicePackageId,
                    OldEndingBillingDate = this.EndBillingDate.Value,
                    NewEndingBillingDate = cuCommand.EndBillingDate.Value
                };
                AddDomainEvent(changeBillingDateEvent);

                this.EndBillingDate = cuCommand.EndBillingDate;
            }
        }

        public void AddApplyTax(CreateReceiptVoucherLineTaxCommand taxCommand)
        {
            var newVoucherTax = new ReceiptVoucherLineTax(taxCommand);
            newVoucherTax.CreatedBy = this.CreatedBy;
            newVoucherTax.CreatedDate = this.CreatedDate;
            _receiptVoucherLineTaxes.Add(newVoucherTax);
        }

        public void AddDetailReduction(ReductionDetailCommand reduction)
        {
            var newDetailReduction = new ReceiptVoucherDetailReduction(reduction);
            newDetailReduction.ReceiptVoucherId = this.ReceiptVoucherId;
            newDetailReduction.ReceiptVoucherDetailId = this.Id;
            newDetailReduction.CreatedBy = this.CreatedBy;
            _receiptVoucherDetailReductions.Add(newDetailReduction);
        }
        public void UpdateDetailReduction(ReductionDetailCommand reduction)
        {
            var existModel = _receiptVoucherDetailReductions.Find(r => r.ReasonId == reduction.ReasonId);
            if (existModel != null)
            {
                existModel.Binding(reduction);
                existModel.UpdatedDate = DateTime.Now;
            }

            else AddDetailReduction(reduction);
        }

        public void RemoveReductionDetails(IEnumerable<string> cIds)
        {
            this._receiptVoucherDetailReductions.RemoveAll(t => cIds.Contains(t.ReasonId));
        }
        public void AddOrUpdatePriceBusTable(CUChannelPriceBusTableCommand command)
        {
            if (command.Id == 0)
            {
                var newPriceBustable = new ChannelPriceBusTable(command)
                {
                    CreatedBy = this.CreatedBy
                };
                _priceBusTables.Add(newPriceBustable);
            }
            else
            {
                var existedPriceBusTable = _priceBusTables.Find(p => p.Id == command.Id);
                existedPriceBusTable.Binding(command);
                existedPriceBusTable.UpdatedBy = this.UpdatedBy;
            }

        }
        public void AddOrUpdateBusTableCalculator(CUBusTablePricingCalculatorCommand command)
        {
            if (command.Id == 0)
            {
                var newBustableCalculator = new BusTablePricingCalculator(command)
                {
                    CreatedBy = this.CreatedBy
                };
                _busTablePricingCalculators.Add(newBustableCalculator);
            }
            else
            {
                var existedCalculator = _busTablePricingCalculators.Find(p => p.Id == command.Id);
                existedCalculator.Binding(command);
                existedCalculator.UpdatedBy = this.UpdatedBy;
            }
        }

        public void CalculateSubTotalByFixedPrice()
        {
            this.SubTotalBeforeTax = 0;
            if (this.UsingMonths > 0)
            {
                this.SubTotalBeforeTax = this.PackagePrice * this.UsingMonths;
                this.TaxAmount = this.TaxAmount * this.UsingMonths;
            }
            else if (this.StartBillingDate.HasValue && this.EndBillingDate.HasValue)
            {
                var startDay = this.StartBillingDate.Value.Day;
                var startMonth = this.StartBillingDate.Value.Month;
                var startYear = this.StartBillingDate.Value.Year;

                var diffYears =
                    this.EndBillingDate.Value.Year -
                    this.StartBillingDate.Value.Year;
                var endDay = this.EndBillingDate.Value.Day;
                var endMonth = this.EndBillingDate.Value.Month + diffYears * 12;

                if (startMonth == endMonth)
                {
                    var daysOfMonth = DateTime.DaysInMonth(startYear, startMonth);
                    var usedDays = endDay - startDay + 1;
                    this.SubTotalBeforeTax =
                        (this.PackagePrice / daysOfMonth) * usedDays;
                }
                else
                {
                    for (var idx = startMonth; idx <= endMonth; idx++)
                    {
                        var yearIdx = idx / 12;
                        var daysOfMonth = DateTime.DaysInMonth(
                            startYear + yearIdx,
                            idx - 12 * yearIdx
                        );
                        if (idx == startMonth)
                        {
                            this.SubTotalBeforeTax +=
                                (this.PackagePrice / daysOfMonth) *
                                (daysOfMonth - startDay + 1);
                        }
                        else if (idx == endMonth)
                        {
                            this.SubTotalBeforeTax +=
                                (this.PackagePrice / daysOfMonth) * endDay;
                        }
                        else if (idx > startMonth && idx < endMonth)
                        {
                            this.SubTotalBeforeTax += this.PackagePrice;
                        }
                    }
                }
            }

            this.SubTotalBeforeTax = CurrencyUnit.RoundByCurrency(
                this.CurrencyUnitId,
                this.SubTotalBeforeTax
            );
        }
    }
}