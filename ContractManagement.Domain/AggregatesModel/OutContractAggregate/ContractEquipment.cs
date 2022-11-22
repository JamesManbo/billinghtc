using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using ContractManagement.Domain.Utilities;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    [Table("ContractEquipments")]
    public class ContractEquipment : DeploymentEquipment, IBind
    {        
        public int? TransactionEquipmentId { get; set; }

        public ContractEquipment()
        {
            CurrencyUnitId = CurrencyUnit.VND.Id;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
        }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            switch (command)
            {
                case CUContractEquipmentCommand contractEquipment:
                    base.Binding(contractEquipment);
                    TransactionEquipmentId = contractEquipment.TransactionEquipmentId;
                    break;
                case CUTransactionEquipmentCommand transactionEquipment:
                    base.Binding(transactionEquipment);
                    TransactionEquipmentId = transactionEquipment.Id;
                    break;
                default:
                    throw new ContractDomainException($"The {nameof(CUContractEquipmentCommand)} command is not supported in binding handling");
            }
        }    

        public override void CalculateTotal()
        {
            this.ExaminedSubTotal = this.RealSubTotal = this.ReclaimedSubTotal = this.SubTotal = 0;
            this.ExaminedGrandTotal = this.RealGrandTotal = this.ReclaimedGrandTotal = this.GrandTotal = 0;

            if (this.IsFree)
            {
                return;
            }

            if (this.RealUnit == 0)
            {
                this.ExaminedSubTotal = this.UnitPrice * (decimal)this.ExaminedUnit;
                this.SubTotal = this.ExaminedSubTotal;
            }
            else
            {
                this.RealSubTotal = this.UnitPrice * (decimal)this.RealUnit;
                this.SubTotal = this.RealSubTotal;
            }

            this.ReclaimedSubTotal = this.UnitPrice * (decimal)this.ReclaimedUnit;

            this.ExaminedGrandTotal = this.ExaminedSubTotal;
            this.RealGrandTotal = this.RealSubTotal;
            this.ReclaimedGrandTotal = this.ReclaimedSubTotal;

            this.GrandTotal = this.SubTotal;
        }
    }
}