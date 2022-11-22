using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using GenericRepository.Extensions;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("TransactionServicePackages")]
    public class TransactionServicePackage : DeploymentChannel<
        TransactionEquipment,
        TransactionChannelPoint,
        TransactionChannelTax,
        TransactionServiceLevelAgreement,
        TransactionPromotionForContract,
        TransactionPriceBusTable
        >, IBind, IRequest
    {
        public TransactionServicePackage()
        {
        }

        public TransactionServicePackage(CUTransactionServicePackageCommand command)
        {

            OutContractServicePackageId = command.OutContractServicePackageId;
            IsOld = command.IsOld;
            IsAcceptanced = command.IsAcceptanced;

            Binding(command);
        }

        public TransactionServicePackage Update(CUTransactionServicePackageCommand updateCommand, bool forceBind = false)
        {
            if (updateCommand.IsDeleted != true)
            {
                Binding(updateCommand, true);
            }
            else
            {
                StatusId = updateCommand.StatusId;
                IsDeleted = updateCommand.IsDeleted;
            }

            return this;
        }
        public int OutContractServicePackageId { get; set; }
        public int TransactionId { get; set; }

        public bool? IsOld { get; set; }
        public int? OldId { get; set; }
        public bool? IsAcceptanced { get; set; }
        public bool NeedEnterStartPoint { get; set; }
        public bool IsMultipleTransaction { get; set; }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUTransactionServicePackageCommand transSrvPackageCommand)
            {
                this.OutContractServicePackageId = transSrvPackageCommand.OutContractServicePackageId;
                this.TransactionId = transSrvPackageCommand.TransactionId;
                this.OutContractId = transSrvPackageCommand.OutContractId;
                this.IsTechnicalConfirmation = transSrvPackageCommand.IsTechnicalConfirmation;
                this.IsSupplierConfirmation = transSrvPackageCommand.IsSupplierConfirmation;
                this.IsMultipleTransaction = transSrvPackageCommand.IsMultipleTransaction;

                if (transSrvPackageCommand.OldId.HasValue)
                    OldId = transSrvPackageCommand.OldId;

                base.ForceBinding(transSrvPackageCommand);
                this.BindingReferences(transSrvPackageCommand);
            }
        }
        private void BindingReferences(CUTransactionServicePackageCommand command)
        {
            if (this.HasStartAndEndPoint)
            {
                if (!command.CreateFromCustomer)
                {
                    // Thêm mới hoặc cập nhật điểm đầu nếu có
                    if (command.StartPoint != null)
                    {
                        AddOrUpdateStartPoint<CUTransactionChannelPointCommand, CUTransactionEquipmentCommand>
                            (command.StartPoint);
                    }
                }
                else
                {
                    NeedEnterStartPoint = true;
                }
            }

            if (command.EndPoint != null)
            {
                AddOrUpdateEndPoint<CUTransactionChannelPointCommand, CUTransactionEquipmentCommand>
                       (command.EndPoint);
            }

            if (command.TimeLine.SuspensionStartDate.HasValue)
            {
                SetStartSuspensionDate(command.TimeLine.SuspensionStartDate.Value);
            }

            if (command.PriceBusTables.Any())
            {
                command.PriceBusTables.ForEach(AddOrUpdatePriceBusTable);
            }
            var removePriceBusTableIds = PriceBusTables.Where(t => command.PriceBusTables.All(p => p.Id != t.Id))
                .Select(t => t.Id);
            RemovePriceBusTables(removePriceBusTableIds);

            if (command.TransactionChannelTaxes.Any())
            {
                command.TransactionChannelTaxes
                    .ForEach(AddOrUpdateTaxValue);
            }

            var removedIds = this.TaxValues.Where(t =>
                   command.TransactionChannelTaxes.All(c => c.TaxCategoryId != t.TaxCategoryId))
                .Select(t => t.TaxCategoryId);

            RemoveTaxValues(removedIds);

            if (command.ServiceLevelAgreements.Any())
            {
                command.ServiceLevelAgreements.ForEach(AddOrUpdateSLA);
            }

            var removeSLAIds = this.ServiceLevelAgreements.Where(
                    s => s.Id != 0 && command.ServiceLevelAgreements.All(c => c.Id != s.Id))
                .Select(s => s.Id);
            RemoveSLAs(removeSLAIds);

            #region Cập nhật khuyến mại

            if (command.TransactionPromotionForContracts != null
                && command.TransactionPromotionForContracts.Count > 0)
            {
                command.TransactionPromotionForContracts.ForEach(c =>
                {
                    c.IsApplied = false;
                    AddPromomotion(c);
                });
            }
            #endregion
        }

        public void SetStartSuspensionDate(DateTime startSuspensionDate)
        {
            this.TimeLine.SuspensionStartDate = startSuspensionDate.ToExactLocalDate();
        }
        public void SetEndSuspensionDate(DateTime endSuspensionDate)
        {
            this.TimeLine.SuspensionEndDate = endSuspensionDate.ToExactLocalDate();
        }

        public override void SetPromotionAmount(int? promotionType, decimal value, int num)
        {
            // Ingore handling
        }

        public override void SetPromotionDate(int? promoType, decimal promoQuantity)
        {
            // Ingore handling
        }

        public override void UnSetPromotionDate(int? promoType, decimal promoQuantity)
        {
            // Ingore handling
        }

        public override void SetPrepayPeriod(int prepayMonth)
        {
            this.TimeLine.PrepayPeriod = prepayMonth;
        }

        public override void SetEffectiveDate(DateTime timeValue)
        {
            this.TimeLine.Effective = timeValue;
        }

        public override void SetStartBillingDate(DateTime timeValue)
        {
            this.TimeLine.StartBilling = timeValue;
        }

        public void SetLatestBillingDate(DateTime timeValue)
        {
            this.TimeLine.LatestBilling = timeValue;
        }

        public void SetSuspensionStartDate(DateTime timeValue)
        {
            this.TimeLine.SuspensionStartDate = timeValue;
        }
        public void SetSuspensionEndDate(DateTime timeValue)
        {
            this.TimeLine.SuspensionEndDate = timeValue;
        }
        public void SetTerminateDate(DateTime timeValue)
        {
            this.TimeLine.TerminateDate = timeValue;
        }
    }
}
