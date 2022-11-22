using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Seed;
using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("TransactionPromotionForContracts")]
    public class TransactionPromotionForContract
        : AppliedPromotionAbstraction, IBind
    {
        public int TransactionServicePackageId { get; set; }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUTransactionPromotionForContractCommand promotionCommand)
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
                this.TransactionServicePackageId = promotionCommand.TransactionServicePackageId;
                this.OutContractServicePackageId = promotionCommand.OutContractServicePackageId;
            }
        }
    }
}
