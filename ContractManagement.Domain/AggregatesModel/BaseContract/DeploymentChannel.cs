using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using ContractManagement.Domain.Utilities;
using ContractManagement.Utility;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    public abstract class DeploymentChannel<TEquipment,
        TPoint,
        TTax,
        TSla,
        TPromotion,
        TPriceBusTable
        > : Entity
        where TEquipment : DeploymentEquipment, IBind, new()
        where TPoint : DeploymentChannelPoint<TEquipment>, IBind, new()
        where TTax : TaxValueAbstraction, IBind, new()
        where TSla : ServiceLevelAgreementAbstraction, IBind, new()
        where TPromotion : AppliedPromotionAbstraction, IBind, new()
        where TPriceBusTable : DeploymentChannelPackagePriceBusTable, IBind, new()
    {
        public string Uid { get; set; }
        public ServiceChannelType Type { get; set; }
        public int? OutContractId { get; set; }
        /// <summary>
        /// Số thứ tự của kênh tại hợp đồng bao gồm cả các giao dịch liên quan đến hợp đồng
        /// </summary>
        public int ChannelIndex { get; set; }
        public int? InContractId { get; set; }
        public int? ProjectId { get; set; }
        public string CableRoutingNumber { get; set; } // số hiệu tuyến cáp
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? ServicePackageId { get; set; }
        [Required] public int ServiceId { get; set; }
        [Required] [StringLength(256)] public string ServiceName { get; set; }
        [StringLength(256)] public string PackageName { get; set; }
        public bool IsFreeStaticIp { get; set; }
        [StringLength(256)] public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        [StringLength(256)] public string CustomerCode { get; set; }
        [StringLength(256)] public string CId { get; set; }
        [StringLength(256)] public string RadiusAccount { get; set; }
        [StringLength(256)] public string RadiusPassword { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal OrgPackagePrice { get; set; }
        public decimal PackagePrice { get; set; }
        public float LineQuantity { get; set; } // số lượng tuyến cáp
        public float? CableKilometers { get; set; } // số kilomet cáp
        public int? StartPointChannelId { get; set; }
        public int? EndPointChannelId { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal EquipmentAmount { get; set; }
        public int StatusId { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public float TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public bool IsInFirstBilling { get; set; }
        public int? RadiusServerId { get; set; }
        public int ChannelGroupId { get; set; }
        public int PaymentTargetId { get; set; }
        public int FlexiblePricingTypeId { get; set; }
        public decimal? MaxSubTotal { get; set; } // Giá dịch vụ tối đa
        public decimal? MinSubTotal { get; set; } // Giá dịch vụ tối thiểu
        public byte IsDefaultSLAByServiceId { get; set; }
        public bool IsRadiusAccountCreated { get; set; }
        public bool IsHasServicePackage { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public bool? IsSupplierConfirmation { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        [StringLength(1000)] public string Note { get; set; }

        [StringLength(1000)] public string OtherNote { get; set; }

        public virtual Contractor PaymentTarget { get; set; }
        public virtual OutContract OutContract { get; set; }
        public IReadOnlyCollection<TTax> TaxValues => _taxValues;
        protected readonly List<TTax> _taxValues;

        public IReadOnlyCollection<TSla> ServiceLevelAgreements => _serviceLevelAgreements.ToList();
        protected readonly List<TSla> _serviceLevelAgreements;

        public IReadOnlyCollection<TPromotion> AppliedPromotions => _appliedPromotions.ToList();
        protected readonly List<TPromotion> _appliedPromotions;

        public IReadOnlyCollection<TPriceBusTable> PriceBusTables => _priceBusTables.ToList();
        protected readonly List<TPriceBusTable> _priceBusTables;

        public TPoint StartPoint { get; set; }
        public TPoint EndPoint { get; set; }

        public DeploymentChannel()
        {
            StatusId = OutContractServicePackageStatus.Undeveloped.Id;
            TimeLine = new BillingTimeLine();
            LineQuantity = 1;
            FlexiblePricingTypeId = FlexiblePricingType.FixedPricing.Id;

            _serviceLevelAgreements = new List<TSla>();
            _taxValues = new List<TTax>();
            _appliedPromotions = new List<TPromotion>();
            _priceBusTables = new List<TPriceBusTable>();

            CurrencyUnitId = CurrencyUnit.VND.Id;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
            CreatedDate = DateTime.Now;
            IsInFirstBilling = true;
        }

        private void BindingNonSensitiveProperties<ICommand>(ICommand cuChannelCommand)
            where ICommand : CUDeploymentChannelCommand
        {
            if (cuChannelCommand.ServiceId <= 0
                || string.IsNullOrWhiteSpace(cuChannelCommand.ServiceName))
            {
                throw new ContractDomainException("Service invalid");
            }

            PaymentTargetId = cuChannelCommand.PaymentTargetId;
            TransactionServicePackageId = cuChannelCommand.TransactionServicePackageId;
            //UpdatePaymentTarget(cuChannelCommand.PaymentTarget);

            Uid = cuChannelCommand.Uid;
            Type = cuChannelCommand.Type;
            ProjectId = cuChannelCommand.ProjectId;
            if (cuChannelCommand.OutContractId.HasValue
                && cuChannelCommand.OutContractId > 0)
            {
                OutContractId = cuChannelCommand.OutContractId;
                Type = ServiceChannelType.Output;
            }

            if (cuChannelCommand.InContractId.HasValue
                && cuChannelCommand.InContractId > 0)
            {
                InContractId = cuChannelCommand.InContractId;
                Type = ServiceChannelType.Input;
            }

            CurrencyUnitId = cuChannelCommand.CurrencyUnitId > 0
                ? cuChannelCommand.CurrencyUnitId : CurrencyUnit.VND.Id;
            CurrencyUnitCode = !string.IsNullOrEmpty(cuChannelCommand.CurrencyUnitCode)
                ? cuChannelCommand.CurrencyUnitCode : CurrencyUnit.VND.CurrencyUnitCode;

            ServiceId = cuChannelCommand.ServiceId;
            ServiceName = cuChannelCommand.ServiceName;
            IsTechnicalConfirmation = cuChannelCommand.IsTechnicalConfirmation;
            IsSupplierConfirmation = cuChannelCommand.IsSupplierConfirmation;

            if (!cuChannelCommand.ServicePackageId.HasValue
                || cuChannelCommand.ServicePackageId == 0)
            {
                IsHasServicePackage = false;
            }
            else
            {
                IsHasServicePackage = true;
                ServicePackageId = cuChannelCommand.ServicePackageId;
                PackageName = cuChannelCommand.PackageName;
            }

            IsDefaultSLAByServiceId = cuChannelCommand.IsDefaultSLAByServiceId;
            PromotionAmount = cuChannelCommand.PromotionAmount ?? 0;
            BandwidthLabel = cuChannelCommand.BandwidthLabel;
            InternationalBandwidth = cuChannelCommand.InternationalBandwidth;
            DomesticBandwidth = cuChannelCommand.DomesticBandwidth;
            InternationalBandwidthUom = cuChannelCommand.InternationalBandwidthUom;
            DomesticBandwidthUom = cuChannelCommand.DomesticBandwidthUom;
            CId = cuChannelCommand.CId;
            ChannelGroupId = cuChannelCommand.ChannelGroupId ?? 0;
            RadiusAccount = cuChannelCommand.RadiusAccount;
            RadiusPassword = cuChannelCommand.RadiusPassword;
            StartPointChannelId = cuChannelCommand.StartPointChannelId;
            EndPointChannelId = cuChannelCommand.EndPointChannelId;

            OtherFee = cuChannelCommand.OtherFee;

            IsFreeStaticIp = cuChannelCommand.IsFreeStaticIp;

            PackageName = cuChannelCommand.PackageName;

            //PromotionAmount = servicePackageCommand.PromotionAmount;
            OrgPackagePrice = cuChannelCommand.OrgPackagePrice;
            FlexiblePricingTypeId = cuChannelCommand.FlexiblePricingTypeId;

            LineQuantity = cuChannelCommand.LineQuantity;
            CableKilometers = cuChannelCommand.CableKilometers;

            HasStartAndEndPoint = cuChannelCommand.HasStartAndEndPoint;
            HasDistinguishBandwidth = cuChannelCommand.HasDistinguishBandwidth;
            StatusId = cuChannelCommand.StatusId;

            MaxSubTotal = cuChannelCommand.MaxSubTotal;
            MinSubTotal = cuChannelCommand.MinSubTotal;

            if (!HasStartAndEndPoint)
            {
                PackagePrice = cuChannelCommand.PackagePrice ?? 0;
                InstallationFee = cuChannelCommand.InstallationFee;
            }

            Note = cuChannelCommand.Note;
            OtherNote = cuChannelCommand.OtherNote;
        }

        public void Binding<ICommand>(ICommand cuChannelCommand)
            where ICommand : CUDeploymentChannelCommand
        {
            this.BindingNonSensitiveProperties(cuChannelCommand);

            SetPaymentForm(cuChannelCommand.TimeLine.PaymentForm);

            SetPaymentPeriod(cuChannelCommand.TimeLine.PaymentPeriod);

            SetSignedDate(cuChannelCommand.TimeLine.Signed);

            SetPrepayPeriod(cuChannelCommand.TimeLine.PrepayPeriod);

            if (cuChannelCommand.TimeLine.Effective.HasValue)
            {
                SetEffectiveDate(cuChannelCommand.TimeLine.Effective.Value);
            }

            if (cuChannelCommand.TimeLine.StartBilling.HasValue)
            {
                SetStartBillingDate(cuChannelCommand.TimeLine.StartBilling.Value);
            }
        }
        public void ForceBinding<ICommand>(ICommand command)
            where ICommand : CUDeploymentChannelCommand
        {
            this.BindingNonSensitiveProperties(command);
            this.TimeLine.PaymentForm = command.TimeLine.PaymentForm;
            this.TimeLine.PaymentPeriod = command.TimeLine.PaymentPeriod;
            this.TimeLine.PrepayPeriod = command.TimeLine.PrepayPeriod;
            this.TimeLine.Signed = command.TimeLine.Signed;
            this.TimeLine.DaysSuspended = command.TimeLine.DaysSuspended;
            this.TimeLine.DaysPromotion = command.TimeLine.DaysPromotion;

            if (command.TimeLine.Effective.HasValue)
            {
                this.TimeLine.Effective = command.TimeLine.Effective.Value;
            }

            if (command.TimeLine.StartBilling.HasValue)
            {
                this.TimeLine.StartBilling = command.TimeLine.StartBilling.Value;
            }

            if (command.TimeLine.NextBilling.HasValue)
            {
                this.TimeLine.NextBilling = command.TimeLine.NextBilling.Value;
            }

            if (command.TimeLine.LatestBilling.HasValue)
            {
                this.TimeLine.LatestBilling = command.TimeLine.LatestBilling.Value;
            }

            if (command.TimeLine.SuspensionStartDate.HasValue)
            {
                this.TimeLine.SuspensionStartDate = command.TimeLine.SuspensionStartDate.Value;
            }

            if (command.TimeLine.SuspensionEndDate.HasValue)
            {
                this.TimeLine.SuspensionEndDate = command.TimeLine.SuspensionEndDate.Value;
            }

            if (command.TimeLine.TerminateDate.HasValue)
            {
                this.TimeLine.TerminateDate = command.TimeLine.TerminateDate.Value;
            }
        }

        public void AddOrUpdateStartPoint<TPointCommand, TEquipmentPoint>(TPointCommand command, bool preventRemoveEquipIfNotUpdate = false)
            where TEquipmentPoint : CUDeploymentEquipmentCommand, IBaseRequest
            where TPointCommand : CUDeploymentChannelPointCommand<TEquipmentPoint>
        {
            if (command == null ||
                command.InstallationAddress == null ||
                string.IsNullOrWhiteSpace(command.InstallationAddress.Street))
            {
                throw new ContractDomainException("Địa chỉ điểm đầu là bắt buộc");
            }

            var channelFactory = new BindingModelFactory<TPoint, TPointCommand>();
            if (command.Id == 0)
            {
                StartPoint = channelFactory.CreateInstance(command);
                StartPoint.PointType = Models.OutContracts.OutputChannelPointTypeEnum.Input;
                command.Equipments.ForEach(StartPoint.AddOrUpdateEquipment);
            }
            else
            {
                StartPoint.Binding(command, true);
                command.Equipments.ForEach(StartPoint.AddOrUpdateEquipment);

                if (!preventRemoveEquipIfNotUpdate)
                {
                    var removedEquipmentIds = StartPoint.Equipments
                        .Where(c => c.Id > 0 && command.Equipments.All(ep => ep.Id != c.Id))
                        .Select(c => c.Id);
                    StartPoint.RemoveEquipments(removedEquipmentIds);
                }
            }
        }

        public void AddOrUpdateEndPoint<TPointCommand, TEquipmentPoint>(TPointCommand command, bool preventRemoveEquipIfNotUpdate = false)
            where TEquipmentPoint : CUDeploymentEquipmentCommand, IBaseRequest
            where TPointCommand : CUDeploymentChannelPointCommand<TEquipmentPoint>
        {
            if (command == null ||
                command.InstallationAddress == null ||
                string.IsNullOrWhiteSpace(command.InstallationAddress.Street))
            {
                throw new ContractDomainException("Địa chỉ điểm cuối là bắt buộc");
            }

            var channelFactory = new BindingModelFactory<TPoint, TPointCommand>();
            if (command.Id == 0)
            {
                EndPoint = channelFactory.CreateInstance(command);
                EndPoint.PointType = Models.OutContracts.OutputChannelPointTypeEnum.Output;
                command.Equipments.ForEach(EndPoint.AddOrUpdateEquipment);
            }
            else
            {
                EndPoint.Binding(command, true);
                command.Equipments.ForEach(EndPoint.AddOrUpdateEquipment);

                if (!preventRemoveEquipIfNotUpdate)
                {
                    var removedEquipmentIds = EndPoint.Equipments
                    .Where(c => c.Id > 0 && command.Equipments.All(ep => ep.Id != c.Id))
                    .Select(c => c.Id);
                    EndPoint.RemoveEquipments(removedEquipmentIds);
                }
            }
        }

        public void AddOrUpdatePriceBusTable<TPriceBusTableCmd>(TPriceBusTableCmd command)
            where TPriceBusTableCmd : DeploymentChannelPriceBusTableCommand, IBaseRequest
        {
            var busTableFactory = new BindingModelFactory<TPriceBusTable, TPriceBusTableCmd>();

            var busTableEntity = busTableFactory.CreateInstance(command);
            if (busTableEntity.Id == 0)
            {
                _priceBusTables.Add(busTableEntity);
            }
            else
            {
                var existedPriceBusTable = _priceBusTables.Find(p => p.Id == command.Id);
                existedPriceBusTable.Binding(command);
            }
        }

        public void SetStatusId(int statusId)
        {
            this.StatusId = statusId;
            //if (this.StatusId == OutContractServicePackageStatus.Developed.Id)
            //{
            //    if (this.HasStartAndEndPoint)
            //    {
            //        foreach (var equipment in this.StartPoint.Equipments)
            //        {
            //            equipment.SetRealUnits(equipment.ExaminedUnit);
            //            equipment.SetStatusId(EquipmentStatus.Deployed.Id);
            //        }
            //    }

            //    foreach (var equipment in this.EndPoint.Equipments)
            //    {
            //        equipment.SetRealUnits(equipment.ExaminedUnit);
            //        equipment.SetStatusId(EquipmentStatus.Deployed.Id);
            //    }
            //}
        }
        public void AddOrUpdateSLA<TSLACommand>(TSLACommand slaCommand)
            where TSLACommand : IBaseRequest
        {
            var bindingModelFactory = new BindingModelFactory<TSla, TSLACommand>();
            var slaModel = bindingModelFactory.CreateInstance(slaCommand);

            if (slaModel.Id == 0)
            {
                this.IsDefaultSLAByServiceId = 0;
                slaModel.Uid = Guid.NewGuid().ToString();
                slaModel.ServiceId = this.ServiceId;
                _serviceLevelAgreements.Add(slaModel);
            }
            else if (slaModel.Id > 0 && !slaModel.IsDefault)
            {
                this.IsDefaultSLAByServiceId = 0;
                if (this._serviceLevelAgreements.Any(a => a.Id == slaModel.Id))
                {
                    var updateModel = _serviceLevelAgreements.Find(s => s.Id == slaModel.Id);
                    updateModel.Binding(slaCommand);
                }
                else
                {
                    var newSlaModel = (TSla)slaModel.DeepClone();
                    slaModel.Uid = Guid.NewGuid().ToString();
                    slaModel.ServiceId = this.ServiceId;
                    _serviceLevelAgreements.Add(newSlaModel);
                }
            }
        }

        public void RemoveSLAs(IEnumerable<int> ids)
        {
            _serviceLevelAgreements.RemoveAll(s => ids.Contains(s.Id));
        }

        public void AddOrUpdateTaxValue<TTaxValueCommand>(TTaxValueCommand taxValueCommand)
            where TTaxValueCommand : IBaseRequest
        {
            var bindingModelFac = new BindingModelFactory<TTax, TTaxValueCommand>();
            var taxValue = bindingModelFac.CreateInstance(taxValueCommand);
            var updateModel = _taxValues.Find(t => t.TaxCategoryId == taxValue.TaxCategoryId);
            if (updateModel == null)
            {
                _taxValues.Add(taxValue);
            }
            else
            {
                updateModel.Binding(taxValueCommand, true);
            }
        }

        public void RemoveTaxValues(IEnumerable<int> ids)
        {
            this._taxValues.RemoveAll(t => ids.Contains(t.TaxCategoryId));
        }
        public void RemovePriceBusTables(IEnumerable<int> ids)
        {
            this._priceBusTables.RemoveAll(t => ids.Contains(t.Id));
        }

        public void RemoveEquipments(List<int> removeIds)
        {
            if (this.StartPointChannelId.HasValue)
            {
                this.StartPoint.RemoveEquipments(removeIds);
            }
            this.EndPoint.RemoveEquipments(removeIds);
        }

        public void RemovePromotions(IEnumerable<int> removeIds)
        {
            _appliedPromotions.ForEach(e =>
            {
                if (e.IsApplied && (e.PromotionValueType == 3 || e.PromotionValueType == 4))
                {
                    UnSetPromotionDate(e.PromotionValueType, e.PromotionValue);
                }
            });

            _appliedPromotions.RemoveAll(e => removeIds.Contains(e.Id));
        }

        public void SetPaymentForm(int paymentForm)
        {
            this.TimeLine.PaymentForm = paymentForm;
        }
        public void SetSignedDate(DateTime signedDate)
        {
            this.TimeLine.Signed = signedDate.ToExactLocalDate();
        }

        public void AddPromomotion<TCommand>(TCommand command)
            where TCommand : IBaseRequest
        {
            var bindingModelFactory = new BindingModelFactory<TPromotion, TCommand>();
            var promotionModel = bindingModelFactory.CreateInstance(command);

            var exitsPromotion = AppliedPromotions
                .SingleOrDefault(o => o.OutContractServicePackageId == promotionModel.OutContractServicePackageId
                        && o.PromotionDetailId == promotionModel.PromotionDetailId);

            if (exitsPromotion == null)
            {
                _appliedPromotions.Add(promotionModel);
                if (promotionModel.IsApplied && (promotionModel.PromotionValueType == 3 || promotionModel.PromotionValueType == 4))
                {
                    SetPromotionDate(promotionModel.PromotionValueType,
                        promotionModel.PromotionValue);
                }
                if (promotionModel.PromotionValueType == 1 || promotionModel.PromotionValueType == 2)
                {
                    SetPromotionAmount(
                        promotionModel.PromotionValueType,
                        promotionModel.PromotionValue,
                        promotionModel.NumberMonthApplied);
                }
            }
        }
        public void SetPaymentPeriod(int period)
        {
            if (period <= 0)
            {
                throw new ContractDomainException("paymentPeriod is not valid");
            }

            this.TimeLine.PaymentPeriod = period;
        }

        public void AddPaymentPeriod(int period)
        {
            if (period < 0)
            {
                throw new ContractDomainException("paymentPeriod is not valid");
            }

            if (this.TimeLine.PaymentPeriod == 0)
            {
                SetPaymentPeriod(period);
                return;
            }

            this.TimeLine.PaymentPeriod += period;

            if (this.TimeLine.StartBilling.HasValue)
                this.TimeLine.NextBilling = this.TimeLine.StartBilling.Value.AddMonths(period);
        }

        /// <summary>
        /// Calculate total amount of the service package
        /// </summary>
        public virtual void CalculateTotal()
        {
            TaxPercent = this.TaxValues.Sum(t => t.TaxValue);
            if (HasStartAndEndPoint)
            {
                PackagePrice = 0;
                InstallationFee = 0;
                OtherFee = 0;
                EquipmentAmount = 0;

                // Tính tổng từ giá của các điểm kết nối
                if (StartPoint.ApplyFeeToChannel)
                {
                    PackagePrice += StartPoint.MonthlyCost;
                    InstallationFee += StartPoint.InstallationFee;
                    OtherFee += StartPoint.OtherFee;
                    EquipmentAmount += StartPoint.EquipmentAmount;
                }

                if (EndPoint.ApplyFeeToChannel)
                {
                    PackagePrice += EndPoint.MonthlyCost;
                    InstallationFee += EndPoint.InstallationFee;
                    OtherFee += EndPoint.OtherFee;
                    EquipmentAmount += EndPoint.EquipmentAmount;
                }
            }
            else
            {
                EquipmentAmount = this.EndPoint.EquipmentAmount;
            }

            SubTotalBeforeTax
                = (PackagePrice * this.TimeLine.PaymentPeriod).RoundByCurrency(this.CurrencyUnitCode);

            SubTotal = SubTotalBeforeTax + (SubTotalBeforeTax * (decimal)TaxPercent / 100).RoundByCurrency(this.CurrencyUnitCode);

            GrandTotalBeforeTax = SubTotalBeforeTax + EquipmentAmount + InstallationFee + OtherFee - PromotionAmount;
            TaxAmount = (GrandTotalBeforeTax * (decimal)TaxPercent / 100).RoundByCurrency(this.CurrencyUnitCode);
            GrandTotal = GrandTotalBeforeTax + TaxAmount;
        }
        public abstract void UnSetPromotionDate(int? promoType, decimal promoQuantity);
        public abstract void SetPromotionDate(int? promoType, decimal promoQuantity);
        public abstract void SetPromotionAmount(int? promotionType, decimal value, int num);
        public abstract void SetPrepayPeriod(int month);
        public abstract void SetEffectiveDate(DateTime timeValue);
        public abstract void SetStartBillingDate(DateTime timeValue);

    }
}
