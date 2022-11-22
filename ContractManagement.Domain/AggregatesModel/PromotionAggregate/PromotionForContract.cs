using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Seed;
using MediatR;

namespace ContractManagement.Domain.AggregatesModel.PromotionAggregate
{
    [Table("PromotionForContracts")]
    public class PromotionForContract
        : AppliedPromotionAbstraction, IBind
    {

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CreateAppliedPromotionCommand promotionCommand)
            {
                this.PromotionDetailId = promotionCommand.PromotionDetailId;
                this.PromotionValue = promotionCommand.PromotionValue;
                this.PromotionValueType = promotionCommand.PromotionValueType;
                this.IsApplied = promotionCommand.IsApplied;
                this.NumberMonthApplied = promotionCommand.NumberMonthApplied;
                this.PromotionId = promotionCommand.PromotionId;
                this.PromotionName = promotionCommand.PromotionName;
                this.PromotionType = promotionCommand.PromotionType;
                this.PromotionTypeName = promotionCommand.PromotionTypeName;
                this.OutContractServicePackageId = promotionCommand.OutContractServicePackageId;
            }
        }
    }
}
