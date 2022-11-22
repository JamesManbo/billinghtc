using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Events.ContractEvents;
using ContractManagement.Domain.Exceptions;
using GenericRepository.Extensions;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    [Table("OutContractServicePackages")]
    public class OutContractServicePackage : DeploymentChannel<
        ContractEquipment,
        OutputChannelPoint,
        OutContractServicePackageTax,
        ServiceLevelAgreement,
        PromotionForContract,
        ChannelPriceBusTable
        >
    {

        public OutContractServicePackage() : base()
        {
        }

        public OutContractServicePackage(CUOutContractChannelCommand cuChannelCommand, bool forceBind = false)
            : base()
        {
            if (forceBind)
            {
                ForceBind(cuChannelCommand);
            }
            else
            {
                Binding(cuChannelCommand);
            }            
        }

        public void ForceBind(CUOutContractChannelCommand cuChannelCommand)
        {
            base.ForceBinding(cuChannelCommand);
            this.BindingReferences(cuChannelCommand);
        }

        public void Binding(CUOutContractChannelCommand cuChannelCommand)
        {
            base.Binding(cuChannelCommand);
            this.BindingReferences(cuChannelCommand);
        }

        private void BindingReferences(CUOutContractChannelCommand cuChannelCommand)
        {
            if (this.HasStartAndEndPoint)
            {
                // Thêm mới hoặc cập nhật điểm đầu nếu có
                AddOrUpdateStartPoint<CUOutputChannelPointCommand, CUContractEquipmentCommand>
                    (cuChannelCommand.StartPoint, cuChannelCommand.PreventRemoveEquipmentIfNotUpdate);
            }

            // Thêm mới hoặc cập nhật điểm cuối nếu có
            AddOrUpdateEndPoint<CUOutputChannelPointCommand, CUContractEquipmentCommand>
                (cuChannelCommand.EndPoint, cuChannelCommand.PreventRemoveEquipmentIfNotUpdate);

            if (cuChannelCommand.PriceBusTables.Any())
            {
                cuChannelCommand.PriceBusTables.ForEach(AddOrUpdatePriceBusTable);
            }
            var removePriceBusTableIds = PriceBusTables.Where(t => cuChannelCommand.PriceBusTables.All(p => p.Id != t.Id))
                .Select(t => t.Id);
            RemovePriceBusTables(removePriceBusTableIds);

            if (cuChannelCommand.OutContractServicePackageTaxes.Any())
            {
                cuChannelCommand.OutContractServicePackageTaxes
                    .ForEach(AddOrUpdateTaxValue);
            }

            var removedIds = this.TaxValues.Where(t =>
                   cuChannelCommand.OutContractServicePackageTaxes.All(c => c.TaxCategoryId != t.TaxCategoryId))
                .Select(t => t.TaxCategoryId);
            RemoveTaxValues(removedIds);

            if (cuChannelCommand.ServiceLevelAgreements.Any())
            {
                cuChannelCommand.ServiceLevelAgreements.ForEach(AddOrUpdateSLA);
            }

            var removeSLAIds = this.ServiceLevelAgreements.Where(
                    s => cuChannelCommand.ServiceLevelAgreements.All(c => c.Id != s.Id))
                .Select(s => s.Id);
            RemoveSLAs(removeSLAIds);

            #region Cập nhật khuyến mại

            if (cuChannelCommand.PromotionForContractNews != null
                && cuChannelCommand.PromotionForContractNews.Count > 0)
            {
                cuChannelCommand.PromotionForContractNews.ForEach(AddPromomotion);
            }

            if (cuChannelCommand.PromotionForContractDels != null && cuChannelCommand.PromotionForContractDels.Count > 0)
            {
                foreach (var promotionDetail in cuChannelCommand.PromotionForContractDels)
                {
                    if (promotionDetail.PromotionValueType == 3 || promotionDetail.PromotionValueType == 4)
                    {
                        UnSetPromotionDate(promotionDetail.PromotionValueType, promotionDetail.PromotionValue);
                    }

                    // tìm và bỏ áp dụng khuyến mại cho gói cước
                    //var promo = _promotionQueries.GetPromotionForContractById(promotionDetail.Id);
                    //await _promotionForContract.RemoveAndSave(promo);
                    RemovePromotions(cuChannelCommand
                        .PromotionForContractDels
                        .Select(e => e.PromotionForContractId));
                }
            }
            #endregion
        }

        public void SetReplacedStatus()
        {
            this.StatusId = OutContractServicePackageStatus.Replaced.Id;
        }

        public OutContractServicePackage Update(CUOutContractChannelCommand updateCommand, bool forceBind = false)
        {
            if (updateCommand.IsDeleted != true)
            {
                if(forceBind)
                {
                    ForceBind(updateCommand);
                }
                else
                {
                    Binding(updateCommand);
                }
            }
            else
            {
                StatusId = updateCommand.StatusId;
                IsDeleted = updateCommand.IsDeleted;
            }

            return this;
        }

        public override void SetStartBillingDate(DateTime startBillingDate)
        {
            // Không cho phép chỉnh sửa ngày bắt đầu tính cước
            if (!IsTransient() && this.TimeLine.StartBilling != null) return;

            //if (this.TimeLine.Effective.HasValue && startBillingDate.LessThanDate(this.TimeLine.Effective.Value))
            //{
            //    throw new ContractDomainException("Start billing date can not be less than effective date");
            //}

            this.TimeLine.StartBilling = startBillingDate.ToExactLocalDate();

            /// Với hình thức trả trước,
            /// ngày tính cước tiếp theo sẽ là ngày bắt đầu tính cước + số tháng thanh toán trước
            if (this.TimeLine.PaymentForm == (int)PaymentMethodForm.Prepaid)
            {
                SetNextBillingDate(this.TimeLine.StartBilling.Value.AddMonths(this.TimeLine.PrepayPeriod));
            }
            /// Với hình thức trả sau,
            /// ngày tính cước tiếp theo sẽ là ngày bắt đầu tính cước + kỳ hạn thanh toán
            else
            {
                SetNextBillingDate(this.TimeLine.StartBilling.Value
                    .AddMonths(this.TimeLine.PrepayPeriod)
                    .AddMonths(this.TimeLine.PaymentPeriod));
            }

            /// Áp dụng khuyến mại 
            /// nếu khuyến mại có làm thay đổi ngày tính cước kỳ tiếp theo
            this.AppliedPromotions.Where(p => !p.IsDeleted)
                .ToList()
                .ForEach(e =>
                {
                    if (!e.IsApplied && (e.PromotionValueType == 3 || e.PromotionValueType == 4))
                    {
                        SetPromotionDate(e.PromotionValueType, e.PromotionValue);
                    }
                });

            if (this.StatusId == OutContractServicePackageStatus.Undeveloped.Id)
            {
                this.StatusId = OutContractServicePackageStatus.Developed.Id;
            }

            if (!IsTransient() && this.TimeLine.PrepayPeriod > 0)
            {
                AddUpdateTimelineForInitReceiptVoucherEvent();
            }
        }

        public override void SetEffectiveDate(DateTime effectiveDate)
        {
            // Editting is not allowed
            if (!IsTransient() && this.TimeLine.Effective != null) return;

            //if (effectiveDate.LessThanDate(this.TimeLine.Signed))
            //{
            //    throw new ContractDomainException("EffectiveDate can not be less than SignedDate");
            //}

            this.TimeLine.Effective = effectiveDate.ToExactLocalDate();

            // Cập nhật trạng thái của kênh thành đã triển khai
            this.SetStatusId(OutContractServicePackageStatus.Developed.Id);
        }

        private void AddUpdateTimelineForInitReceiptVoucherEvent()
        {
            if (this.Type == ServiceChannelType.Output)
            {
                var updateTimelineForInitVoucherEvent = new UpdateTimelineReceiptVoucherDomainEvent
                {
                    ContractId = this.OutContractId.Value,
                    OutServicePackageId = this.Id,
                    TimeLine = TimeLine
                };
                AddDomainEvent(updateTimelineForInitVoucherEvent);
            }
        }

        private void SetNextBillingDateAfterCanclePromotion(DateTime nextBillingDate)
        {
            if (!TimeLine.NextBilling.HasValue) return;
            TimeLine.NextBilling = nextBillingDate;
        }

        public override void SetPrepayPeriod(int prepayMonth)
        {
            // Chỉ cho phép nhập giá trị thanh toán trước khi tạo mới kênh
            if (!this.IsTransient()) return;

            this.TimeLine.PrepayPeriod = 0;
            // Chỉ xử lý trong trường hợp kênh có đơn giá cố định hàng tháng
            if (this.FlexiblePricingTypeId == FlexiblePricingType.FixedPricing.Id) {

                if (TimeLine.NextBilling.HasValue)
                {
                    SetNextBillingDate(this.TimeLine.NextBilling.Value.AddMonths(prepayMonth - this.TimeLine.PrepayPeriod));
                }
                else
                {
                    if (this.TimeLine.StartBilling.HasValue)
                    {
                        SetNextBillingDate(this.TimeLine.StartBilling.Value.AddMonths(prepayMonth).AddDays(1));
                    }
                }

                this.TimeLine.PrepayPeriod = prepayMonth;
            }
        }

        public void SetNextBillingDate(DateTime nextBillingDate)
        {
            if (!TimeLine.StartBilling.HasValue)
                throw new ContractDomainException("The NextBillingDate value cannot be set if the StartBilling value is null");

            if (!TimeLine.NextBilling.HasValue)
            {
                if (nextBillingDate.Date < TimeLine.StartBilling.Value.Date)
                    throw new ContractDomainException("The new NextBillingDate value cannot be less than the StartBilling value");
            }

            TimeLine.NextBilling = nextBillingDate;
        }

        public void SetIsRadiusUserCreated()
        {
            if (!string.IsNullOrWhiteSpace(this.RadiusAccount)
                && !string.IsNullOrWhiteSpace(this.RadiusPassword))
            {
                this.IsRadiusAccountCreated = true;
            }
        }

        public void SetIsPaidTheFirstBilling()
        {
            this.IsInFirstBilling = false;
        }
        public void SetIsFirstBilling()
        {
            this.IsInFirstBilling = true;
        }
        public void SetNextBillingWhenCancelVoucher(DateTime nextBillingTime)
        {
            this.TimeLine.NextBilling = nextBillingTime;
        }

        public override void UnSetPromotionDate(int? promoType, decimal promoQuantity)
        {
            if (this.TimeLine == null || !this.TimeLine.NextBilling.HasValue)
            {
                return;
            }

            if (promoType == 3)
            {
                SetNextBillingDateAfterCanclePromotion(
                    TimeLine.NextBilling.Value.AddMonths(0 - decimal.ToInt32(promoQuantity))
                    );
            }

            if (promoType == 4)
            {
                SetNextBillingDateAfterCanclePromotion(
                    TimeLine.NextBilling.Value.AddDays(0 - decimal.ToInt32(promoQuantity))
                    );
            }
        }

        public override void SetPromotionDate(int? promoType, decimal promoQuantity)
        {
            if (this.TimeLine == null || !this.TimeLine.NextBilling.HasValue)
            {
                return;
            }

            if (promoType == 3)
            {
                SetNextBillingDate(this.TimeLine.NextBilling.Value.AddMonths(decimal.ToInt32(promoQuantity)));
            }

            if (promoType == 4)
            {
                SetNextBillingDate(this.TimeLine.NextBilling.Value.AddDays(decimal.ToInt32(promoQuantity)));
            }
        }

        public override void SetPromotionAmount(int? promotionType, decimal value, int promotionMonth)
        {
            int timeApplied = promotionMonth >= TimeLine.PrepayPeriod
                ? TimeLine.PrepayPeriod
                : promotionMonth;
            this.PromotionAmount = promotionType == 1 ? this.PackagePrice * value / 100 * timeApplied : value * timeApplied;
            CalculateTotal();
        }
    }
}